using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OTSSDK
{
    #region Core OTS

    public class OTSBase(string name) : IOTSBase
    {
        public Guid ID { get; } = Guid.NewGuid();
        public string Name { get; } = name;
    }

    public abstract class OTSTemplateBase<T>(string name) 
        : OTSBase(name), IOTSTemplate<T>
        where T : IOTSComponent
    {
        public abstract T CreateInstance();
    }

    public abstract class OTSObjectBase(string name) : OTSBase(name), IOTSObject { }

    #endregion


    #region IO Nodes

    public class OTSIOBase(string name, Guid componentId, OTSTypes type) : OTSBase(name), IOTSIONodeDefinition
    {
        public IOTSData? Value
        {
            get
            {
                return new OTSData(OTSType, TempValue);
            }

            set
            {
                var pair = value?.TypeValuePair;
                if(pair != null) {

                    TempValue = pair.Item2;
                    OTSType = pair.Item1;

                } else
                {
                    TempValue = null;
                }
            }
        }

        public Guid ComponentId { get; } = componentId;
        public OTSTypes OTSType { get; protected set; } = type;
        private object? TempValue { get; set; }
    }

    //------------------- View -----------------------//
    public class OTSViewBase(string name, IOTSOutput connectee,
        Guid componentId, OTSTypes type) 
        : OTSIOBase(name, componentId, type), IOTSViewDefinition
        { 
            public IOTSOutput Output { get; } = connectee;
        }

    //public class OTSViewTemplate(string name, Guid componentId, OTSTypes type) :
    //    OTSViewBase(name, componentId, type), 
    //    IOTSTemplate<IOTSView>,
    //    IOTSViewTemplate
    //{
    //    public IOTSView CreateInstance() => new OTSView(this);
    //}

    public class OTSView: OTSViewBase, IOTSView
    {
        protected object? _value = (long)0;
        public new IOTSData? Value { get { return new OTSData(OTSType, _value); } set => _value = value as object ?? default; }

        public virtual void Set(IOTSData? data)
        {
            if (data == null) 
            { 
                _value = TypeConversion.GetOTSTypeDefault(OTSType);
            } else
            {
                var raw = data.TypeValuePair;
                _value = raw.Item2;
                OTSType = raw.Item1;
            }
        }
    
        public IOTSData? Peak() => Value;    
    
        public OTSView(string name, IOTSOutput connectee,
            Guid componentId, OTSTypes initialType = OTSTypes.SIGNED) :
            base(name, connectee, componentId, initialType) { }

        public OTSView(IOTSViewTemplate template)
            : base(template.Name, template.Output,  template.ComponentId, template.OTSType) { }
    }

    //------------------- Input -----------------------//
    public class OTSInputBase(string name, Guid componentId, OTSTypes type) : 
        OTSIOBase(name, componentId, type), IOTSInputDefinition
        { }

    public class OTSInputTemplate(string name, Guid componentId, OTSTypes type) :
        OTSInputBase(name, componentId, type), 
        IOTSTemplate<IOTSInput>,
        IOTSInputTemplate
    {
        public IOTSInput CreateInstance() => new OTSInput(this);
    }

    public class OTSInput: OTSInputBase, IOTSInput
    {
        public virtual void Set(IOTSData? data)
        {
            if (data == null) 
            { 
                Value = new OTSData(OTSType);
            } else
            {
                var raw = data.TypeValuePair;
                Value = new OTSData(raw.Item1, raw.Item2);
            }
        }
    
        public IOTSData? Peak() => Value;    
    
        public OTSInput(string name, Guid componentId, OTSTypes initialType = OTSTypes.SIGNED) :
            base(name, componentId, initialType) { }

        public OTSInput(IOTSInputTemplate template)
            : base(template.Name, template.ComponentId, template.OTSType) { }
    }


    //------------------- Output -----------------------//
    public class OTSOutputBase(string name, Guid componentId, OTSTypes type) 
        : OTSIOBase(name, componentId, type), IOTSOutputDefinition
        { }

    public abstract class OTSOutputTemplate<T>(string name, Guid componentId, OTSTypes type) :
        OTSOutputBase(name, componentId, type),
        IOTSTemplate<IOTSOutput>,
        IOTSOutputTemplate
        where T : IOTSOutput
    {
        public abstract IOTSOutput CreateInstance();
    }

    public class OTSOutput(string name, Guid componentId, OTSTypes type) 
        : OTSOutputBase(name, componentId, type), IOTSOutput
    {
    }

    #endregion


    #region Component

    public abstract class OTSComponentBase<TInput, TView, TOutput, TField>
         : OTSBase,
        IOTSComponentDefinition<TInput, TView, TOutput, TField>
        where TInput : IOTSInputDefinition
        where TView : IOTSViewDefinition
        where TOutput : IOTSOutputDefinition
        where TField : IOTSConfigFieldDefinition
    {
        public bool AllowExpansion { get; }
        public Guid LibraryGuid { get; }

        #pragma warning disable IDE0028 // Simplify collection initialization
        public IEnumerable<TInput> Inputs { get; protected set; } = [];
        public IEnumerable<TView> Views { get; protected set; } = new List<TView>();
        public IEnumerable<TOutput> Outputs { get; protected set; } = [];
        public IEnumerable<TField> Fields { get; protected set; } = new List<TField>();
        public Lazy<OTSComponentClass> ComponentClass { get; protected set; }
        #pragma warning restore IDE0028 // Simplify collection initialization

        public OTSComponentBase(string name, Guid libraryGuid, bool allowExpansion) : base(name)
        {
            AllowExpansion = allowExpansion;
            LibraryGuid = libraryGuid;
            ComponentClass =  new Lazy<OTSComponentClass>(() => CommonUtil.IdentifyComponentClass(this));
        }
    }

    public abstract class OTSComponentTemplate<T>(string name, Guid libraryGuid, bool allowExpansion) :
        OTSComponentBase<IOTSInputTemplate, IOTSViewTemplate, IOTSOutputTemplate, IOTSConfigFieldTemplate>
        (name, libraryGuid, allowExpansion),
        IOTSComponentTemplate<T> where T : IOTSComponent
    {
        public abstract T CreateInstance();
    }

    public abstract class OTSComponent :
        OTSComponentBase<IOTSInput, IOTSView, IOTSOutput, IOTSConfigField>,
        IOTSComponent
    {
        public IOTSInput? GetInput(string name) => 
            Inputs.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
        public IOTSOutput? GetOutput(string name) => 
            Outputs.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
        public IOTSView? GetView(string name) => 
            Views.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
        public IOTSConfigField? GetConfig(string name) =>
            Fields.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase));

        public IOTSView? AddViewingInput(IOTSOutput providerNode)
        {
            if(!AllowExpansion) { return null; }
            var view = new OTSView($"View {Views.Count()+1}", providerNode, ID, providerNode.OTSType);
            var config = new OTSConfigField(view.Name, view.OTSType, true);

            ((ICollection<IOTSView>)Views).Add(view);
            ((ICollection<IOTSConfigField>)Fields).Add(config);

            return view;
        }

        public bool RemoveViewingInput(Guid viewId)
        {
            var viewRemove = Views.FirstOrDefault(x => x.ID == viewId);
            var configRemove = Fields.FirstOrDefault(x => Name == viewRemove?.Name);

            return viewRemove != null && configRemove != null &&
                (((ICollection<IOTSViewDefinition>)Views).Remove(viewRemove) &&
                ((ICollection<IOTSConfigField>)Fields).Remove(configRemove));
        }
        
        public OTSComponent(string name, Guid libraryGuid, bool allowExpansion = false) :
            base(name, libraryGuid, allowExpansion)
        { 
            
        }

        public OTSComponent(IOTSComponentTemplate<IOTSComponent> template) : 
            base(template.Name, template.LibraryGuid, template.AllowExpansion)
            { }

        public virtual void Update(){}
    }

    public abstract class OTSMonitorTemplate<T>(string name, Guid libraryGuid, bool allowExpansion) : 
        OTSComponentTemplate<T>(name, libraryGuid, allowExpansion)
        where T : IOTSComponent
    {
    }

    public abstract class OTSMonitor : OTSComponent
    {
        protected OTSMonitor(string name, Guid libraryGuid, bool allowExpansion = false) : base(name, libraryGuid, allowExpansion)
        {
        }

        protected OTSMonitor(IOTSComponentTemplate<IOTSComponent> template) :
            base(template.Name, template.LibraryGuid, template.AllowExpansion) { }

        public override void Update()
        {
            foreach(var view in Views)
            {
                var config = GetConfig(view.Name);
                config?.Set(view.Peak());
            }
        }
    }

    #endregion


    #region Auxilliary 

    //------------------- Field -----------------------//
    public class OTSFieldBase(string name) :
        OTSBase(name), IOTSFieldDefinition
    {
        public OTSTypes OTSType { get; }
        public bool? Visibility { get; }
    }

    public class OTSFieldTemplate(string name) :
        OTSFieldBase(name), 
        IOTSTemplate<IOTSField>,
        IOTSFieldTemplate
    {
        public IOTSField CreateInstance() => new OTSField(this);
    }

    public class OTSField :
        OTSFieldBase,
        IOTSField
    {
        internal IOTSData? Value { get; private set; }
        public void Set(IOTSData? data) { Value = data; }

        public OTSField(string name): base(name) {}
        public OTSField(IOTSFieldTemplate template): base(template.Name) {}
    }

    //------------------- Config Field -----------------------//
    public class OTSConfigFieldBase(string name, OTSTypes type, bool? visiblity=null, EditLock editLock=EditLock.AlwaysLocked) :
        OTSBase(name), IOTSConfigFieldDefinition
    {
        public OTSTypes OTSType { get; protected set; } = type;
        public bool? Visibility { get; protected set; } = visiblity;
        public EditLock EditLock { get; protected set; } = editLock;
    }

    public class OTSConfigFieldTemplate(string name, OTSTypes type, bool? visiblity=null, EditLock editLock=EditLock.AlwaysLocked) :
        OTSConfigFieldBase(name, type, visiblity, editLock), 
        IOTSTemplate<IOTSConfigField>,
        IOTSConfigFieldTemplate
    {
        public IOTSConfigField CreateInstance() => new OTSConfigField(this);
    }

    public class OTSConfigField :
        OTSConfigFieldBase,
        IOTSConfigField
    {
        protected IOTSData? Value { get; set; }
        public virtual void Set(IOTSData? data) { Value = data; }
        public virtual IOTSData? Get() { return Value; }

        public OTSConfigField(string name, OTSTypes type, bool? visiblity=null, EditLock editLock=EditLock.AlwaysLocked): 
            base(name, type, visiblity, editLock) {}
        public OTSConfigField(IOTSConfigFieldTemplate template): 
            base(template.Name, template.OTSType, template.Visibility, template.EditLock) {}
    }

    //------------------- Link -----------------------//

    public class OTSLink(IOTSOutput output, IOTSInput input) :  OTSBase($"{output.ID}::{input.ID}"), IOTSLink
    {
        public IOTSInput Input { get; private set; } = input;
        public IOTSOutput Output { get; private set; } = output;

        public IOTSData? Propogate()
        {
            var cVal = Output.Value;
            Input.Set(cVal);
            return cVal;
        }
    }

    #endregion


    #region Application

    public class OTSApplication(string appName) : 
        OTSBase(appName), IOTSApplication
    {

    }

    #endregion
}

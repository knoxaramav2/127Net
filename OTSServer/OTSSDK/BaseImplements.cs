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

    public class OTSBase(string name, Guid parentId) : IOTSBase
    {
        public Guid ID { get; } = Guid.NewGuid();
        public Guid ParentId { get; set; } = parentId;
        public string Name { get; } = name;
    }

    public abstract class OTSTemplateBase<T>(string name, Guid libraryId) 
        : OTSBase(name, libraryId), IOTSTemplate<T>
        where T : IOTSComponent
    {
        public abstract T CreateInstance();
    }

    public abstract class OTSObjectBase(string name, Guid parentId) : OTSBase(name, parentId), IOTSObject { }

    #endregion


    #region IO Nodes

    public class OTSIOBase(string name, Guid componentId, OTSTypes type) : OTSBase(name, componentId), IOTSIONodeDefinition
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

        public IOTSLink? Link { get; private set; }
        public bool SetLink(IOTSLink link)
        {
            if(Link != null){ return false; }
            Link = link;
            return true;
        }

        public bool RemoveLink()
        {
            if(Link == null){ return false; }
            Link = null;
            return true;
        }
    
        public OTSView(string name, IOTSOutput connectee,
            Guid componentId, OTSTypes initialType = OTSTypes.SIGNED) :
            base(name, connectee, componentId, initialType) { }

        public OTSView(IOTSViewTemplate template, Guid componentId)
            : base(template.Name, template.Output,  componentId, template.OTSType) { }
    }

    //------------------- Input -----------------------//
    public class OTSInputBase(string name, Guid componentId, OTSTypes type) : 
        OTSIOBase(name, componentId, type), IOTSInputDefinition
        { }

    public class OTSInputTemplate(string name, OTSTypes type) :
        OTSInputBase(name, Guid.Empty, type), 
        IOTSTemplate<IOTSInput>,
        IOTSInputTemplate
    {
        public IOTSInput CreateInstance() => new OTSInput(this);
    }

    public class OTSInput(IOTSInputTemplate template) : OTSInputBase(template.Name, Guid.Empty, template.OTSType), IOTSInput
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
    
        public IOTSLink? Link { get; private set; }
        public bool SetLink(IOTSLink link)
        {
            if(Link != null){ return false; }
            Link = link;
            return true;
        }

        public bool RemoveLink()
        {
            if(Link == null){ return false; }
            Link = null;
            return true;
        }

        public IOTSData? Peak() => Value;
    }


    //------------------- Output -----------------------//
    public class OTSOutputBase(string name, Guid componentId, OTSTypes type)
        : OTSIOBase(name, componentId, type), IOTSOutputDefinition
    { }

    public class OTSOutputTemplate(string name, OTSTypes type) :
        OTSOutputBase(name, Guid.Empty, type),
        IOTSTemplate<IOTSOutput>,
        IOTSOutputTemplate
    {
        
        public IOTSOutput CreateInstance() => new OTSOutput(this);
    }

    public class OTSOutput(IOTSOutputTemplate template) : OTSOutputBase(template.Name, Guid.Empty, template.OTSType), IOTSOutput
    {
        public IEnumerable<IOTSLink> Links { get; private set; } = [];
        public bool SetLink(IOTSLink link)
        {
            if(Links.Any(x => x.Input.ID == link.Input.ID && x.Output.ID == link.Output.ID))
            {
                return false;
            }

            ((ICollection<IOTSLink>)Links).Add(link);
            return true;
        }
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
        public Guid LibraryGuid => ParentId;

        #pragma warning disable IDE0028 // Simplify collection initialization
        public IEnumerable<TInput> Inputs { get; protected set; } = [];
        public IEnumerable<TView> Views { get; protected set; } = new List<TView>();
        public IEnumerable<TOutput> Outputs { get; protected set; } = [];
        public IEnumerable<TField> Fields { get; protected set; } = new List<TField>();
        public Lazy<OTSComponentClass> ComponentClass { get; protected set; }
        #pragma warning restore IDE0028 // Simplify collection initialization

        public OTSComponentBase(string name, Guid libraryGuid, 
            IEnumerable<TInput> inputs,
            IEnumerable<TOutput> outputs,
            List<TField> fields,
            bool allowExpansion) : base(name, libraryGuid)
        {
            AllowExpansion = allowExpansion;
            ComponentClass =  new Lazy<OTSComponentClass>(() => CommonUtil.IdentifyComponentClass(this));
            Inputs = inputs;
            Outputs = outputs;
            Fields = fields;
            Views = [];
        }
    }

    public abstract class OTSComponentTemplate<T> :
        OTSComponentBase<IOTSInputTemplate, IOTSViewTemplate, IOTSOutputTemplate, IOTSConfigFieldTemplate>,
        IOTSComponentTemplate<T> where T : IOTSComponent
    {
        public OTSComponentTemplate(string name, Guid libraryGuid,
        IEnumerable<IOTSInputTemplate> inputs,
        IEnumerable<IOTSOutputTemplate> outputs,
        List<IOTSConfigFieldTemplate> fields, 
        bool allowExpansion) : base (name, libraryGuid, inputs, outputs, fields, allowExpansion)
        {
            foreach (var input in inputs) { input.ParentId = ID; }
            foreach (var output in outputs) { output.ParentId = ID; }
            foreach (var field in fields) { field.ParentId = ID; }
        }

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
            var config = new OTSConfigField(view.Name, view.OTSType, ID, true);

            Views = Views.Append(view);
            Fields = Fields.Append(config);

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

        public OTSComponent(IOTSComponentTemplate<IOTSComponent> template) : 
            base(template.Name, template.LibraryGuid, 
                template.Inputs.Select(x => x.CreateInstance()).ToList(),
                template.Outputs.Select(x => x.CreateInstance()).ToList(),
                template.Fields.Select(x => x.CreateInstance()).ToList(),
                template.AllowExpansion)
    { 
        foreach(var input in (List<IOTSInput>)Inputs) { input.ParentId = ID; }
        foreach (var item in (List<IOTSOutput>)Outputs) { item.ParentId = ID; }
        foreach (var field in (List<IOTSConfigField>)Fields) { field.ParentId = ID; }
        }

        public virtual void Update(){}
    }

    #endregion

    #region Monitor

    public class OTSMonitorTemplate<T>(string name, Guid libraryGuid) :
        OTSComponentTemplate<IOTSMonitor>(name, libraryGuid, [], [], [], true),
        IOTSMonitorTemplate where T : IOTSMonitor
    {
        public override IOTSMonitor CreateInstance()
        {
            return new OTSMonitor(this);
        }
    }

    public class OTSMonitor(IOTSMonitorTemplate template) : OTSComponent(template), IOTSMonitor
    {
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

    #region Provider

    public class OTSProviderTemplate<T>(string name, Guid libraryGuid,
        IEnumerable<IOTSOutputTemplate> outputs,
        List<IOTSConfigFieldTemplate> fields) :
        OTSComponentTemplate<IOTSProvider>(name, libraryGuid, [], outputs, fields, false),
        IOTSProviderTemplate where T: IOTSProvider
    {
        public override IOTSProvider CreateInstance()
        {
            return new OTSProvider(this);
        }
    }

    public class OTSProvider(IOTSProviderTemplate template) : 
        OTSComponent(template), IOTSProvider
    {
    }

    #endregion

    #region Auxilliary 

    //------------------- Field -----------------------//
    public class OTSFieldBase(string name, Guid componentId) :
        OTSBase(name, componentId), IOTSFieldDefinition
    {
        public OTSTypes OTSType { get; }
        public bool? Visibility { get; }
    }

    public class OTSFieldTemplate(string name, Guid componentId) :
        OTSFieldBase(name, componentId), 
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

        public OTSField(string name, Guid componentId): base(name, componentId) {}
        public OTSField(IOTSFieldTemplate template): base(template.Name, Guid.Empty) {}
    }

    //------------------- Config Field -----------------------//
    public class OTSConfigFieldBase :
        OTSBase, IOTSConfigFieldDefinition
    {
        public OTSTypes OTSType { get; protected set; }
        public bool? Visibility { get; protected set; }
        public EditLock EditLock { get; protected set; }

        public OTSConfigFieldBase (string name, OTSTypes type, Guid componentId, bool? visiblity=null, EditLock editLock=EditLock.AlwaysLocked)
            : base(name, componentId)
        {
            OTSType = type;
            Visibility = visiblity;
            EditLock = editLock;
        }

        public OTSConfigFieldBase(IOTSConfigFieldTemplate template) : base(template.Name, Guid.Empty)
        { }

    }

    public class OTSConfigFieldTemplate(string name, OTSTypes type, Guid componentId, bool? visiblity=null, EditLock editLock=EditLock.AlwaysLocked) :
        OTSConfigFieldBase(name, type, componentId, visiblity, editLock), 
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

        public OTSConfigField(string name, OTSTypes type, Guid componentId, bool? visiblity=null, EditLock editLock=EditLock.AlwaysLocked): 
            base(name, type, componentId, visiblity, editLock) {}
        public OTSConfigField(IOTSConfigFieldTemplate template): 
            base(template.Name, template.OTSType, Guid.Empty, template.Visibility, template.EditLock) {}
    }

    //------------------- Link -----------------------//

    public class OTSLink(IOTSOutput output, IOTSInput input) :  OTSBase($"{output.ID}::{input.ID}", Guid.Empty), IOTSLink
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
        OTSBase(appName, Guid.Empty), IOTSApplication
    {

    }

    #endregion
}

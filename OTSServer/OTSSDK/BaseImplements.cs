using System;
using System.Collections.Generic;
using System.Linq;
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
        public Guid ComponentId { get; } = componentId;
        public OTSTypes OTSType { get; protected set; } = type;
    }

    //------------------- View -----------------------//
    public class OTSViewBase(string name, Guid componentId, OTSTypes type) 
        : OTSIOBase(name, componentId, type), IOTSViewDefinition
        { }

    public class OTSViewTemplate(string name, Guid componentId, OTSTypes type) :
        OTSViewBase(name, componentId, type), 
        IOTSTemplate<IOTSView>,
        IOTSViewTemplate
    {
        public IOTSView CreateInstance() => new OTSView(this);
    }

    public class OTSView: OTSViewBase, IOTSView
    {
        protected object _value = (long)0;
        public IOTSData Value { get { return new OTSData(OTSType, _value); } }

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
    
        public OTSView(string name, Guid componentId, OTSTypes initialType = OTSTypes.SIGNED) :
            base(name, componentId, initialType) { }

        public OTSView(IOTSViewTemplate template)
            : base(template.Name, template.ComponentId, template.OTSType) { }
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
        protected object _value = (long)0;
        public OTSData Value { get { return new OTSData(OTSType, _value); } }

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

    public abstract class OTSOutput(string name, Guid componentId, OTSTypes type) 
        : OTSOutputBase(name, componentId, type), IOTSOutput
    {
        public abstract IOTSData? Get();
    }

    #endregion


    #region Component

    public class OTSComponentBase<TInput, TView, TOutput, TField>
        (string name, Guid libraryGuid, bool allowExpansion) : 
        OTSBase(name),
        IOTSComponentDefinition<TInput, TView, TOutput, TField>
        where TInput : IOTSInputDefinition
        where TView  : IOTSViewDefinition
        where TOutput : IOTSOutputDefinition
        where TField : IOTSConfigFieldDefinition
    {
        public bool AllowExpansion { get; } = allowExpansion;
        public Guid LibraryGuid { get; } = libraryGuid;

        public IEnumerable<TInput> Inputs { get; protected set; } = [];
        public IEnumerable<TView> Views { get; protected set; } = [];
        public IEnumerable<TOutput> Outputs { get; protected set; } = [];
        public IEnumerable<TField> Fields { get; protected set; } = [];
    }

    public abstract class OTSComponentTemplate<T>(string name, Guid libraryGuid, bool allowExpansion) :
        OTSComponentBase<IOTSInputTemplate, IOTSViewTemplate, IOTSOutputTemplate, IOTSConfigFieldTemplate>
        (name, libraryGuid, allowExpansion),
        IOTSComponentTemplate<T> where T : IOTSComponent
    {
        public abstract T CreateInstance();
    }

    public class OTSComponent :
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

        public bool AddViewingInput(IOTSView newInput)
        {
            if(!AllowExpansion || Views.Any(x => x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))) { return false; }
            ((ICollection<IOTSViewDefinition>)Views).Add(newInput);
            return false;
        }

        public bool RemoveViewingInput(string viewInputName)
        {
            var toRemove = Views.FirstOrDefault(x => viewInputName.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
            if (toRemove != null)
            {
                ((ICollection<IOTSViewDefinition>)Views).Remove(toRemove);
                return true;
            }

            return false;
        }
        
        public OTSComponent(string name, Guid libraryGuid, bool allowExpansion = false) :
            base(name, libraryGuid, allowExpansion)
        { 
            if (allowExpansion) { AddViewingInput(new OTSView("View1", ID, OTSTypes.SIGNED)); } 

        }

        public OTSComponent(IOTSComponentTemplate<IOTSComponent> template) : 
            base(template.Name, template.LibraryGuid, template.AllowExpansion)
            { }
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
        internal IOTSData? Value { get; private set; }
        public virtual void Set(IOTSData? data) { Value = data; }
        public virtual IOTSData? Get() { return Value; }

        public OTSConfigField(string name, OTSTypes type, bool? visiblity=null, EditLock editLock=EditLock.AlwaysLocked): 
            base(name, type, visiblity, editLock) {}
        public OTSConfigField(IOTSConfigFieldTemplate template): 
            base(template.Name, template.OTSType, template.Visibility, template.EditLock) {}
    }

    //------------------- Link -----------------------//

    internal class OTSLink
        (Guid providerComponentId, Guid providerOutputNodeId,
        Guid receiverComponentId, Guid receiverOutputNodeId) : 
        OTSBase($"{providerComponentId}::{providerOutputNodeId}::{receiverComponentId}::{receiverOutputNodeId}"), IOTSLink
    {
        public Guid ProviderComponentId { get; } = providerComponentId;
        public Guid ProviderOutputNodeId { get; } = providerOutputNodeId;

        public Guid ReceiverComponentId { get; } = receiverComponentId;
        public Guid ReceiverOutputNodeId { get; } = receiverOutputNodeId;
    }

    #endregion


    #region Application

    public class OTSApplication(string appName) : 
        OTSBase(appName), IOTSApplication
    {

    }

    #endregion
}

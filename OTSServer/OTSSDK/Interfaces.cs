namespace OTSSDK
{
    #region Core OTS Definitions

    public interface IOTSBase
    {
        Guid ID { get; }
        public string Name { get; }
    }

    public interface IOTSTemplate<T> : IOTSBase
    {
        T CreateInstance();
    }

    public interface IOTSObject : IOTSBase
    {}

    #endregion

    #region Library
    public interface IOTSLibrary
    {
        string Name { get; }
        string Version { get; }
        string Platform { get; }
        string Author { get; }
        string Repository { get; }
        string Description { get; }
        Guid ID { get; }
        IEnumerable<IOTSComponentTemplate<IOTSComponent>> Components { get; }
        public IOTSComponent? GetComponent(string name);
    }
    #endregion

    #region OTS Field

    public interface IOTSFieldDefinition: IOTSBase
    {
        OTSTypes OTSType { get; }
        bool? Visibility { get; }
    }

    public interface IOTSFieldTemplate 
        : IOTSFieldDefinition, IOTSTemplate<IOTSField>
    {}

    public interface IOTSField : IOTSObject, IOTSFieldDefinition
    {
        void Set(IOTSData? data);
    }
    

    public enum EditLock { AlwaysLocked, AlwaysUnlocked, Locked, Unlocked }
    public interface IOTSConfigFieldDefinition : IOTSFieldDefinition
    {
        EditLock EditLock { get; }
    }

    public interface IOTSConfigFieldTemplate 
        : IOTSConfigFieldDefinition, IOTSTemplate<IOTSConfigField>
    {
        
    }

    public interface IOTSConfigField : IOTSField, IOTSConfigFieldDefinition
    {
        IOTSData? Get();
    }

    #endregion

    #region Component

    public enum OTSComponentClass
    {
        PROVIDER,
        ACTUATOR,
        MONITOR,
        SINGLE,
        NOMAD,
    }

    public interface IOTSComponentDefinition<out TInput, out TView, out TOutput, out TField>
        where TInput : IOTSInputDefinition
        where TView  : IOTSViewDefinition
        where TOutput : IOTSOutputDefinition
        where TField : IOTSConfigFieldDefinition
    {
        Guid LibraryGuid { get; }
        bool AllowExpansion { get; }

        IEnumerable<TInput> Inputs { get; }
        IEnumerable<TView> Views { get; }
        IEnumerable<TOutput> Outputs { get; }
        IEnumerable<TField> Fields { get; }
        Lazy<OTSComponentClass> ComponentClass { get; }
    }

    public interface IOTSComponentTemplate<T> : 
        IOTSComponentDefinition<IOTSInputTemplate, IOTSViewTemplate, IOTSOutputTemplate, IOTSConfigFieldTemplate>,
        IOTSTemplate<T> where T: IOTSComponent
    {
    }

    public interface IOTSComponent 
        : IOTSObject, 
        IOTSComponentDefinition<IOTSInput, IOTSView, IOTSOutput, IOTSConfigField>
    {
        IOTSInput? GetInput(string name);
        IOTSOutput? GetOutput(string name);
        IOTSView? GetView(string name);
        IOTSConfigField? GetConfig(string name);
        public IOTSView? AddViewingInput(IOTSOutput providerNode);
        public bool RemoveViewingInput(Guid viewId);
        public void Update();
    }

    #endregion


    #region IO Nodes

    public interface IOTSIONodeDefinition : IOTSObject
    {
        //Guid NodeId { get; }
        Guid ComponentId { get; }
        OTSTypes OTSType { get; }
        IOTSData? Value { get; set; }
    }


    public interface IOTSInputDefinition : IOTSIONodeDefinition { }
    public interface IOTSInputTemplate:
        IOTSTemplate<IOTSInput>,
        IOTSInputDefinition { }

    public interface IOTSInput : 
        IOTSInputDefinition 
    {
        void Set(IOTSData? data);
        IOTSData? Peak();
    }

    public interface IOTSViewDefinition : IOTSInputDefinition 
        { IOTSOutput Output { get; } }
    public interface IOTSViewTemplate:
        IOTSTemplate<IOTSView>,
        IOTSViewDefinition { }

    public interface IOTSView : 
        IOTSViewDefinition, IOTSInput
    {

    }
   
    public interface IOTSOutputDefinition : IOTSIONodeDefinition { }
    public interface IOTSOutputTemplate :
        IOTSTemplate<IOTSOutput>,
        IOTSOutputDefinition { }

    public interface IOTSOutput : 
        IOTSOutputDefinition
    {
    }

    #endregion

    public interface IOTSLink : IOTSObject
    {
        /*
         * Push value from provider to receiver
         * Leak result as output
         */
        public IOTSData? Propogate();
    }

    public interface IOTSApplication : IOTSObject
    {

    }

    public interface IOTSApplicationTemplate : IOTSApplication, IOTSTemplate<IOTSApplication>{ }
}

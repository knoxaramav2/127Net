using OTSCommon.Plugins;
using OTSRunner;
using OTSSDK;

namespace OTSExecution
{
    public interface IOTSExecManager
    {
        public string GetManifest();
        public IOTSLibrary? GetLibrary(string libName);
        public IEnumerable<IOTSLibrary> GetLibraries();
    }

    public class OTSExecManager : IOTSExecManager
    {
        private IObjectStore ObjectStore;
        private IPluginManager PluginManager;
        private IOTSAppManager AppManager;

        public OTSExecManager()
        {
            PluginManager = new PluginManager();
            AppManager = new OTSAppManager();
            ObjectStore = new ObjectStore();
        }

        public string GetManifest()
        {
            var NL = Environment.NewLine;
            string ret = $"<OTSManifest>{NL}";
            
            foreach(var library in GetLibraries())
            {
                ret += $"\t<Library Name={library.Name} PLATFORM={library.Platform} GUID={library.ID}>{NL}";
                foreach(var component in library.Components)
                {
                    ret += $"\t\t<Component Name={component.Name} GUID={component.ID}>{NL}";
                    foreach(var input in component.Inputs) { ret += $"\t\t\t<Input Name={input.Name} GUID={input.ID}/>{NL}"; }
                    foreach(var view in component.Views) { ret += $"\t\t\t<View Name={view.Name} GUID={view.ID}/>{NL}"; }
                    foreach(var output in component.Outputs) { ret += $"\t\t\t<Output Name={output.Name} GUID={output.ID}/>{NL}"; }
                    foreach(var field in component.Fields) { ret += $"\t\t\t<Field Name={field.Name} Type={field.OTSType}/>{NL}"; }
                    ret += $"\t\t</Component>{NL}";
                }
                ret += $"\t</Library>{NL}";
            }

            ret += $"</OTSManifest>{NL}";

            return ret;
        }

        public IOTSLibrary? GetLibrary(string libName)
        {
            return PluginManager.Libraries.FirstOrDefault(x => libName.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<IOTSLibrary> GetLibraries()
        {
            try
            {
                return PluginManager.Libraries;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
            
        }
    }
}

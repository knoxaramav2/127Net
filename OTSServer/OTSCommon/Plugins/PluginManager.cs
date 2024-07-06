using OTSSDK;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OTSCommon.Plugins
{
    public interface IPluginManager
    {
        public IEnumerable<IOTSLibrary> Libraries { get; }

        public IEnumerable<IOTSLibrary> GetAllLibraryVersions(string name, bool allowDisabled = false);
        public IOTSLibrary? GetLibrary(string name);
        public int DiscoverPlugins();
    }

    public class PluginManager : IPluginManager
    {
        struct LibConfig{ public IOTSLibrary Instance { get; set; } public bool Enabled; }

        private readonly string PluginDirectory;
        private Dictionary<Guid, LibConfig> LoadedLibraries;

        public PluginManager(string pluginDirectory="") 
        { 
            PluginDirectory = pluginDirectory == "" ?
                Path.Join(AppDomain.CurrentDomain.BaseDirectory, "plugins") :
                pluginDirectory;
            Directory.CreateDirectory(PluginDirectory);
            Console.WriteLine($"Scanning: {PluginDirectory}");
            PrintDirPaths(PluginDirectory, 0);
            DiscoverPlugins();
        }

        private static void PrintDirPaths(string path, int depth)
        {
            var directoryies = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path);
            var spacing = new string('\t', depth);

            foreach (var file in files)
            {
                Console.WriteLine($"{spacing}|_ ${ Path.GetFileName(file)}");
            }

            foreach (var dir in directoryies)
            {
                Console.WriteLine($"{spacing}|_ #{Path.GetFileName(dir)}");
                PrintDirPaths(dir, depth+1);
            }
        }

        public IEnumerable<IOTSLibrary> Libraries => GetHighestLibInstances();

        public IOTSLibrary? GetLibrary(Guid guid) => LoadedLibraries.TryGetValue(guid, out var library) ? library.Instance : null;
        public IEnumerable<IOTSLibrary> GetAllLibraryVersions(string name, bool allowDisabled = false)
        {
            return LoadedLibraries.Where(x => (x.Value.Enabled || allowDisabled) &&
                name.Equals(x.Value.Instance.Name, StringComparison.OrdinalIgnoreCase)
                ).Select(x => x.Value.Instance)
                 .OrderBy(x => x.Version);
        }

        public IOTSLibrary? GetLibrary(string name) => GetAllLibraryVersions(name).FirstOrDefault();

        private IEnumerable<IOTSLibrary> GetHighestLibInstances()
        {
            var distLibNames = LoadedLibraries.Select(x => x.Value.Instance.Name).Distinct();

            ICollection<IOTSLibrary> ret = [];
            foreach (var lib in distLibNames) {
                var latest = GetLibrary(lib)!;
                ret.Add(latest);
            }

            return ret;
        }

        private ICollection<OTSInfoMessage> ValidatePlugins()
        {
            ICollection<OTSInfoMessage> ret = [];

            var duplicates = LoadedLibraries.Where(x =>
                LoadedLibraries.Count(y => y.Value.Instance.Name.Equals(x.Value.Instance.Name, StringComparison.OrdinalIgnoreCase)) > 1 
            ).OrderBy(x => x.Value.Instance.Name)
             .OrderBy(x => x.Value.Instance.Version)
             .Reverse();

            var distinct = duplicates.Select(x => x.Value.Instance.Name).Distinct();
            Dictionary<string, IEnumerable<string>> versionGroups = [];
            foreach (var item in distinct)
            {
                var versions = duplicates.Where(x => item.Equals(x.Value.Instance.Name, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Value.Instance.Version);
                versionGroups[item] = versions;
            }

            foreach(var conflict in versionGroups)
            {
                var msg = $"Conflicting library versions found: {Environment.NewLine}";
                msg += conflict.Value.Select(x => $"\t\t{x}{Environment.NewLine}");
                msg += "Using latest version, older versions will be disabled";

                ret.Add(new OTSInfoMessage(msg, OTSInfoType.Warning));

                //Disable outdated library versions
                var highVersion = conflict.Value.First();
                var libInstances = 
                LoadedLibraries.Where(x => x.Value.Instance.Name.Equals(conflict.Key, 
                    StringComparison.OrdinalIgnoreCase) && 
                    !x.Value.Instance.Version.Equals(highVersion))
                    .Select(x => x.Value)
                    .Select(x => x.Enabled = false);
            }

            return ret;
        }

        [MemberNotNull(nameof(LoadedLibraries))]
        public int DiscoverPlugins()
        {
            LoadedLibraries = [];
            int pluginsLoaded = 0;

            var libFiles = Directory.GetFiles(PluginDirectory, "*.dll");
            foreach (var file in libFiles) { Assembly.LoadFile(file); }
            
            var allAsms = AppDomain.CurrentDomain.GetAssemblies().ToArray();

            var pluginsAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.Location.Contains(PluginDirectory));

            var componentTypes = pluginsAssemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => (
                typeof(IOTSComponentTemplate<IOTSComponent>).IsAssignableFrom(x)) && x.IsClass)
                .ToArray();

            var libTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => (
                typeof(IOTSLibrary).IsAssignableFrom(x)) && x.IsClass)
                .ToArray();

            foreach (var libType in libTypes) {
                if(Activator.CreateInstance(libType) is IOTSLibrary lib)
                {
                    ++pluginsLoaded;
                    LoadedLibraries.Add(lib.ID, new LibConfig{ Instance=lib, Enabled=true });
                    //Console.WriteLine($"Load Library: {lib.Name} | GUID: {lib.LibraryGuid}");
                    //Console.WriteLine($":: {lib.Description}");
                    //Console.WriteLine("______________________");
                    foreach(var comp in lib.Components)
                    {
                        //Console.WriteLine($"\tComponent: {comp.Name} | GUID: {comp.LibraryGuid}");

                        var inputs = comp.Inputs;
                        var outputs = comp.Outputs;

                        //foreach(var input in inputs){ Console.WriteLine($"\t\tInput: {input.Name} | {input.OTSType}"); }
                        //foreach(var output in outputs){ Console.WriteLine($"\t\tOutput: {output.Name} | {output.OTSType}"); }
                    }
                }
            }

            return pluginsLoaded;
        }
         
    }
}

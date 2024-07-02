using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Constraints;
using OTSCommon.Database;
using OTSCommon.Models;
using OTSCommon.Plugins;
using OTSCommon.Security;
using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSTests
{
    public class OTSSetupException : Exception
    {
        public OTSSetupException() { }
        public OTSSetupException(string message) : base(message) { }
    }

    public class SingleSetupPlugins(SingleSetup setup)
    {
        private SingleSetup Setup { get; set; } = setup;
        public PluginManager PluginManager { get; private set; } = new PluginManager();

        private Dictionary<string, IOTSLibrary> LibraryMacros { get; set; } = [];
        private Dictionary<string, Func<IOTSComponent?>> ComponentMacros { get; set; } = [];

        public SingleSetupPlugins EnsureLibrary(string libraryName, string? libraryAlias=null)
        {
            libraryAlias ??= libraryName;

            libraryAlias = libraryAlias.ToUpper();
            if(LibraryMacros.ContainsKey(libraryAlias)) 
                { throw new OTSSetupException($"Cannot register duplicate library {libraryName} with alias {libraryAlias}."); }

            var library = PluginManager.GetLibrary(libraryName) ?? throw new OTSSetupException($"Library {libraryName} not found.");
            LibraryMacros[libraryAlias] = library;
            
            return this;
        }

        public SingleSetupPlugins EnsureComponent(string libraryAlias, string componentName, string? componentAlias = null)
        {
            componentAlias ??= componentName;
            libraryAlias = libraryAlias.ToUpper();
            componentAlias = componentAlias.ToUpper();
            
            if(ComponentMacros.ContainsKey(componentAlias))
                { throw new OTSSetupException($"Cannot register duplicate component {componentName} with alias {componentAlias}."); }
            GetLibrary(libraryAlias, out var library);

            ComponentMacros[componentAlias] = () => library?.GetComponent(componentName) 
                ?? throw new OTSSetupException($"Unabled to create factory for {componentAlias} ({componentName}).");
            
            return this;
        }

        public SingleSetup EndPlugins => Setup;

        public bool GetLibrary(string alias, out IOTSLibrary? library) => LibraryMacros.TryGetValue(alias.ToUpper(), out library);
        public bool GetComponent(string alias, out IOTSComponent? component)
        {
            ComponentMacros.TryGetValue(alias.ToUpper(), out var componentFunc);
            component = componentFunc?.Invoke();

            return component != null;
        }
    }

    public class SingleSetup: IDisposable
    {
        private static SingleSetup? Instance { get; set; }

        public OTSDbCtx OTSDbCtx { get; private set; }
        public OTSService OTSService { get; private set; }
        public SingleSetupPlugins? PluginSetups { get; set; }
        
        private SingleSetup() 
        { 
            var config = new ConfigurationBuilder()
                .AddUserSecrets<UserTests>()
                .Build();    
            var dbOptions = new DbContextOptionsBuilder<OTSDbCtx>()
                .UseInMemoryDatabase(databaseName: "OTS_IM")
                .Options;
            OTSDbCtx = new OTSDbCtx(dbOptions, config);
            OTSDbCtx.Database.EnsureDeleted();
            OTSService = new OTSService(OTSDbCtx);
        }

        public SingleSetup EnsureUserAccount(string username, string password, bool admin=false)
        {
            var maxAuthStr = admin ? "Admin" : "Owner";
            var maxAuth = OTSService.GetRoleAuthority(maxAuthStr)!;
            var opAuth = maxAuth.Downgrade ?? maxAuth;

            OTSService.AddUser(username, password, maxAuth, opAuth);

            return this;
        }

        public SingleSetup EnsureDevice(string username, IDeviceInfo info)
        {
            var user = OTSService.GetUser(username)!;

            OTSService.AddDevice(user.Id, info);

            return this;
        }

        public SingleSetupPlugins EnsurePlugins()
        {
            PluginSetups = new SingleSetupPlugins(this);
            return PluginSetups;
        }

        public static SingleSetup GetInstance()
        {
            Instance ??= new SingleSetup();
            return Instance;
        }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
            //GC.SuppressFinalize(this);
            OTSDbCtx.Dispose();
            Instance = null;
        }    
    }
}

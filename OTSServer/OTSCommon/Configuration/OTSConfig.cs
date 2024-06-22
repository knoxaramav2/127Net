using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Configuration
{
    public static class OTSConfig
    {
        private const uint DefaultReadLevel = 100;
        private const uint DefaultWriteLevel = 50;

        private static readonly string cfgDir = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "config");
        private static readonly string cfgFile = Path.Combine(cfgDir, "global.cfg");

        public static uint AuthReadLevel { get; set; } = DefaultReadLevel;
        public static uint AuthWriteLevel { get; set; } = DefaultWriteLevel;


        public static void LoadFromDisk()
        {
            if(!File.Exists(cfgFile)) { return; }
            var lines = File.ReadAllLines(cfgFile);

            foreach(var line in lines)
            {
                var trms = line.Split(":", StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);
                if(trms.Length != 2){ continue; }

                string key = trms[0].ToUpper(), val = trms[1];

                switch (key) { 
                    case "RD_AUTH": AuthReadLevel = uint.Parse(val); break;
                    case "WR_AUTH": AuthWriteLevel = uint.Parse(val);break;
                }
            }
        }

        public static void SaveToDisk()
        {
            var contents =
$@"
RD_AUTH:{AuthReadLevel}
WR_AUTH:{AuthWriteLevel}
";

            File.WriteAllText(cfgFile, contents);
        }

    }
}

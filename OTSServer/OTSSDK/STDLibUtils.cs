using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSSDK
{
    public static class StdLibUtils
    {
        //Providers
        public static string ProvidersLibName => "OTSProvider";
        public static string ProvidersConstSigned => "ConstSignedProvider";
        public static string ProvidersConstUnsigned => "ConstUnsignedProvider";
        public static string ProvidersConstDecimal => "ConstDecimalProvider";
        public static string ProvidersConstBool => "ConstBoolProvider";
        public static string ProvidersConstString => "ConstStringProvider";
        public static string ProvidersConstMap => "ConstMapProvider";
        public static string ProvidersConstList => "ConstListProvider";
        public static string ProvidersRandomSigned => "RandomSignedProvider";
        public static string ProvidersRandomDecimal => "RandomDecimalProvider";

        //Device Info
        public static string DeviceInfoLibName => "OTSDeviceHwInfo";
        public static string MemoryMonitor => "MemoryMonitor";
        public static string ProcessMemory => "ProcessMemory";
        public static string SystemMemoryAvailable => "SystemMemoryAvailable";
        public static string SystemMemoryTotal => "SystemMemoryTotal";

        //Math
        public static string MathLibName => "OTSMath";
        public static string MathAddition => "MathAddition";
        public static string MathSubtraction => "MathSubtraction";
        public static string MathMultiplication => "MathMultiplication";
        public static string MathDivision => "MathDivision";

        //Monitor
        public static string MonitorLibName => "OTSMonitor";
        public static string RawMonitor => "RawMonitor";

        //Logic
        public static string LogicLibName => "OTSLogic";
        public static string LogicalEqual => "LogicalEqual";
        public static string LogicalNotEqual => "LogicalNotEqual";
        public static string LogicalLess => "LogicalLess";
        public static string LogicalLessEqual => "LogicalLessEqual";
        public static string LogicalGreater => "LogicalGreater";
        public static string LogicalGreaterEqual => "LogicalGreaterEqual";
        public static string LogicalAnd => "LogicalAnd";
        public static string LogicalOr => "LogicalOr";
        public static string LogicalXor => "LogicalXor";


    }
}

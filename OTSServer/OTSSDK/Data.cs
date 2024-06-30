using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OTSSDK
{
    //TODO Provide concrete type getters/setters
    public interface IOTSData
    {
        OTSTypes OTSType { get; }
        T? As<T>();
        void Set<T>(T? value);

        Tuple<OTSTypes, object> TypeValuePair { get; }
    }

    public struct OTSData(OTSTypes type, object? value = null) : IOTSData
    {
        private object _value = value == null ? TypeConversion.GetOTSTypeDefault(type) : 
            type switch
        {
            OTSTypes.NONE => new object(),
            OTSTypes.SIGNED => (long)TypeConversion.TryCastSignedNumeric(value!),
            OTSTypes.UNSIGNED => (ulong)TypeConversion.TryCastUnsignedNumeric(value!),
            OTSTypes.BOOL => (bool)TypeConversion.TryCastBool(value!),
            OTSTypes.STRING => (string)TypeConversion.TryCastString(value!),
            OTSTypes.DECIMAL => (double)TypeConversion.TryCastDouble(value!),
            OTSTypes.LIST => TypeConversion.TryCastArray(value!),
            OTSTypes.MAP => TypeConversion.TryCastMap(value!),
            _ => throw new OTSException($"Invalid OTS Data type {type}")
        };
        public OTSTypes OTSType { get; } = type;

        public readonly T? As<T>()
        {
            return TypeConversion.TemplateConversion<T>(_value);
        }

        public void Set<T>(T? value)
        {
            var targetType = OTSTypesUtil.MapValueToOTSType(value);
            _value = TypeConversion.PolyConversion(value, targetType);
        }

        public readonly Tuple<OTSTypes, object> TypeValuePair => new(OTSType, _value);

        #region Operator Overloading

        public static bool operator ==(OTSData lVal, OTSData rVal) => lVal.OTSType switch
        {
            OTSTypes.UNSIGNED => lVal.As<ulong>() == rVal.As<ulong>(),
            OTSTypes.SIGNED => lVal.As<long>() == rVal.As<long>(),
            OTSTypes.BOOL => lVal.As<bool>() == rVal.As<bool>(),
            OTSTypes.STRING => lVal.As<string>() == rVal.As<string>(),
            OTSTypes.DECIMAL => lVal.As<double>() == rVal.As<double>(),
            OTSTypes.LIST => lVal.As<List<object>>()?
                .All(x => rVal.As<List<object>>()?.All(y => x == y)??false)??false,
            OTSTypes.MAP => //	¯\_(ツ)_/¯
                (lVal.As<Dictionary<string, object>>()?.Count ?? 0) == 
                (rVal.As<Dictionary<string, object>>()?.Count ?? 0) &&
                (lVal.As<Dictionary<string, object>>()?
                .All(x => rVal.As<Dictionary<string, object>>()?.TryGetValue(x.Key, out var res) 
                    ?? false && res == x.Value) 
                        ?? false),
            _ => false
        };

        public static bool operator !=(OTSData lVal, OTSData rVal)
        {
            return !(lVal == rVal);
        }

        public static bool operator <(OTSData lVal, OTSData rVal) => lVal.OTSType switch
        {
            OTSTypes.UNSIGNED => lVal.As<ulong>() < rVal.As<ulong>(),
            OTSTypes.SIGNED => lVal.As<long>() < rVal.As<long>(),
            OTSTypes.BOOL => lVal.As<long>() < rVal.As<long>(),
            OTSTypes.STRING => lVal.As<string>()?.Length < rVal.As<string>()?.Length,
            OTSTypes.DECIMAL => lVal.As<double>() < rVal.As<double>(),
            OTSTypes.LIST => lVal.As<List<object>>()?.Count < rVal.As<List<object>>()?.Count,
            OTSTypes.MAP => lVal.As<Dictionary<string, object>>()?.Count < rVal.As<Dictionary<string, object>>()?.Count,
            _ => false
        };

        public static bool operator >(OTSData lVal, OTSData rVal) => lVal.OTSType switch
        {

            OTSTypes.UNSIGNED => lVal.As<ulong>() > rVal.As<ulong>(),
            OTSTypes.SIGNED => lVal.As<long>() > rVal.As<long>(),
            OTSTypes.BOOL => lVal.As<long>() > rVal.As<long>(),
            OTSTypes.STRING => lVal.As<string>()?.Length > rVal.As<string>()?.Length,
            OTSTypes.DECIMAL => lVal.As<double>() > rVal.As<double>(),
            OTSTypes.LIST => lVal.As<List<object>>()?.Count > rVal.As<List<object>>()?.Count,
            OTSTypes.MAP => lVal.As<Dictionary<string, object>>()?.Count > rVal.As<Dictionary<string, object>>()?.Count,
            _ => false
        };

        public static bool operator <=(OTSData lVal, OTSData rVal) => lVal.OTSType switch
        {
            OTSTypes.UNSIGNED => lVal.As<ulong>() <= rVal.As<ulong>(),
            OTSTypes.SIGNED => lVal.As<long>() <= rVal.As<long>(),
            OTSTypes.BOOL => lVal.As<long>() <= rVal.As<long>(),
            OTSTypes.STRING => lVal.As<string>()?.Length <= rVal.As<string>()?.Length,
            OTSTypes.DECIMAL => lVal.As<double>() <= rVal.As<double>(),
            OTSTypes.LIST => lVal.As<List<object>>()?.Count <= rVal.As<List<object>>()?.Count,
            OTSTypes.MAP => lVal.As<Dictionary<string, object>>()?.Count <= rVal.As<Dictionary<string, object>>()?.Count,
            _ => false
        };

        public static bool operator >=(OTSData lVal, OTSData rVal) => lVal.OTSType switch
        {

            OTSTypes.UNSIGNED => lVal.As<ulong>() >= rVal.As<ulong>(),
            OTSTypes.SIGNED => lVal.As<long>() >= rVal.As<long>(),
            OTSTypes.BOOL => lVal.As<long>() >= rVal.As<long>(),
            OTSTypes.STRING => lVal.As<string>()?.Length >= rVal.As<string>()?.Length,
            OTSTypes.DECIMAL => lVal.As<double>() >= rVal.As<double>(),
            OTSTypes.LIST => lVal.As<List<object>>()?.Count >= rVal.As<List<object>>()?.Count,
            OTSTypes.MAP => lVal.As<Dictionary<string, object>>()?.Count >= rVal.As<Dictionary<string, object>>()?.Count,
            _ => false
        };

        public override readonly bool Equals(object? obj)
        {
            if(obj is OTSData rData) { return this == rData; }
            return false;
        }

        private int DataHash { get; } = (new Random()).Next();
        public override readonly int GetHashCode()
        {
            return DataHash;
        }

        #endregion
    }

    public static class TypeConversion
    {
        public static bool CastIfSignedNumeric(object? value, out long res)
        {
            if(value is long ||value is int || value is short)
            {
                res = (long)value; 
                return true;
            }

            res = 0;
            return false;
        }

        public static bool CastIfUnsignedNumeric(object? value, out ulong res)
        {
            if(value is ulong ||value is uint || value is ushort || value is byte)
            {
                res = (ulong)value; 
                return true;
            }

            res = 0;
            return false;
        }

        public static bool CastIfDouble(object? value, out double res)
        {
            if(value is float || value is double)
            {
                res = (double) value;
                return true;
            }

            res = 0;
            return false;
        }

        public static bool CastIfString(object? value, out string res)
        {
            if(value is string v)
            {
                res = v;
                return true;
            }

            res = string.Empty;
            return true;
        }

        public static bool CastIfBool(object? value, out bool res)
        {
            if(value is bool v)
            {
                res = v;
                return true;
            }

            res = false;
            return false; 
        }
    
        
        public static long TryCastSignedNumeric(object? value) =>
            value switch
            {
                ulong u64 => (long)u64,
                uint u32 => (long)u32,
                ushort u16 => (long)u16,
                byte bt => (long)bt,

                long i64 => (long)i64,
                int i32 => (long)i32,
                short i16 => (long)i16,

                float f32 => (long)f32,
                double f64 => (long)f64,
                decimal d64 => (long)d64,

                bool bl => (long)(bl ? 1 : 0),

                string str => long.TryParse(str, out long strRes) ? strRes : 0,

                _ => (long)0
            };

        public static ulong TryCastUnsignedNumeric(object? value) =>
            value switch
            {
                ulong u64 => checked((ulong)u64),
                uint u32 => checked((ulong)u32),
                ushort u16 => checked((ulong)u16),
                byte bt => checked((ulong)bt),

                long i64 => checked((ulong)i64),
                int i32 => checked((ulong)i32),
                short i16 => checked((ulong)i16),

                float f32 => checked((ulong)f32),
                double f64 => checked((ulong)f64),
                decimal d64 => checked((ulong)d64),

                bool bl => (ulong)(bl ? 1 : 0),

                string str => ulong.TryParse(str, out ulong strRes) ? strRes : 0,

                _ => (ulong)0
            };

        public static double TryCastDouble(object? value) =>
            value switch
            {
                ulong u64 => (double)u64,
                uint u32 => (double)u32,
                ushort u16 => (double)u16,
                byte bt => (double)bt,

                long i64 => (double)i64,
                int i32 => (double)i32,
                short i16 => (double)i16,

                float f32 => (double)f32,
                double f64 => (double)f64,
                decimal d64 => (double)d64,

                bool bl => (double)(bl ? 1 : 0),

                string str => double.TryParse(str, out double strRes) ? strRes : 0,

                _ => (double)0
            };

        public static string TryCastString(object? value) 
        {
            return value?.ToString() ?? string.Empty;
        }

        public static bool TryCastBool(object? value) =>
            value switch
            {
                ulong u64 => u64 != 0,
                uint u32 => u32 != 0,
                ushort u16 => u16 != 0,
                byte bt => bt != 0,

                long i64 => i64 != 0,
                int i32 => i32 != 0,
                short i16 => i16 != 0,

                float f32 => f32 != 0,
                double f64 => f64 != 0,
                decimal d64 => d64 != 0,

                bool bl => bl,

                string str => bool.TryParse(str, out bool strRes) && strRes,

                _ => false
            };

        public static List<object> TryCastArray(object? value)
        {
            if(value is List<object> list) { return list; }
            else if (value is Dictionary<string, object> dict){
                return [dict.Values];
            }

            return value == null ? 
                [] : [value];
        }

        public static Dictionary<string, object> TryCastMap(object? value)
        {
            if(value is Dictionary<string, object> dictionary) { return dictionary; }
            else if (value is List<string> list)
            {
                var dictRet = new Dictionary<string, object>();
                var counter = 0;
                foreach(var item in list){
                    dictRet[$"Item{counter+1}"] = list[counter];
                    counter++;
                }
            }

            var dict = new Dictionary<string, object>();
            if(value != null) { dict["Item"] = value; }

            return dict;
        }
    
        public static object PolyConversion(object? value, OTSTypes targetType) => 
            targetType switch
            {
                OTSTypes.SIGNED => TryCastSignedNumeric(value),
                OTSTypes.UNSIGNED => TryCastUnsignedNumeric(value),
                OTSTypes.BOOL => TryCastBool(value),
                OTSTypes.STRING => TryCastString(value),
                OTSTypes.DECIMAL => TryCastDouble(value),
                OTSTypes.LIST => TryCastArray(value),
                OTSTypes.MAP => TryCastMap(value),
                _ => new object()
            };
    
        private static T? GetDef<T>(object? value)
        {
            if (value is List<object> list) { return (T)(object) list; }
            else if (value is Dictionary<string, object> dictionary) { return (T)(object) dictionary; }
            return default;
        }

        public static T? TemplateConversion<T>(object? value)
        {
            var retVal = 
                Type.GetTypeCode(typeof(T)) switch
                {
                    TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 or TypeCode.Char or TypeCode.SByte =>
                        (T)(object)TryCastSignedNumeric(value),
                    TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 or TypeCode.Byte =>
                        (T)(object)TryCastUnsignedNumeric(value),
                    TypeCode.Boolean => (T)(object)TryCastBool(value),
                    TypeCode.Decimal or TypeCode.Double => (T)(object)TryCastDouble(value),
                    TypeCode.String => (T)(object)TryCastString(value),
                    _ => GetDef<T>(value)
                };

            //TODO Check list, dict

            return retVal;
        }

        
        public static object GetOTSTypeDefault(OTSTypes type)
        {
            return type switch
            {
                OTSTypes.SIGNED => (long)0,
                OTSTypes.UNSIGNED => (ulong)0,
                OTSTypes.DECIMAL => (double)0.0f,
                OTSTypes.BOOL => false,
                OTSTypes.STRING => string.Empty,
                OTSTypes.LIST => new List<object>(),
                OTSTypes.MAP => new Dictionary<string, object>(),
                _ => new object(),
            };
        }
    }
}

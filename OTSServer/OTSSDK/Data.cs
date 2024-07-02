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
    public interface IOTSData: ICloneable
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

        public readonly object Clone()
        {
            return new
            {
                OTSType,
                value
            };
        }

        #region Operator Overloading

        // Equality
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

        public static bool operator ^(OTSData lVal, OTSData rVal) => lVal.OTSType switch
        {

            OTSTypes.UNSIGNED or
            OTSTypes.SIGNED or
            OTSTypes.BOOL or
            OTSTypes.DECIMAL => lVal.As<bool>() ^ rVal.As<bool>(),
            OTSTypes.STRING => 
                !lVal.As<string>()?.Any(x => rVal.As<string>()?.Contains(x) ?? true) ?? true,
            OTSTypes.LIST => 
                !lVal.As<List<object>>()?.Any(x => rVal.As<List<object>>()?.Contains(x) ?? true) ?? true,
            OTSTypes.MAP => 
                !lVal.As<Dictionary<string, object>>()?.Any(x => rVal.As<Dictionary<string, object>>()?.ContainsKey(x.Key) ?? true) ?? true,
            _ => false
        };

        // Math
        public static OTSData operator +(OTSData lVal, OTSData rVal) => AutoTypeOpts.EnsureAdd(lVal, rVal);

        public static OTSData operator -(OTSData lVal, OTSData rVal) => AutoTypeOpts.EnsureSub(lVal, rVal);

        public static OTSData operator *(OTSData lVal, OTSData rVal) => 
            new( lVal.OTSType,
            lVal.OTSType switch
        {
            OTSTypes.UNSIGNED => lVal.As<ulong>() * rVal.As<ulong>(),
            OTSTypes.SIGNED => lVal.As<long>() * rVal.As<long>(),
            OTSTypes.BOOL => lVal.As<long>() * rVal.As<long>(),
            OTSTypes.STRING => new OTSData(OTSTypes.STRING, ""),
            OTSTypes.DECIMAL => lVal.As<double>() * rVal.As<double>(),
            OTSTypes.LIST => new OTSData(OTSTypes.LIST, new List<object>()),
            OTSTypes.MAP => new OTSData(OTSTypes.MAP, new Dictionary<string, object>()),
            _ => false
        });

        public static OTSData operator /(OTSData lVal, OTSData rVal) => 
            new( lVal.OTSType,
            lVal.OTSType switch
        {
            OTSTypes.UNSIGNED => lVal.As<ulong>() / rVal.As<ulong>(),
            OTSTypes.SIGNED => lVal.As<long>() / rVal.As<long>(),
            OTSTypes.BOOL => lVal.As<long>() / rVal.As<long>(),
            OTSTypes.STRING => new OTSData(OTSTypes.STRING, ""),
            OTSTypes.DECIMAL => lVal.As<double>() * rVal.As<double>(),
            OTSTypes.LIST => new OTSData(OTSTypes.LIST, new List<object>()),
            OTSTypes.MAP => new OTSData(OTSTypes.MAP, new Dictionary<string, object>()),
            _ => false
        });

        //&& and ||
        public static bool operator true(OTSData value) => value.OTSType switch
        {

            OTSTypes.UNSIGNED or
            OTSTypes.SIGNED or
            OTSTypes.BOOL or
            OTSTypes.DECIMAL => value.As<bool>(),
            OTSTypes.STRING => value.As<string>() != string.Empty,
            OTSTypes.LIST => value.As<List<object>>()?.Count > 0,
            OTSTypes.MAP => value.As<Dictionary<string, object>>()?.Count > 0,
            _ => false
        };

        public static bool operator false(OTSData value) => value.OTSType switch
        {

            OTSTypes.UNSIGNED or
            OTSTypes.SIGNED or
            OTSTypes.BOOL or
            OTSTypes.DECIMAL => !value.As<bool>(),
            OTSTypes.STRING => value.As<string>() == string.Empty,
            OTSTypes.LIST => value.As<List<object>>()?.Count == 0,
            OTSTypes.MAP => value.As<Dictionary<string, object>>()?.Count == 0,
            _ => false
        };

        public static OTSData operator &(OTSData lVal, OTSData rVal)
        {
            if(lVal.OTSType == OTSTypes.UNSIGNED)
                { return new OTSData(OTSTypes.UNSIGNED, lVal.As<ulong>() & rVal.As<ulong>()); }
            if(lVal.OTSType == OTSTypes.SIGNED)
                { return new OTSData(OTSTypes.SIGNED, lVal.As<long>() & rVal.As<long>()); }
            if(lVal.OTSType == OTSTypes.BOOL)
                { return new OTSData(OTSTypes.BOOL, lVal.As<bool>() & rVal.As<bool>()); }
            if(lVal.OTSType == OTSTypes.MAP)
            {
                var lMap = lVal.As<Dictionary<string, object>>();
                var rMap = rVal.As<Dictionary<string, object>>(); 
                var newMap = new Dictionary<string, object>();

                if(lMap == null || rMap == null) { return new OTSData(OTSTypes.MAP, newMap); }

                foreach(var key in lMap.Keys)
                {
                    if (rMap.TryGetValue(key, out object? value))
                    {
                        newMap[key] = value;
                    }
                }

                return new OTSData(OTSTypes.MAP, newMap);
            }
            if(lVal.OTSType == OTSTypes.LIST)
            {
                var lList = lVal.As<List<object>>();
                var rList = rVal.As<List<object>>(); 
                var newList = new List<object>();

                if(lList == null || rList == null) { return new OTSData(OTSTypes.LIST, newList); }

                newList = lList.Where(x => rList.Contains(x)).ToList();

                return new OTSData(OTSTypes.LIST, newList);
            }

            
            return new OTSData(OTSTypes.NONE, null);
        }

        public static OTSData operator |(OTSData lVal, OTSData rVal)
        {
            if(lVal.OTSType == OTSTypes.UNSIGNED)
                { return new OTSData(OTSTypes.UNSIGNED, lVal.As<ulong>() | rVal.As<ulong>()); }
            if(lVal.OTSType == OTSTypes.SIGNED)
                { return new OTSData(OTSTypes.SIGNED, lVal.As<long>() | rVal.As<long>()); }
            if(lVal.OTSType == OTSTypes.BOOL)
                { return new OTSData(OTSTypes.BOOL, lVal.As<bool>() | rVal.As<bool>()); }
            if(lVal.OTSType == OTSTypes.MAP)
            {
                var lMap = lVal.As<Dictionary<string, object>>();
                var rMap = rVal.As<Dictionary<string, object>>(); 
                var newMap = new Dictionary<string, object>();

                if(lMap == null || rMap == null) { return new OTSData(OTSTypes.MAP, newMap); }

                foreach(var key in rMap.Keys)
                {
                    newMap[key] = rMap[key];
                }

                return new OTSData(OTSTypes.MAP, newMap);
            }
            if(lVal.OTSType == OTSTypes.LIST)
            {
                var lList = lVal.As<List<object>>();
                var rList = rVal.As<List<object>>(); 
                var newList = lList;

                if(lList == null || rList == null) { return new OTSData(OTSTypes.LIST, lList); }

                newList!.AddRange(rList);
                newList = newList.Distinct().ToList();

                return new OTSData(OTSTypes.LIST, newList);
            }

            
            return new OTSData(OTSTypes.NONE, null);
        }

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

    //TODO Refactor into more specific utility classes
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
    
        public static T? TemplateConversion<T>(object? value)
        {
            if(value is T t)
            {
                return t;
            }

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
                    _ => GetOTSTypeDefault(TypeFromGeneric<T>())
                };

            //TODO Check list, dict

            return (T)retVal;
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
    
        public static IOTSData EnsureValue(IOTSData? value, OTSTypes type)
        {
            if(value != null && value.OTSType == type) { return value; }
            if(value != null) { return new OTSData(type, PolyConversion(value.TypeValuePair.Item2, type)); }
            return new OTSData(type, GetOTSTypeDefault(type));
        }
   
        public static OTSTypes TypeFromGeneric<T>()
        {
            OTSTypes ret = OTSTypes.NONE;
            var typeT = typeof(T);

            if(typeT == typeof(int) || 
               typeT == typeof(long) || 
               typeT == typeof(short))
            {
                ret = OTSTypes.SIGNED;
            } else if(typeT == typeof(uint) || 
               typeT == typeof(ulong) || 
               typeT == typeof(ushort))
            {
                ret = OTSTypes.UNSIGNED;
            } else if(typeT == typeof(float) || 
               typeT == typeof(double))
            {
                ret = OTSTypes.DECIMAL;
            } else if(typeT == typeof(bool))
            {
                ret = OTSTypes.BOOL;
            } else if(typeT == typeof(string))
            {
                ret = OTSTypes.STRING;
            }else if(typeT == typeof(Dictionary<string, object>))
            {
                ret = OTSTypes.MAP;
            } else if(typeT == typeof(List<object>))
            {
                ret = OTSTypes.LIST;
            }

            return ret;
        }

    }

    //TODO Overhaul typing system to be more steamlined
    public static class AutoTypeOpts
    {
        public static OTSTypes NormalizeTypeAndPriorty (IOTSData? originalLVal, IOTSData? originalRVal, out IOTSData LValue, out IOTSData RValue)
        {
            var lvPriority = originalLVal?.OTSType ?? OTSTypes.NONE;
            var rvPriority = originalRVal?.OTSType ?? OTSTypes.NONE;
            var priorityType = (OTSTypes)Math.Max((int)lvPriority, (int)rvPriority);

            originalLVal = TypeConversion.EnsureValue(originalLVal, (OTSTypes)priorityType);
            originalRVal = TypeConversion.EnsureValue(originalRVal, (OTSTypes)priorityType);

            if (lvPriority == rvPriority) { LValue = originalLVal; RValue = originalRVal; }
            else if (lvPriority < priorityType)
            {
                LValue = new OTSData((OTSTypes)priorityType,
                TypeConversion.PolyConversion(originalLVal.TypeValuePair.Item2, (OTSTypes)priorityType));
                RValue = originalRVal;
            }
            else
            {
                RValue = new OTSData((OTSTypes)priorityType,
                TypeConversion.PolyConversion(originalRVal.TypeValuePair.Item2, (OTSTypes)priorityType));
                LValue = originalLVal;
            }

            return priorityType;
        }
    
        public static OTSData EnsureAdd(IOTSData? lVal, IOTSData? rVal)
        {
            var pType = NormalizeTypeAndPriorty(lVal, rVal, out var eLVal, out var eRVal);
            object? sum;

            try
            {
                sum = pType switch
            {
                OTSTypes.NONE => TypeConversion.GetOTSTypeDefault(pType),
                OTSTypes.BOOL => eLVal.As<bool>() && eRVal.As<bool>(),
                OTSTypes.UNSIGNED => checked(eLVal.As<ulong>() + eRVal.As<ulong>()),
                OTSTypes.SIGNED => checked(eLVal.As<long>() + eRVal.As<long>()),
                OTSTypes.DECIMAL => checked(eLVal.As<double>() + eRVal.As<double>()),
                OTSTypes.STRING => checked(eLVal.As<string>() + eRVal.As<string>()),
                OTSTypes.LIST => ExtendLvRvList(eLVal.As<List<object>>(), eRVal.As<List<object>>()),
                OTSTypes.MAP => ExtendLvRvMap(eLVal.As<Dictionary<string, object>>(), eRVal.As<Dictionary<string, object>>()),
                _ => throw new OTSException($"Illegal type {pType}"),
            };
            }
            catch (Exception)
            {
                sum = TypeConversion.GetOTSTypeDefault(pType);
            }

            return new OTSData(pType, sum);
        }

        public static string Substring(string valStr, string subStr)
        {
            var idx = valStr.IndexOf(subStr);
            return idx < 0? valStr : valStr.Remove(idx, subStr.Length);
        }
        public static OTSData EnsureSub(IOTSData? lVal, IOTSData? rVal)
        {
            var pType = NormalizeTypeAndPriorty(lVal, rVal, out var eLVal, out var eRVal);
            object? diff;

            try
            {
                diff = pType switch
                {
                    OTSTypes.BOOL => checked(eLVal.As<bool>() && !eRVal.As<bool>()),
                    OTSTypes.UNSIGNED => checked(eLVal.As<ulong>() - eRVal.As<ulong>()),
                    OTSTypes.SIGNED => checked(eLVal.As<long>() - eRVal.As<long>()),
                    OTSTypes.DECIMAL => checked(eLVal.As<double>() - eRVal.As<double>()),
                    OTSTypes.STRING => Substring(eLVal.As<string>()!, eRVal.As<string>()!),
                    OTSTypes.LIST => eLVal.As<List<object>>()!.Where(x => !eRVal.As<List<object>>()!.Any(y => x == y)).ToList(),
                    OTSTypes.MAP => eLVal.As<Dictionary<string, object>>()!.Where(x => eRVal.As<Dictionary<string, object>>()!.ContainsKey(x.Key)).ToDictionary(),
                    _ => TypeConversion.GetOTSTypeDefault(pType),
                };
            }
            catch (Exception)
            {
                diff = TypeConversion.GetOTSTypeDefault(pType);
            }

            return new OTSData(pType, diff);
        }

        public static OTSData EnsureMultiply(IOTSData? lVal, IOTSData? rVal)
        {
            var pType = NormalizeTypeAndPriorty(lVal, rVal, out var eLVal, out var eRVal);
            object? prod;

            try
            {
                prod = pType switch
                {
                    OTSTypes.BOOL => eLVal.As<bool>() && eRVal.As<bool>(),
                    OTSTypes.UNSIGNED => checked(eLVal.As<ulong>() * eRVal.As<ulong>()),
                    OTSTypes.SIGNED => checked(eLVal.As<long>() * eRVal.As<long>()),
                    OTSTypes.DECIMAL => checked(eLVal.As<double>() * eRVal.As<double>()),
                    _ => TypeConversion.GetOTSTypeDefault(pType),
                };
            }
            catch (Exception)
            {
                prod = TypeConversion.GetOTSTypeDefault(pType);
            }

            return new OTSData(pType, prod);
        }
    
        public static OTSData EnsureDivide(IOTSData? lVal, IOTSData? rVal)
        {
            var pType = NormalizeTypeAndPriorty(lVal, rVal, out var eLVal, out var eRVal);
            var quot = pType switch
            {
                OTSTypes.UNSIGNED => checked(eLVal.As<ulong>() / eRVal.As<ulong>()),
                OTSTypes.SIGNED => checked(eLVal.As<long>() / eRVal.As<long>()),
                OTSTypes.DECIMAL => checked(eLVal.As<double>() / eRVal.As<double>()),
                _ => TypeConversion.GetOTSTypeDefault(pType),
            };

            return new OTSData(pType, quot);
        }

        private static List<object> ExtendLvRvList(List<object>? lVal, List<object>? rVal)
        {
            lVal ??= [];
            rVal ??= [];
            List<object> ret = [];
            ret.AddRange(lVal);
            ret.AddRange(rVal);
            return ret;
        }
        private static Dictionary<string, object> ExtendLvRvMap(Dictionary<string, object>? lVal,Dictionary<string, object>? rVal)
        {
            lVal ??= [];
            rVal ??= [];
            Dictionary<string, object> ret = [];

            foreach(var kv in lVal) { ret[kv.Key] = kv.Value; }
            foreach(var kv in rVal) { if(!ret.ContainsKey(kv.Key)) ret[kv.Key] = kv.Value; }

            return ret;
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
namespace AntDesign.Pro.Template

{
    public static class GlobalExtensions
    {
        public static string GetXMlValue(this string xml, params string[] keys)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            string keyStr = string.Empty;
            foreach (var r in keys)
            {
                keyStr += "/" + r;
            }
            keyStr = keyStr.Trim('/');
            if (xmldoc.SelectSingleNode(keyStr) != null)
            {
                return xmldoc.SelectSingleNode(keyStr).InnerText;
            }
            return "";
        }
        public static bool IsNullOrEmpty(this string strValue)
        {
            if (!string.IsNullOrEmpty(strValue) && strValue.Trim().Length != 0)
                return strValue.ToLower() == "null";
            return true;
        }

        public static bool IsNullOrEmpty(this string[] str)
        {
            if (str == null || str.Length == 0)
                return true;
            if (str.Length == 1)
                return IsNullOrEmpty(str[0]);
            return false;
        }

        public static bool IsNullOrEmpty(this ArrayList arrList)
        {
            if (arrList == null || arrList.Count == 0)
                return true;
            if (arrList.Count == 1)
                return IsNullOrEmpty(arrList[0].ToString());
            return false;
        }

        public static bool IsNullOrEmpty(this List<string> list)
        {
            if (list == null || list.Count == 0)
                return true;
            if (list.Count == 1)
                return IsNullOrEmpty(list[0]);
            return false;
        }

        public static bool IsNullOrEmpty(this object strValue)
        {
            if (strValue != null)
                return strValue == DBNull.Value;
            return true;
        }

        public static bool ToBool(this JObject obj, string targetCode, params string[] keys)
        {
            bool result = true;
            JToken temp = obj;
            if (obj != null)
            {
                foreach (var r in keys)
                {
                    temp = temp[r];
                    if (temp == null || temp.ToString() == "")
                    {
                        result = false;
                        break;
                    }
                }
            }
            result = result ? temp.ToString() == targetCode : false;
            return result;
        }


        public static bool ToBool(this JObject obj, bool bl, params string[] keys)
        {
            bool result = true;
            JToken temp = obj;
            if (obj != null)
            {
                foreach (var r in keys)
                {
                    temp = temp[r];
                    if (temp == null || temp.ToString() == "")
                    {
                        result = false;
                        break;
                    }
                }
            }
            result = temp.ToBool() == bl;
            return result;
        }
        public static string ToValue(this JObject obj, params string[] keys)
        {
            bool bl = true;
            string result = string.Empty;
            JToken temp = obj;
            if (obj != null)
            {
                foreach (var r in keys)
                {
                    try
                    {
                        temp = temp[r];
                        if (temp == null || temp.ToString() == "")
                        {
                            bl = false;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        bl = false;
                        break;
                    }
                }
            }
            result = bl ? temp.ToString().Replace("\r\n", "") : "";
            return result;
        }

        public static string ToTrim(this string obj)
        {
            return obj.ToString().Replace("\r\n", "").Replace("\\r\\n", "");
        }

        public static string ToValue(this JToken obj, params string[] keys)
        {
            bool bl = true;
            string result = string.Empty;
            JToken temp = obj;
            if (obj != null)
            {
                foreach (var r in keys)
                {
                    temp = temp[r];
                    if (temp == null || temp.ToString() == "")
                    {
                        bl = false;
                        break;
                    }
                }
            }
            result = bl&& temp!=null ? temp.ToString() : "";
            return result;
        }


        public static int ToInt(this object strValue)
        {
            return strValue.ToInt(0);
        }

        public static long ToLong(this object strValue, long defValue = 0)
        {
            if (strValue == null)
                return 0;
            long.TryParse(strValue.ToString().Trim(), out defValue);
            return defValue;
        }
        public static int ToInt(this object strValue, int defValue)
        {
            if (strValue == null)
                return defValue;
            int.TryParse(strValue.ToString().Trim(), out defValue);
            return defValue;
        }


        public static Decimal ToDecimal(this object strValue)
        {
            decimal result = 0;
            if (strValue == null)
            {
                return result;
            }
            decimal.TryParse(strValue.ToString().Trim(), out result); 
            return result;
        }

      


        public static Double ToDouble(this object strValue)
        {
            double result = 0;
            if (strValue == null)
            {
                return result;
            }
            double.TryParse(strValue.ToString().Trim(), out result);
            return result;
        }

        public static Decimal ToDecimal(this object strValue, Decimal defValue)
        {
            if (strValue == null || strValue.ToString().Trim().Length > 30)
                return defValue;
            strValue = (object)strValue.ToString().Replace(",", "").Trim();
            Decimal num = defValue;
            if (strValue != null && new Regex("^([-]|[0-9])[0-9]*(\\.\\w*)?$").IsMatch(strValue.ToString()))
                num = Convert.ToDecimal(Convert.ToDecimal(strValue).ToString());
            return num;
        }

        public static string ToArraystring(this string origin)
        {
            if (!string.IsNullOrEmpty(origin) && origin.IndexOf("[") != 0)
            {
                if (origin.Contains("{"))
                {
                    origin = $"[{origin}]";
                }
                else
                {
                    origin = $"[\"{origin}\"]";
                }
            }
            else if (string.IsNullOrEmpty(origin))
            {
                origin = "[]";
            }
            return origin;
        }

        public static bool ToBool(this object data)
        {
            if (data == null)
                return false;
            bool? value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            return bool.TryParse(data.ToString(), out result) && result;
        }
        private static bool? GetBool(this object data)
        {
            switch (data.ToString().Trim().ToLower())
            {
                case "0":
                    return false;

                case "1":
                    return true;

                case "是":
                    return true;

                case "否":
                    return false;

                case "yes":
                    return true;

                case "no":
                    return false;

                case "true":
                    return true;

                case "fasle":
                    return false;

                default:
                    return null;
            }
        }

        public static Guid ToGuid(this object strValue)
        {
            return strValue.ToGuid(Guid.Empty);
        }

        public static Guid ToGuid(this object strValue, Guid defValue)
        {
            if (strValue == null)
                return defValue;
            try
            {
                return new Guid(strValue.ToString());
            }
            catch
            {
                return defValue;
            }
        }



        public static DateTime ToDate(this object date)
        {
            try
            {
                if (date == null || string.IsNullOrEmpty(date.ToString()))
                {
                    return DateTime.Now;
                }
                return DateTime.Parse(date.ToString());
            }
            catch
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>

        public static long ConvertDateTimeInt(this DateTime time)
        {
            //double intResult = 0;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Ticks - startTime.Ticks) / 10000;            //除10000调整为13位
            return t;
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>

        public static System.DateTime ConvertIntDateTime(this double d)
        {

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime time = startTime.AddMilliseconds(d);
            return time;
        }

        public static string ToStringDefault(this object strValue)
        {
            if (strValue == null) { return ""; }
            return strValue.ToStringDefault("");
        }

        public static string ToStringDefault(this object strValue, string defValue)
        {
            if (strValue == null || strValue == DBNull.Value)
                return defValue;
            return strValue.ToString();
        }

        public static string ToStringDefault(this Dictionary<string, string> strValue)
        {
            return ToStringDefault(strValue, "");
        }

        public static string ToStringDefault(this Dictionary<string, string> strValue, string defValue)
        {
            if (strValue == null || strValue.Count == 0)
                return defValue;
            string str = "";
            foreach (KeyValuePair<string, string> keyValuePair in strValue)
                str = str + keyValuePair.Key + "=" + keyValuePair.Value + "&";
            return "?" + str.TrimEnd('&');
        }

        public static string ToY_M_D(this object data)
        {
            if (data == null)
                return null;
            DateTime result;
            bool isValid = DateTime.TryParse(data.ToString(), out result);
            if (isValid)
            {
                return result.ToString("yyyy-MM-dd");
            }
            return null;
        }
        public static string ToYMD(this object data)
        {
            if (data == null)
                return null;
            DateTime result;
            bool isValid = DateTime.TryParse(data.ToString(), out result);
            if (isValid)
            {
                return result.ToString("yyyyMMdd");
            }
            return null;
        }

        public static string ToJson(this object obj, bool bl = false)
        {
            //var options = new JsonSerializerOptions()
            //{
            //    IgnoreNullValues = true,
            //    WriteIndented = false,
            //    AllowTrailingCommas = true,
            //};
            //if (bl)
            //{
            //    JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            //}
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToString(this byte[] stream)
        {
            if (stream != null)
            {
                return Encoding.UTF8.GetString(stream);
            }
            return string.Empty;
        }
        public static byte[] ToBytes(this object obj)
        {
            string s = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None);
            return Encoding.UTF8.GetBytes(s);
        }
        /// <summary>
        /// 根据指定字段去重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string ToEnumDescription(this object e)
        {
            try
            {
                Type t = e.GetType();
                //获取枚举项的字段
                FieldInfo[] fis = t.GetFields();
                foreach (FieldInfo fi in fis)
                {
                    //如果当前字段名称不是当前枚举项
                    if (fi.Name != e.ToString())
                    {
                        continue;//结束本次循环
                    }
                    //如果当前字段的包含自定义特性
                    if (fi.IsDefined(typeof(DescriptionAttribute), true))
                    {
                        //获取自定义特性的属性值
                        return (fi.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute).Description;
                    }
                }
                return e.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static void Check(this object obj, string message)
        {
            if (obj == null)
            {
                throw new Exception(message);
            }
            if (obj is bool bl && !bl)
            {
                throw new Exception(message);
            }
            if (obj is Guid uid && uid == Guid.Empty)
            {
                throw new Exception(message);
            }
            if (obj is string key && string.IsNullOrEmpty(key))
            {
                throw new Exception(message);
            }
            if (obj is int nt && nt <= 0)
            {
                throw new Exception(message);
            }
            if (obj is DateTime dateTime && (dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue))
            {
                throw new Exception(message);
            }
            if (obj is IEnumerable enumerable && enumerable.Cast<object>() != null && !enumerable.Cast<object>().Any())
            {
                throw new Exception(message);
            }
            if (obj is List<object> list && list.Cast<object>() != null && !list.Cast<object>().Any())
            {
                throw new Exception(message);
            }
        }
        public static void Check(this object obj, string message, bool throwCondition = false)
        {
            if (obj is bool bl && bl == throwCondition)
            {
                throw new Exception(message);
            }
        }

        public static object ReturnNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is IEnumerable enumerable && enumerable.Cast<object>() != null && !enumerable.Cast<object>().Any())
            {
                return null;
            }
            if (obj is List<object> list && list.Cast<object>() != null && !list.Cast<object>().Any())
            {
                return null;
            }
            return null;
        }


        public static string ToRemoveImgPrefix(this string strValue, bool isThumbnail = true)
        {
            if (!string.IsNullOrEmpty(strValue))
            {
                strValue = strValue.Replace("http://image.tolvyo.com", "").Replace("http://image2.tolvyo.com", "").Replace("http://admin.tolvyo.com", "");
                if (isThumbnail)
                {
                    string route = strValue.Substring(0, strValue.LastIndexOf('.'));
                    string ext = strValue.Substring(strValue.LastIndexOf('.'));
                    strValue = route + "_detailthumbnail" + ext;
                }
            }
            return strValue;
        }

        /// <summary>
        /// 时间戳反转为时间，有很多中翻转方法，但是，请不要使用过字符串（string）进行操作，大家都知道字符串会很慢！
        /// </summary>
        /// <param name="TimeStamp">时间戳</param>
        /// <param name="AccurateToMilliseconds">是否精确到毫秒</param>
        /// <returns>返回一个日期时间</returns>
        public static DateTime ToDateTime(this long TimeStamp, bool AccurateToMilliseconds = false)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            if (AccurateToMilliseconds)
            {
                return startTime.AddTicks(TimeStamp * 10000);
            }
            else
            {
                return startTime.AddTicks(TimeStamp * 10000000);
            }
        }


        public static DateTime ToDateTime(this string timeStamp)
        {
            //处理字符串,截取括号内的数字
            var strStamp = Regex.Matches(timeStamp, @"(?<=\()((?<gp>\()|(?<-gp>\))|[^()]+)*(?(gp)(?!))").Cast<Match>().Select(t => t.Value).ToArray()[0].ToString();
            //处理字符串获取+号前面的数字
            var str = Convert.ToInt64(strStamp.Substring(0, strStamp.IndexOf("+")));
            long timeTricks = new DateTime(1970, 1, 1).Ticks + str * 10000 + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours * 3600 * (long)10000000;
            return new DateTime(timeTricks);

        }
        //public static Decimal ToCNDecimal(this string strValue)
        //{
        //    //−2 110,25
        //    ///-7,10
        //    decimal result = 0;
        //    if (strValue == null)
        //    {
        //        return result;
        //    }
        //    strValue= strValue.Replace(" ", "");
        //    string[] tempArry=strValue.Split(",");
        //    //逗号后面是2位 就判断小数点
        //    if (tempArry .Count()==2&& tempArry[1].Length>=3)//1,234.56
        //    {
        //        strValue = strValue.ToString().Replace(",", "");
        //    }
        //    else if (tempArry.Count() == 2&&tempArry[1].Length <=2)//1,23
        //    {
        //        strValue = strValue.ToString().Replace(",", ".");
        //    }
        //    decimal.TryParse(strValue.ToString().Trim(), out result);
        //    return result;
        //}
        public static string RegexString(this string orignalStr, string pattern, string replaceStr)
        {
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(orignalStr))
            {
                orignalStr = regex.Replace(orignalStr, replaceStr);
            }
            return orignalStr;
        }
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}


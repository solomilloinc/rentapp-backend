using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;

namespace rentap.backend.Core.Helpers
{
    public static class CustomExtensions
    {
        public static string GetText<T, TResult>(this T obj, Expression<Func<T, TResult>> path, string @default = null)
        {
            var result = obj.Get(path);
            return (ReferenceEquals(result, null)) ? @default : result.ToString();
        }

        public static TResult Get<T, TResult>(this T obj, Expression<Func<T, TResult>> path)
        {
            return obj.Get(path, default(TResult));
        }

        public static TResult Get<T, TResult>(this T obj, Expression<Func<T, TResult>> path, TResult @default)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            var items = Reflection.GetPropertyPath(path);
            var o = (object)obj;
            foreach (var item in items)
            {
                if (o == null)
                {
                    break;
                }

                var property = item.FieldOrProperty as PropertyInfo;
                if (property != null)
                {
                    o = property.GetValue(o, null);
                }
                else
                {
                    var field = item.FieldOrProperty as FieldInfo;
                    if (field != null)
                    {
                        o = field.GetValue(o);
                    }
                    else
                    {
                        throw new ArgumentException("The member at {0} is neither a field nor a property", "path");
                    }
                }
            }

            return o == null ? @default : (TResult)o;
        }

        public static bool IsInTimeRange(this DateTime obj, DateTime timeRangeFrom, DateTime timeRangeTo)
        {
            TimeSpan time = obj.TimeOfDay, t1From = timeRangeFrom.TimeOfDay, t1To = timeRangeTo.TimeOfDay;

            // if the time from is smaller than the time to, just filter by range
            if (t1From <= t1To)
            {
                return time >= t1From && time <= t1To;
            }

            // time from is greater than time to so two time intervals have to be created: one {timeFrom-12AM) and another one {12AM to timeTo}
            TimeSpan t2From = TimeSpan.MinValue, t2To = t1To;
            t1To = TimeSpan.MaxValue;

            return (time >= t1From && time <= t1To) || (time >= t2From && time <= t2To);
        }

        public static string CapitalizeFirstLetterOfEachWord(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        public static void MoveElementToLastIndex<T>(this List<T> list, T element)
        {
            list.Remove(element);
            list.Add(element);
        }

        public static string GetQueryString(this object obj, bool forcePropertyNamesInLowerCase = false)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select (forcePropertyNamesInLowerCase ? p.Name.ToLower() : p.Name) + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }

        public static DateTime? GetNullableDateTime(this DateTime date)
        {
            if (date == default(DateTime))
            {
                return null;
            }

            return date;
        }



        public static void AutoDispose(this object value)
        {

            var flags =
                  BindingFlags.Public | BindingFlags.NonPublic // Get public and non-public
                | BindingFlags.Static | BindingFlags.Instance // Get instance + static
                | BindingFlags.FlattenHierarchy;// Search up the hierarchy
            var propertyInfos = value.GetType().GetProperties(flags).Where(i => i.PropertyType.GetInterfaces().Contains(typeof(IDisposable)));
            var memberInfos = value.GetType().GetFields(flags).Where(i => i.FieldType.GetInterfaces().Contains(typeof(IDisposable)));
            foreach (var i in propertyInfos)
            {
                var o = (IDisposable)i.GetValue(value);
                if (o != null)
                {
                    o.Dispose();
                }
            }
            foreach (var i in memberInfos)
            {
                var o = (IDisposable)i.GetValue(value);
                if (o != null)
                {
                    o.Dispose();
                }
            }
        }

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

        public static int[] GetArrayFromCommaSeparatedValue(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new int[0];
            }

            return text.Split(',').Select(p => int.Parse(p)).ToArray();
        }

        public static bool ContainsCaseInsensitive(this string text, string toContain)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(toContain))
            {
                return false;
            }

            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(text, toContain, CompareOptions.IgnoreCase) >= 0;
        }

        public static bool StartsWithCaseInsensitive(this string text, string toContain)
        {
            return text.StartsWith(toContain, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EqualsCaseInsensitive(this string text, string toCompare)
        {
            return text.Equals(toCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets property name based on lambda expression instead of string value
        /// </summary>
        /// <returns>string</returns>
        public static string GetPropertyName<TType, TReturnType>(this TType @this, Expression<Func<TType, TReturnType>> propertyExpression)
        {
            return ((MemberExpression)propertyExpression.Body).Member.Name;
        }

        /// <summary>
        /// Gets PropertyInfo based on lambda expression instead of string value
        /// </summary>
        /// <returns>PropertyInfo</returns>
        public static PropertyInfo GetPropertyInfo<TType, TReturnType>(this TType @this, Expression<Func<TType, TReturnType>> propertyExpression)
        {
            return @this.GetType().GetProperty(@this.GetPropertyName(propertyExpression));
        }

        public static string ReplaceCaseInsensitive(this string text, string oldValue, string newValue)
        {
            return StringHelper.ReplaceCaseInsensitive(text, oldValue, newValue);
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ / parts into part
                         select part.AsEnumerable();
            return splits;
        }

        public static Expression<Func<T, K>>[] MergeExpressions<T, K>(this Expression<Func<T, K>>[] expressions, Expression<Func<T, K>>[] otherExpressions)
        {
            return expressions.Union(otherExpressions).DistinctBy(p => p.Body.Type).ToArray();
        }

        public static bool IsGreaterThanZero(this int? value)
        {
            return value.HasValue && value.Value > 0;
        }

        public static List<T> GetRandomObjects<T>(this List<T> list, int maxCount)
        {
            return list.OrderBy(p => Guid.NewGuid()).Take(maxCount).ToList();
        }

    }
}

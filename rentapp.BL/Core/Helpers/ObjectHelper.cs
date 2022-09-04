using System.Linq;
using System.Reflection;

namespace rentap.backend.Core.Helpers
{
    public static class ObjectHelper<T, K>
    {
        public static bool HasChanged(T source, K dest)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var property2 = typeof(K).GetProperty(property.Name);
                if (property2 == null)
                {
                    continue;
                }
                object val1 = property.GetValue(source);
                object val2 = property2.GetValue(dest);
                if (val1 != null && val2 != null && !val1.Equals(val2))
                {
                    return true;
                }
                if ((val1 == null && val2 != null) || (val1 != null && val2 == null))
                {
                    return true;
                }
            }
            return false;
        }
        public static void UpdatePropertyValues(T dto, K obj)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var property2 = typeof(K).GetProperty(property.Name);
                if (property2 == null)
                {
                    continue;
                }
                property2.SetValue(obj, property.GetValue(dto));
            }
        }
        public static K CopyAllTo(T source, K target)
        {
            var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var targetProperties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var sourceProperty in sourceProperties)
            {
                var targetProperty = targetProperties.SingleOrDefault(p => p.Name == sourceProperty.Name);
                if (targetProperty != null && sourceProperty.PropertyType == targetProperty.PropertyType)
                {
                    targetProperty.SetValue(target, sourceProperty.GetValue(source));
                }
            }
            return target;
        }
    }
}

using System;

namespace Doods.Framework.Std.Utilities
{
    public static class ReflectionUtils
    {
        public static bool IsNullableType(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, nameof(t));

            return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static bool IsNullable(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, nameof(t));

            if (t.IsValueType)
            {
                return IsNullableType(t);
            }

            return true;
        }
    }
}

﻿namespace Doods.Framework.Std.Validation
{
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null) return false;
            return !string.IsNullOrWhiteSpace(value.ToString());
        }
    }
}
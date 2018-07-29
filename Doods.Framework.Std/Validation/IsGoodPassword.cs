namespace Doods.Framework.Std.Validation
{
    public class IsGoodPassword<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null) return false;
            if (!(value is string)) return false;
            var v = value as string;
            if (string.IsNullOrWhiteSpace(v)) return false;
            if (v.Length < 8) return false;
            return true;
        }
    }
}
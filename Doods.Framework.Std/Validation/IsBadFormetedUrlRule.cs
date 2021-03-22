using System.Text.RegularExpressions;

namespace Doods.Framework.Std.Validation
{
    public class IsBadFormetedUrlRule<T> : IValidationRule<T>
    {
        //private const string regex = @"^(https?:\/\/)";
        private readonly Regex _haveHttpS = new Regex(@"^(https?:\/\/)");
        private readonly bool _needHttp;

        public IsBadFormetedUrlRule(bool needHttp)
        {
            _needHttp = needHttp;
        }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null) return false;
            var res = _haveHttpS.IsMatch(value.ToString().ToLower());

            if (!_needHttp) return !res;
            return res;
        }
    }
}
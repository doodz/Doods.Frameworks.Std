using System.Text.RegularExpressions;

namespace Doods.Framework.Std.Validation
{
    public class IsBadFormetedUrlRule<T> : IValidationRule<T>
    {
        private readonly bool _needHttp;
        public IsBadFormetedUrlRule(bool needHttp)
        {
            _needHttp = needHttp;
        }

        //private const string regex = @"^(https?:\/\/)";
        readonly Regex _haveHttpS = new Regex(@"^(https?:\/\/)");
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null) return false;
            var res = _haveHttpS.IsMatch(value.ToString());

            if (!_needHttp) return !res;
            return res;


        }
    }
}
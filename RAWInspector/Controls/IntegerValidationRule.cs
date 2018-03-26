using System.Globalization;
using System.Windows.Controls;

namespace RAWInspector.Controls
{
    internal sealed class IntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string s && int.TryParse(s, out var _))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Please enter an integer.");
        }
    }
}
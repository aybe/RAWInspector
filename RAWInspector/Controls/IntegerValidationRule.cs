using System;
using System.Globalization;
using System.Windows.Controls;

namespace RAWInspector.Controls
{
    internal sealed class IntegerValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Max < Min)
                throw new ArgumentOutOfRangeException(nameof(Max), Max,
                    $@"{nameof(Max)} must be greater than {nameof(Min)}.");

            if (value is string s && int.TryParse(s, out var i) && i >= Min && i <= Max)
                return ValidationResult.ValidResult;

            return new ValidationResult(false, $"Please enter an integer between {Min:N0} and {Max:N0}.");
        }
    }
}
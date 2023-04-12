using System;
using System.ComponentModel.DataAnnotations;

namespace Benraz.Infrastructure.Common.Validation
{
    /// <summary>
    /// Greater than attribute.
    /// </summary>
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        /// <summary>
        /// Creates attribute.
        /// </summary>
        /// <param name="comparisonProperty">Comparison property.</param>
        public GreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        /// <summary>
        /// Is valid.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="validationContext">Validation context.</param>
        /// <returns>Validation result.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = $"{validationContext.DisplayName} not later than {_comparisonProperty}.";

            if (value.GetType() == typeof(IComparable))
            {
                throw new ArgumentException("Value has not implemented IComparable interface.");
            }

            var currentValue = (IComparable)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                throw new ArgumentException("Comparison property with this name not found.");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue.GetType() == typeof(IComparable))
            {
                throw new ArgumentException("Comparison property has not implemented IComparable interface.");
            }

            if (!ReferenceEquals(value.GetType(), comparisonValue.GetType()))
            {
                throw new ArgumentException("The properties types must be the same.");
            }

            if (currentValue.CompareTo((IComparable)comparisonValue) <= 0)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}





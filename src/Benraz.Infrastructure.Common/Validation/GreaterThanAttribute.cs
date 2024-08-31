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
            // Check if value is null
            if (value == null)
            {
                return ValidationResult.Success; // Or return an error based on requirements
            }

            ErrorMessage = $"{validationContext.DisplayName} must be greater than {_comparisonProperty}.";

            // Check if value implements IComparable
            if (!(value is IComparable currentValue))
            {
                throw new ArgumentException("Value must implement IComparable interface.");
            }

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                throw new ArgumentException("Comparison property with this name not found.");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            
            // Check if comparisonValue is null
            if (comparisonValue == null)
            {
                return ValidationResult.Success; // Or return an error based on requirements
            }

            // Check if comparisonValue implements IComparable
            if (!(comparisonValue is IComparable comparableValue))
            {
                throw new ArgumentException("Comparison property must implement IComparable interface.");
            }

            // Ensure both values are of the same type
            if (currentValue.GetType() != comparableValue.GetType())
            {
                throw new ArgumentException("The properties types must be the same.");
            }

            // Perform the comparison
            if (currentValue.CompareTo(comparableValue) <= 0)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}





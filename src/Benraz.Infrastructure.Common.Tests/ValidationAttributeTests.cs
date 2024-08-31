using Benraz.Infrastructure.Common.Validation;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace Benraz.Infrastructure.Common.Tests
{
    [TestFixture]
    public class ValidationAttributesTests
    {
        private class GreaterThanAttributeValidationModel
        {
            public dynamic LessValue { get; set; }

            [GreaterThan("LessValue")]
            public dynamic GreaterValue { get; set; }
        }

        [TestCase(1, 2, true)]
        [TestCase(1, 1, false)]
        [TestCase(2, 1, false)]
        [TestCase(0.001, 0.01, true)]
        [TestCase(0.001, 0.001, false)]
        [TestCase(0.01, 0.001, false)]
        [TestCase('a', 'b', true)]
        [TestCase('a', 'a', false)]
        [TestCase('b', 'a', false)]
        [TestCase("01/20/2012", "01/21/2012", true)]
        [TestCase("01/20/2012", "01/20/2012", false)]
        [TestCase("01/21/2012", "01/20/2012", false)]
        [TestCase("test", "test+", true)]
        [TestCase("test", "test", false)]
        [TestCase("test+", "test", false)]
        [TestCase(false, true, true)]
        [TestCase(false, false, false)]
        [TestCase(true, false, false)]
        public void GreaterThanAttribute_IsValid(object lessValue, object greaterValue, bool isValid)
        {
            var model = new GreaterThanAttributeValidationModel
            {
                LessValue = lessValue,
                GreaterValue = greaterValue
            };

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);

            Assert.That(result, Is.EqualTo(isValid));
        }
    }
}

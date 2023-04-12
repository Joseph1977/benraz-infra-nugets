using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benraz.Infrastructure.Domain.CodeGenerator
{
    /// <summary>
    /// Code generator
    /// </summary>
    public static class CodeGenerator
    {
        /// <summary>
        /// Generate code.
        /// </summary>
        /// <param name="size">Size.</param>
        /// <param name="useNumbers">Use number.</param>
        /// <param name="useSmallLetter">Use small letter.</param>
        /// <returns></returns>
        public static string GenerateCode(int size = 10, bool useNumbers = true, bool useSmallLetter = true)
        {
            IEnumerable<string> smallLetters = new List<string>();
            IEnumerable<string> number = new List<string>();

            if (useNumbers)
                number = Enumerable.Range(0, 10).Select(e => e.ToString());

            if (useSmallLetter)
                smallLetters = Enumerable.Range(97, 26).Select(e => ((char)e).ToString());

            var builder = new StringBuilder();
            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(smallLetters)
                .Concat(number)
                .OrderBy(e => Guid.NewGuid())
                .Take(size)
                .ToList().ForEach(e => builder.Append(e));
            return builder.ToString();
        }
    }
}




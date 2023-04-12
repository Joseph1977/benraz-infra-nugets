namespace Benraz.Infrastructure.Common.DataRedundancy
{
    /// <summary>
    /// Data redundancy checker.
    /// </summary>
    public class DrChecker : IDrChecker
    {
        /// <summary>
        /// Creates DR checker.
        /// </summary>
        public DrChecker()
        {
        }

        /// <summary>
        /// Returns true if data redundancy active, otherwise false.
        /// </summary>
        /// <returns>True if data redundancy active, otherwise false.</returns>
        public bool IsActiveDR()
        {
            return true;
        }
    }
}





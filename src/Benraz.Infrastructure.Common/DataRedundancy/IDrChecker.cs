namespace Benraz.Infrastructure.Common.DataRedundancy
{
    /// <summary>
    /// Data redundancy checker.
    /// </summary>
    public interface IDrChecker
    {
        /// <summary>
        /// Returns true if data redundancy active, otherwise false.
        /// </summary>
        /// <returns>True if data redundancy active, otherwise false.</returns>
        bool IsActiveDR();
    }
}





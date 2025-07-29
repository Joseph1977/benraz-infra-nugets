namespace Benraz.Infrastructure.Authorization.ClaimValidators
{
    /// <summary>
    /// Claim match type.
    /// </summary>
    public enum ClaimMatchType
    {
        /// <summary>
        /// All (matches all specified claims).
        /// </summary>
        All = 1,

        /// <summary>
        /// At least one (requires at least one matching claim).
        /// </summary>
        AtLeastOne = 2
    }
}

namespace Benraz.Infrastructure.Authorization.ClaimValidators
{
    /// <summary>
    /// Claim filter type.
    /// </summary>
    public enum ClaimFilterType
    {
        /// <summary>
        /// Exact (matches claims that are an exact (case-insensitive)).
        /// </summary>
        Exact = 1,

        /// <summary>
        /// Include (matches claims that contain the specified value).
        /// </summary>
        Include = 2
    }
}

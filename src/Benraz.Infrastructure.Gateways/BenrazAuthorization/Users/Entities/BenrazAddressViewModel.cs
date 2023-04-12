namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Entities
{
    /// <summary>
    /// Address view model.
    /// </summary>
    public class BenrazAddressViewModel
    {
        /// <summary>
        /// Street address.
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Locality.
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Country.
        /// </summary>
        public string Country { get; set; }
    }
}



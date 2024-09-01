using Newtonsoft.Json;

namespace Benraz.Infrastructure.Gateways.BenrazServices.Messages
{
    /// <summary>
    /// Benraz services base request.
    /// </summary>
    public abstract class BenrazServicesRequestBase : HttpRequestBase
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }
    }
	/// <summary>
	/// Benraz services base request.
	/// </summary>
	public abstract class BenrazServicesRequestBaseUserPassword
	{
		/// <summary>
		/// Username.
		/// </summary>
		[JsonIgnore]
		public string Username { get; set; }

		/// <summary>
		/// Password.
		/// </summary>
		[JsonIgnore]
		public string Password { get; set; }
	}
}




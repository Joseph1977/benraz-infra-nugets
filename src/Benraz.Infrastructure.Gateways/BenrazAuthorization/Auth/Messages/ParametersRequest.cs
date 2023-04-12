namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages
{
    /// <summary>
    /// Auth parameters request.
    /// </summary>
    public class ParametersRequest
    {
        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            return "v1/auth/parameters";
        }
    }
}




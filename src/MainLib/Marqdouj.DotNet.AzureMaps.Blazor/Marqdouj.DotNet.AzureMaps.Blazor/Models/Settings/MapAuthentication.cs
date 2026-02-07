using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Settings
{
    /// <summary>
    /// Authentication method to be used when creating the map.
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// The subscription key authentication mechanism.
        /// </summary>
        subscriptionKey,

        /// <summary>
        /// The AAD implicit grant mechanism. Recommended for pages protected by a sign-in.
        /// By default the page will be redirected to the AAD login when the map control initializes.
        /// </summary>
        aad,

        /// <summary>
        /// The anonymous authentication mechanism. Recommended for public pages.
        /// Allows a callback responsible for acquiring an authentication token to be provided.
        /// </summary>
        anonymous,

        /// <summary>
        /// The shared access signature authentication mechanism. 
        /// Allows a callback responsible for acquiring a token to be provided on requests.
        /// </summary>
        sas,
    }

    /// <summary>
    /// Authentication configuration for the Azure Map.
    /// </summary>
    public class MapAuthentication
    {
        /// <summary>
        /// The authentication method to be used.
        /// </summary>
        [JsonIgnore]
        public AuthenticationType AuthType { get; set; }

        [JsonInclude]
        [JsonPropertyName("authType")]
        internal string AuthTypeJs => AuthType.ToString();

        /// <summary>
        /// Subscription key from your Azure Maps account.
        /// Must be specified for subscription key authentication type.
        /// </summary>
        public string? SubscriptionKey { get; set; }

        /// <summary>
        /// The URL for the Shared Access Signature (SAS) token for your Azure Maps Account.
        /// If <see cref="AuthType"/> = sas and this value is set, it will override the getAuthTokenCallback configured in App.Razor.
        /// </summary>
        public string? SasTokenUrl { get; set; }

        /// <summary>
        /// The Azure AD registered app ID. This is the app ID of an app registered in your Azure AD tenant.
        /// Must be specified for AAD authentication type.
        /// </summary>
        public string? AadAppId { get; set; }

        /// <summary>
        /// The AAD tenant that owns the registered app specified by 'aadAppId'.
        /// Must be specified for AAD authentication type.</summary>
        public string? AadTenant { get; set; }

        /// <summary>
        /// The Azure Maps client ID, This is an unique identifier used to identify the maps account.
        /// Preferred to always be specified, but must be specified for AAD and anonymous authentication types.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Indicates whether the authentication options are valid for the specified authentication type.
        /// </summary>
        /// <returns></returns>
        internal bool IsValid()
        {
            return AuthType switch
            {
                AuthenticationType.subscriptionKey => !string.IsNullOrWhiteSpace(SubscriptionKey),
                AuthenticationType.sas => true, //Requires configuring JSInvokable GetSasToken method in App.Razor or configuring a SAS Token url.
                AuthenticationType.aad => !string.IsNullOrWhiteSpace(AadAppId) && !string.IsNullOrWhiteSpace(AadTenant),
                AuthenticationType.anonymous => !string.IsNullOrWhiteSpace(ClientId),
                _ => false
            };
        }

        internal bool IsNotValid() => !IsValid();

        internal string InValidMessage()
        {
            return AuthType switch
            {
                AuthenticationType.subscriptionKey => string.IsNullOrWhiteSpace(SubscriptionKey)
                    ? "SubscriptionKey is required when authentication Mode is SubscriptionKey."
                    : "",
                AuthenticationType.sas => "", //Requires configuring JSInvokable GetSasToken method in App.Razor or configuring a SAS Token url.
                AuthenticationType.aad => string.IsNullOrWhiteSpace(AadAppId) || string.IsNullOrWhiteSpace(AadTenant)
                    ? "AadAppId and AadTenant are required when authentication Mode is Aad."
                    : "",
                AuthenticationType.anonymous => string.IsNullOrWhiteSpace(ClientId)
                    ? "ClientId is required when authentication Mode is Anonymous."
                    : "",
                _ => ""
            };
        }
    }
}


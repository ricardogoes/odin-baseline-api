namespace Odin.Baseline.Domain.DTO.Common
{
    public class AppSettings
    {
        public AWSCognitoSettings AWSCognitoSettings { get; private set; }
        public ConnectionStrings? ConnectionStrings { get; private set; }
        public int? CancellationTokenTimeout { get; private set; }

        public AppSettings(AWSCognitoSettings aWSCognitoSettings, ConnectionStrings connectionStrings, int cancellationTokenTimeout)
        {
            AWSCognitoSettings = aWSCognitoSettings;
            ConnectionStrings = connectionStrings;
            CancellationTokenTimeout = cancellationTokenTimeout;
        }

        public AppSettings(AWSCognitoSettings aWSCognitoSettings)
        {
            AWSCognitoSettings = aWSCognitoSettings;
        }
    }

    public class AWSCognitoSettings
    {
        public string? AccessKeyId { get; private set; }
        public string? AccessSecretKey { get; private set; }
        public string? AppClientId { get; private set; }
        public string CognitoAuthorityUrl { get; private set; }
        public string? Region { get; private set; }

        public AWSCognitoSettings(string cognitoAuthorityUrl)
        {
            CognitoAuthorityUrl = cognitoAuthorityUrl;            
        }

        public AWSCognitoSettings(string accessKeyId, string accessSecretKey, string appClientId, string cognitoAuthorityUrl, string region)
        {
            AccessKeyId = accessKeyId;
            AccessSecretKey = accessSecretKey;
            AppClientId = appClientId;
            CognitoAuthorityUrl = cognitoAuthorityUrl;
            Region = region;
        }
    }

    public class ConnectionStrings
    {        
        public string OdinBaselineDB { get; private set; }

        public ConnectionStrings(string odinBaselineDB)
        {
            OdinBaselineDB = odinBaselineDB;
        }
    }
}

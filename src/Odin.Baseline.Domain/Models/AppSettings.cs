namespace Odin.Baseline.Domain.Models
{
    public class AppSettings
    {
        public AWSCognitoSettings AWSCognitoSettings { get; set; }
    }

    public class AWSCognitoSettings
    {
        public string CognitoAuthorityUrl { get; set; }
    }
}

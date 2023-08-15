namespace Odin.Baseline.Domain.Models
{
    public class AppSettings
    {
        public AWSCognitoSettings AWSCognitoSettings { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public int CancellationTokenTimeout { get; set; }
    }

    public class AWSCognitoSettings
    {
        public string CognitoAuthorityUrl { get; set; }
    }

    public class ConnectionStrings
    {
        public string OdinBaselineDB { get; set; }
    }
}

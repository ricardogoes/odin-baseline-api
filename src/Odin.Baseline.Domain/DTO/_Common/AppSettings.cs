namespace Odin.Baseline.Domain.DTO.Common
{
    public class AppSettings
    {
        public ConnectionStrings? ConnectionStrings { get; private set; }
        public int? CancellationTokenTimeout { get; private set; }

        public AppSettings(ConnectionStrings connectionStrings, int cancellationTokenTimeout)
        {
            ConnectionStrings = connectionStrings;
            CancellationTokenTimeout = cancellationTokenTimeout;
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

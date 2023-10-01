namespace Odin.Baseline.Domain.Models.AppSettings
{
    public class ConnectionStringsSettings
    {
        public string OdinBaselineDbConnection { get; private set; }

        public ConnectionStringsSettings(string odinBaselineDbConnection)
        {
            OdinBaselineDbConnection = odinBaselineDbConnection;
        }
    }
}

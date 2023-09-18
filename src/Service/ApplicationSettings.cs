namespace Service;

public static class ApplicationSettings
{
    public const string ServiceName = "Nebim.Era.Plt.Comm.Customer";
    public static readonly string ServiceVersion = Environment.GetEnvironmentVariable("SERVICE_VERSION") ?? "1.0.0";
    public const string ChangeDataCaptureTopicName = "Nebim.Era.Plt.Comm.Customer.Cdc";
    public const string AppSecretPrefix = "Nebim_Era_Plt_Comm_Customer";
    public const string AppSecretCommonPrefix = "Nebim_Era_Plt_Common";
}

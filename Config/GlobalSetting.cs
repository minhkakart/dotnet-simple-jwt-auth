namespace BaseAuth.Config;

public class GlobalSetting
{
    public static AppSetting? AppSetting { get; private set; }
    public static void IncludeConfig(AppSetting? appSetting)
    {
        AppSetting = appSetting;
    }
}
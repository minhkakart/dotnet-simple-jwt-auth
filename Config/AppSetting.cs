namespace BaseAuth.Config;

public class Database
{
    public string ConnectionString { get; set; } = string.Empty;
}

public class AppSetting
{
    public Database? Database { get; init; }
}
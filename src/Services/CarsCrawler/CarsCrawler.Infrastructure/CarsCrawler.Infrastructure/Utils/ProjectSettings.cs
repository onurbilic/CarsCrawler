namespace CarsCrawler.Infrastructure.Utils;

public class ProjectSettings
{

}
public record RabbitMqSetting
{
    public RabbitMqSetting(string serverName, string userName, string password)
    {
        ServerName = serverName;
        UserName = userName;
        Password = password;
    }

    public string ServerName { get; init; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
namespace CarsCrawler.Domain.Model
{
    public record LoginModel
    {
        // It should be on appsettings but in the test case I want to use hard coded.
        //TODO : set value login info from appsettings.json
        internal string UserName { get; init; } = "johngerson808@gmail.com";
        internal string Password { get; init; } = "test8008";
    }
}
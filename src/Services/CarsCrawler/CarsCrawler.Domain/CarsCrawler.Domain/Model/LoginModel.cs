namespace CarsCrawler.Domain.Model
{
    public class LoginModel
    {
        // It should be on appsettings but in the test case I want to use hard coded.
        //TODO : set value login info from appsettings.json
        public string UserName { get; set; } = "johngerson808@gmail.com";
        public string Password { get; set; } = "test8008";
    }
}
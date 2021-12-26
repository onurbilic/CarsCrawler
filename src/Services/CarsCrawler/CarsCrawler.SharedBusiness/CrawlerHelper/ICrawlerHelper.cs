using CarsCrawler.Domain.Model;
using System;

namespace CarsCrawler.Infrastructure.SharedBusiness
{
    interface ICrawlerHelper
    {
        public String Login(LoginModel loginModel);
    }
}

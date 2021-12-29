using CarsCrawler.Domain.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;

namespace CarsCrawler.Infrastructure.SharedBusiness
{
    public class CrawlerHelper : ICrawlerHelper
    {
        public string Login(LoginModel loginModel)
        {

            return "Ok";
        }
    }
}

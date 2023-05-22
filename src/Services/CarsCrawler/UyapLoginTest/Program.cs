// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using PuppeteerSharp;

namespace UyapLoginTest;

class Program
{
    [DebuggerDisplay("Content: {Content} Url: {Url}")]
    public class Data
    {
        public string Title { get; set; }
    }

    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var options = new LaunchOptions {Headless = true};

        Console.WriteLine("Downloading chromium");

        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        
        using (var browser = await Puppeteer.LaunchAsync(options))
        using (var page = await browser.NewPageAsync())
        {
            await page.GoToAsync("https://esatis.uyap.gov.tr/main/esatis/giris.jsp");
            
            var jsCode = @"Application.getEDevletESifreEntryURL()";
            
            var results = await page.EvaluateExpressionAsync(jsCode);

            await page.GoToAsync(results.ToString());
            
            Console.WriteLine(page.Url);

            var jsCodeLogin = @" document.getElementById('tridField').value = '12445277732';

            document.getElementById('egpField').value = '13O02n18ur';";
            
            var resultEdevlet = await page.EvaluateExpressionAsync(jsCodeLogin);
            await page.ClickAsync("input.submitButton");
            
            Thread.Sleep(500);
            Console.WriteLine(page.Url);
            
            var jsCodeIlan = @"() => {
                const selectors = Array.from(document.querySelectorAll('div[class=""pricing-head""] > h4:first-child'));
                return selectors.map( t=> {return { title: t.innerText}});
                }";
            var resultIlan = await page.EvaluateFunctionAsync<Data[]>(jsCodeIlan);
            Thread.Sleep(10000);

            
            foreach (var result in resultIlan)
            {
                Console.WriteLine(result.ToString());
            }
            
            Console.ReadLine();
        }
    }
}
using CarsCrawler.Domain.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CarsCrawler.Infrastructure.CefCrawler;
using CefSharp;
using CefSharp.OffScreen;

namespace CarsCrawler.Infrastructure.SharedBusiness
{
    public class CrawlerHelper : ICrawlerHelper
    {
        public string Login(LoginModel loginModel)
        {
            AsyncContext.Run(async delegate
            {
                var settings = new CefSettings()
                {
                    CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "CefSharp\\Cache")
                };

                var success =
                    await Cef.InitializeAsync(settings, performDependencyCheck: true, browserProcessHandler: null);

                if (!success)
                {
                    throw new Exception("Unable to initialize CEF, check the log file.");
                }

                using (var browser = new ChromiumWebBrowser(Consts.testUrl))
                {
                    var initialLoadResponse = await browser.WaitForInitialLoadAsync();

                    if (!initialLoadResponse.Success)
                    {
                        throw new Exception(string.Format("Page load failed with ErrorCode:{0}, HttpStatusCode:{1}",
                            initialLoadResponse.ErrorCode, initialLoadResponse.HttpStatusCode));
                    }

                    _ = await browser.EvaluateScriptAsync(
                        "document.querySelector('[name=q]').value = 'CefSharp Was Here!'");

                    //Give the browser a little time to render
                    await Task.Delay(500);
                    // Wait for the screenshot to be taken.
                    var bitmapAsByteArray = await browser.CaptureScreenshotAsync();

                    // File path to save our screenshot e.g. C:\Users\{username}\Desktop\CefSharp screenshot.png
                    var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        "CefSharp screenshot.png");

                    Console.WriteLine();
                    Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

                    File.WriteAllBytes(screenshotPath, bitmapAsByteArray);

                    Console.WriteLine("Screenshot saved. Launching your default image viewer...");

                    // Tell Windows to launch the saved image.
                    Process.Start(new ProcessStartInfo(screenshotPath)
                    {
                        // UseShellExecute is false by default on .NET Core.
                        UseShellExecute = true
                    });

                    Console.WriteLine("Image viewer launched. Press any key to exit.");
                }

                // Wait for user to press a key before exit
                Console.ReadKey();

                // Clean up Chromium objects. You need to call this in your application otherwise
                // you will get a crash when closing.
                Cef.Shutdown();
            });

            return "Ok";
        }
    }
}

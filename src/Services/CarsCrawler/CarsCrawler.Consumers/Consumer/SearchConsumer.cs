using System.Diagnostics;
using CarsCrawler.Domain.Model;
using CarsCrawler.Infrastructure.CefCrawler;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using CarsCrawler.SharedBusiness.Commands;
using CefSharp;
using CefSharp.OffScreen;

namespace CarsCrawler.Consumers.Consumer
{
    public class SearchConsumer : IConsumer<ISearchCarsCommand>
    {
        public Task Consume(ConsumeContext<ISearchCarsCommand> context)
        {
            Console.WriteLine(context);
            
            GetScreenShot();

            return Task.CompletedTask;
        }

        private void GetScreenShot()
        {
            AsyncContext.Run(async delegate
            {
                var settings = new CefSettings()
                {
                    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                    CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "CefSharp\\Cache")
                };

                //Perform dependency check to make sure all relevant resources are in our output directory.
                var success =
                    await Cef.InitializeAsync(settings, performDependencyCheck: true, browserProcessHandler: null);

                if (!success)
                {
                    throw new Exception("Unable to initialize CEF, check the log file.");
                }

                // Create the CefSharp.OffScreen.ChromiumWebBrowser instance
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
        }
    }


    public class SearchConsumerDefinition : ConsumerDefinition<SearchConsumer>
    {
        public SearchConsumerDefinition()
        {
            EndpointName = "In.Carsdotcom.Search";
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<SearchConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = true;
            endpointConfigurator.ClearMessageDeserializers();
            endpointConfigurator.UseRawJsonSerializer();
        }
    }
}
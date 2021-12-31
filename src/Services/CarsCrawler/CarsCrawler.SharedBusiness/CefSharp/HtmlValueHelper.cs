using CarsCrawler.SharedBusiness.Commands;

namespace CarsCrawler.SharedBusiness.CefSharp
{
    public enum HtmlSelector
    {
        name = 1,
        id = 2,
        select = 3,
        buttonClick = 4,
        submitFormClassName = 5,
        getVehicleCard = 6
    }
    public static class HtmlValueHelper
    {
        public static string SetHtmlValue(string key, string value, HtmlSelector selector)
        {
            switch (selector)
            {
                case HtmlSelector.name:
                    return string.Format("document.querySelector('[name={0}]').value = '{1}'", key, value);
                case HtmlSelector.id:
                    return string.Format("document.querySelector('[id={0}]').value = '{1}';", key, value);
                case HtmlSelector.select:
                    var js = string.Format(@"
                        document.getElementById('{0}').selectedIndex = 0
                        document.getElementById('{0}').options[0].text = 'Model S'
                        document.getElementById('{0}').options[0].value = '{1}'", key, value);
                    return js;
                case HtmlSelector.submitFormClassName:
                    return string.Format("document.getElementsByClassName('{0}')[0].submit();", key);
                case HtmlSelector.getVehicleCard:
                    var ve = @"(function()
                                {
                                    var vehicleArray = []; 
  
                                    document.getElementsByClassName('vehicle-card').forEach(function(vehicle) 
                                    { 
                                            const car = {};
                                            car.id = vehicle.id;
                                            car.title = vehicle.getElementsByClassName('title')[0].innerHTML
                                            car.image = vehicle.getElementsByClassName('vehicle-image')[0].src;
                                            car.stockType = vehicle.getElementsByClassName('stock-type')[0].innerHTML;
                                            car.miles = vehicle.getElementsByClassName('mileage')[0].innerHTML;
                                            car.price = vehicle.getElementsByClassName('primary-price')[0].innerHTML;
                                            car.reportLink = vehicle.getElementsByClassName('sds-link--ext')[0].href;
                                            car.dealerName = vehicle.getElementsByClassName('dealer-name')[0].innerHTML;
                                            car.rating = vehicle.getElementsByClassName('sds-rating__count')[0].innerHTML;
                                            vehicleArray.push(car); 
                                    });  

                                    return vehicleArray;

                                })();";
                    return ve;

                default:
                    return string.Empty;
            }
        }
    }

    public static class UrlHelper
    {
        public static string SearchUrlWithPage(string baseUrl, ISearchCarsCommand search, int navigatedPage)
        {

            string navigatedUrl = string.Format(@"{0}shopping/results/?
                                                page={1}&
                                                page_size=20&
                                                list_price_max={2}&
                                                makes[]={3}&
                                                maximum_distance={4}&
                                                models[]={5}&
                                                stock_type={6}&
                                                zip={7}",
                                                baseUrl,
                                                navigatedPage.ToString(),
                                                search.Price,
                                                search.Makes,
                                                search.Distance,
                                                search.Models,
                                                search.StockType,
                                                search.Zip);

            return navigatedUrl;
        }
    }
}

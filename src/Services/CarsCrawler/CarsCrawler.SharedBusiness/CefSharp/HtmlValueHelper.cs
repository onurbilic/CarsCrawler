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
        getVehicleCard = 6,
        getVehicleDetail = 7
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
                                            car.title = vehicle.getElementsByClassName('title')[0].innerText
                                            car.image = vehicle.getElementsByClassName('vehicle-image')[0].src;
                                            car.stockType = vehicle.getElementsByClassName('stock-type')[0].innerText;
                                            car.miles = vehicle.getElementsByClassName('mileage')[0].innerText;
                                            car.price = vehicle.getElementsByClassName('primary-price')[0].innerText;
                                            car.reportLink = vehicle.getElementsByClassName('sds-link--ext')[0].href;
                                            car.dealerName = vehicle.getElementsByClassName('dealer-name')[0].innerText;
                                            car.rating = vehicle.getElementsByClassName('sds-rating__count')[0].innerText;
                                            vehicleArray.push(car); 
                                    });  

                                    return vehicleArray;

                                })();";
                    return ve;


                case HtmlSelector.getVehicleDetail:
                    var vehicleDetailQuery = @"(function()
                                                {
			                                    	let vehicle = { }

			                                    	vehicle.stockType = document.getElementsByClassName('new-used')[0].innerHTML;

                                                    vehicle.title = document.getElementsByClassName('listing-title')[0].innerHTML;
                                                    vehicle.mileage = document.getElementsByClassName('listing-mileage')[0].innerHTML;
                                                    vehicle.price = document.getElementsByClassName('primary-price')[0].innerHTML;

                                                    let dt = document.querySelectorAll('.basics-section > .fancy-description-list > dt');
                                                    let dd = document.querySelectorAll('.basics-section > .fancy-description-list > dd');

                                                    var basicinfo = [];
                                                    for (i = 0; i < dt.length; i++)
                                                    {
                                                        const basicdetail= { key:'',value: ''};
                                                        basicdetail.key = dt[i].innerText;
                                                        basicdetail.value = dd[i].innerText;
                                                        console.log(basicdetail.key);
                                                        basicinfo.push(basicdetail);
                                                    }

                                                    let dtf = document.querySelectorAll('.features-section > .fancy-description-list > dt');
                                                    let ddf = document.querySelectorAll('.features-section > .fancy-description-list > dd');

                                                    var featuresinfo = [];
                                                    for (i = 0; i < dtf.length; i++)
                                                    {
                                                        const featuresdetail= { key:'',value: ''};
                                                        featuresdetail.key = dtf[i].innerText;
                                                        featuresdetail.value = ddf[i].innerText;
                                                        console.log(featuresdetail.key);
                                                        featuresinfo.push(featuresdetail);
                                                    }

                                                    vehicle.basicInfo = basicinfo;
		                                        		
		                                        	vehicle.featuresInfo = featuresinfo;
		                                        	
                                                    if(document.getElementsByClassName('seller-name')[0] != undefined){vehicle.sellerName = document.getElementsByClassName('seller-name')[0].innerText}else{vehicle.sellerName = '-' };
		                                        	
                                                    if(document.getElementsByClassName('dealer-phone')[0] != undefined){vehicle.dealerPhone = document.getElementsByClassName('dealer-phone')[0].innerText}else{vehicle.dealerPhone =  '-' };

                                                    if(document.getElementsByClassName('sds-rating')[0] != undefined){vehicle.rating = document.getElementsByClassName('sds-rating')[0].innerText}else{vehicle.rating =  '-' };

                                                    if(document.getElementsByClassName('dealer-address')[0] != undefined){vehicle.dealerAddress = document.getElementsByClassName('dealer-address')[0].innerText}else{vehicle.dealerAddress =  '-' };

                                                    if(document.getElementsByClassName('sds-link--ext')[0] != undefined){vehicle.extLink = document.getElementsByClassName('sds-link--ext')[0].innerText}else{vehicle.extLink =  '-' };

                                                    if(document.getElementsByClassName('sellers-notes')[0] != undefined){vehicle.sellerNotes = document.getElementsByClassName('sellers-notes')[0].innerText}else{vehicle.sellerNotes =  '-' };
		                                        	
		                                        	return vehicle;
                                                })();";
                    return vehicleDetailQuery;
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

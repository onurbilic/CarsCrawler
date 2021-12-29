namespace CarsCrawler.Infrastructure.Utils
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
                    return String.Format("document.querySelector('[name={0}]').value = '{1}'", key, value);
                case HtmlSelector.id:
                    return String.Format("document.querySelector('[id={0}]').value = '{1}';", key, value);
                case HtmlSelector.select:
                    var js = String.Format(@"
                        document.getElementById('{0}').selectedIndex = 0
                        document.getElementById('{0}').options[0].text = 'Model S'
                        document.getElementById('{0}').options[0].value = '{1}'", key, value);
                    return js;
                case HtmlSelector.submitFormClassName:
                    return String.Format("document.getElementsByClassName('{0}')[0].submit();", key);
                case HtmlSelector.getVehicleCard:
                    var ve = String.Format(@"
                                            var vehicleArray = [];
                                            document.getElementsByClassName('{0}').forEach(function(vehicle) {
                                            if (vehicle.id)
                                                {
                                                    vehicleArray.push(vehicle.id);
                                                    console.log(vehicle.id);
                                                }
                                        });
                                        vehicleArray;
                                        ", key);
                    return ve;

                default:
                    return string.Empty;
            }
        }
    }
}

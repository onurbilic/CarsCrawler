namespace CarsCrawler.Infrastructure.Utils
{
    public enum HtmlSelector
    {
        name = 1,
        id = 2,
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
                    return String.Format("document.getElementById('{0}').value = '{1}'", key, value);
                default:
                    return String.Format("document.querySelector('[name={0}]').value = '{1}'", key, value);
            }
        }
    }
}

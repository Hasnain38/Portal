using System.Text.RegularExpressions;

namespace Portal.Web.Xss
{
    
    public class DefaultHtmlSanitizer : IHtmlSanitizer
    {
        public string Sanitize(string html)
        {
            return Regex.Replace(html, "<.*?>|&.*?;", string.Empty);
        }
    }
}
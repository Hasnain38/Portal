using Abp.Dependency;

namespace Portal.Web.Xss
{
    public interface IHtmlSanitizer: ITransientDependency
    {
        string Sanitize(string html);
    }
}
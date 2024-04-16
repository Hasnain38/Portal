using System.Threading.Tasks;

namespace Portal.Net.Emailing
{
    public interface IEmailSettingsChecker
    {
        bool EmailSettingsValid();

        Task<bool> EmailSettingsValidAsync();
    }
}
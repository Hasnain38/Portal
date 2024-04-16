using System.Threading.Tasks;

namespace Portal.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}
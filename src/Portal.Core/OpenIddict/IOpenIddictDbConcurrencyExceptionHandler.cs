using System.Threading.Tasks;
using Abp.Domain.Uow;

namespace Portal.OpenIddict
{
    public interface IOpenIddictDbConcurrencyExceptionHandler
    {
        Task HandleAsync(AbpDbConcurrencyException exception);
    }
}
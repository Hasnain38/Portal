using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Portal.MultiTenancy.Accounting.Dto;

namespace Portal.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}

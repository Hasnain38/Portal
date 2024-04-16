using Abp.AutoMapper;
using Portal.MultiTenancy.Payments.Dto;

namespace Portal.Web.Areas.App.Models.SubscriptionManagement;

[AutoMapFrom(typeof(SubscriptionPaymentProductDto))]
public class ShowDetailModalViewModel : SubscriptionPaymentProductDto
{
}
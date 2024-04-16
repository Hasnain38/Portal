using Abp.Application.Services.Dto;

namespace Portal.Authorization.Users.Dto
{
    public interface IGetLoginAttemptsInput: ISortedResultRequest
    {
        string Filter { get; set; }
    }
}
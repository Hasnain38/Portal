using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Portal.Dto;

namespace Portal.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}

using Abp.Domain.Services;

namespace Portal.Authorization.Users.Password
{
    public interface IPasswordExpirationService : IDomainService
    {
        void ForcePasswordExpiredUsersToChangeTheirPassword();
    }
}

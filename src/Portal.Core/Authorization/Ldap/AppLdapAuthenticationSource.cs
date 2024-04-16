using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Portal.Authorization.Users;
using Portal.MultiTenancy;

namespace Portal.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}
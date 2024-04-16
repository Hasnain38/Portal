using Microsoft.EntityFrameworkCore;
using Portal.OpenIddict.Applications;
using Portal.OpenIddict.Authorizations;
using Portal.OpenIddict.Scopes;
using Portal.OpenIddict.Tokens;

namespace Portal.EntityFrameworkCore
{
    public interface IOpenIddictDbContext
    {
        DbSet<OpenIddictApplication> Applications { get; }

        DbSet<OpenIddictAuthorization> Authorizations { get; }

        DbSet<OpenIddictScope> Scopes { get; }

        DbSet<OpenIddictToken> Tokens { get; }
    }

}
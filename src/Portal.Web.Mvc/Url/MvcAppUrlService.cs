﻿using Abp.MultiTenancy;
using Portal.Url;

namespace Portal.Web.Url
{
    public class MvcAppUrlService : AppUrlServiceBase
    {
        public override string EmailActivationRoute => "Account/EmailConfirmation";
        
        public override string EmailChangeRequestRoute => "Account/EmailChangeRequest";

        public override string PasswordResetRoute => "Account/ResetPassword";

        public MvcAppUrlService(
                IWebUrlService webUrlService,
                ITenantCache tenantCache
            ) : base(
                webUrlService,
                tenantCache
            )
        {

        }
    }
}
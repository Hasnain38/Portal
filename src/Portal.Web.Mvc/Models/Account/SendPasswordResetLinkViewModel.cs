using System.ComponentModel.DataAnnotations;

namespace Portal.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}
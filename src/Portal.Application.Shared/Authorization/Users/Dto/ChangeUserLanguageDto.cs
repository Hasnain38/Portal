using System.ComponentModel.DataAnnotations;

namespace Portal.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}

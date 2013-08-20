using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZV_Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="Введите имя")]
        [DisplayName("Ваше имя")]
        public string UserName { get; set; }
    }
}
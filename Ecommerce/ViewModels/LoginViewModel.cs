using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo correo es requerido")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo valido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo contraseña es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Mantener la sesión iniciada")]
        public bool Recuerdame { get; set; }
    }
}

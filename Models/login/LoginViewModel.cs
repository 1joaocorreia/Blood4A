using System.ComponentModel.DataAnnotations;

namespace Blood4A.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Por favor, insira o login!")]
    public required string Login {get; set;}
    [Required(ErrorMessage = "Por favor, insira a senha de acesso!")]
    public required string Password {get; set;}
}
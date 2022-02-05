using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required(ErrorMessage = "Username can not be null.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password can not be null.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password can not be null.")]
    public string Email { get; set; }
}
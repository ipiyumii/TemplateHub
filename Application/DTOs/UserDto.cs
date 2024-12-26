using System.ComponentModel.DataAnnotations;

namespace boilerplate_app.Application.DTOs
{
    public class UserDto
    {
        [Required] public string? UserName { get; set; }
        [Required] public string? FullName { get; set; }
        [Required] public string? Email { get; set; }
      
    }
}

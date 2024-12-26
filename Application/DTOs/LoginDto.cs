using System.ComponentModel.DataAnnotations;

namespace boilerplate_app.Application.DTOs
{
    public class LoginDto
    {
        public required string UserName { set; get; }
        public required string Password { get; set; }

    }
}

using System.Net;

namespace boilerplate_app.Application.DTOs
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
}

using Microsoft.AspNetCore.Http;

namespace AnonymousStudentReviews.Api.Features.Universities.UploadIcon;

public class UploadUniversityIconRequest
{
    public IFormFile File { get; set; } = default!;
}

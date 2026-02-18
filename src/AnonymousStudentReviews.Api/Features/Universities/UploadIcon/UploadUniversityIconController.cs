using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.UseCases.Universities.UploadIcon;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Universities.UploadIcon;

[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = RoleNameConstants.Admin)]
[ApiController]
[Route("api/universities/{universityId:guid}/icon")]
public class UploadUniversityIconController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly IValidator<UploadUniversityIconRequest> _validator;
    private readonly IUploadUniversityIconService _service;

    public UploadUniversityIconController(
        IWebHostEnvironment environment,
        IValidator<UploadUniversityIconRequest> validator,
        IUploadUniversityIconService service)
    {
        _environment = environment;
        _validator = validator;
        _service = service;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(UploadUniversityIconResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UploadUniversityIconResponse>> Upload(
        [FromRoute] Guid universityId,
        [FromForm] UploadUniversityIconRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var ext = Path.GetExtension(request.File.FileName).ToLowerInvariant();
        if (ext is not (".png" or ".jpg" or ".jpeg" or ".webp"))
        {
            return Problem(
                detail: "Unsupported file type. Allowed: png, jpg, jpeg, webp",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var webRoot = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
        {
            webRoot = Path.Combine(AppContext.BaseDirectory, "wwwroot");
        }

        var dir = Path.Combine(webRoot, "uploads", "universities");
        Directory.CreateDirectory(dir);

        var fileName = $"{universityId:N}_{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(dir, fileName);

        await using (var stream = System.IO.File.Create(fullPath))
        {
            await request.File.CopyToAsync(stream);
        }

        var iconUrl = $"/uploads/universities/{fileName}";

        var result = await _service.ExecuteAsync(universityId, iconUrl);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return Ok(new UploadUniversityIconResponse { IconUrl = result.Value });
    }
}

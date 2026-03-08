using System.ComponentModel.DataAnnotations;

namespace AnonymousStudentReviews.Api.Features.Auth.ViewModels;

public class AuthorizeViewModel
{
    [Display(Name = "Application")] public string? ApplicationName { get; set; }

    [Display(Name = "Scope")] public string? Scope { get; set; }
}

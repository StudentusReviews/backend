using System.ComponentModel.DataAnnotations;

namespace AnonymousStudentReviews.Api.Options;

public class OpenIddictCertificateOptions
{
    public const string SectionName = "OpenIddictCertificates";

    [Required(ErrorMessage =
        "Missing configuration value for 'OpenIddictCertificates:EncryptionCertificateFileContainerPath'.")]
    public string EncryptionCertificateFileContainerPath { get; init; } = string.Empty;

    [Required(ErrorMessage =
        "Missing configuration value for 'OpenIddictCertificates:SigningCertificateFileContainerPath'.")]
    public string SigningCertificateFileContainerPath { get; init; } = string.Empty;

    [Required(ErrorMessage =
        "Missing configuration value for 'OpenIddictCertificates:EncryptionCertificatePassword'.")]
    public string EncryptionCertificatePassword { get; init; } = string.Empty;

    [Required(ErrorMessage =
        "Missing configuration value for 'OpenIddictCertificates:SigningCertificatePassword'.")]
    public string SigningCertificatePassword { get; init; } = string.Empty;
}

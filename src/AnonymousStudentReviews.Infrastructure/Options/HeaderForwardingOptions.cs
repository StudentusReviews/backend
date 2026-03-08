using System.ComponentModel.DataAnnotations;

namespace AnonymousStudentReviews.Infrastructure.Options;

public class HeaderForwardingOptions
{
    public const string SectionName = "HeaderForwarding";

    [Required(ErrorMessage = "Missing configuration value for 'HeaderForwarding:KnownNetworks'.")]
    public List<KnownNetwork> KnownNetworks { get; init; } = [];

    [Required(ErrorMessage = "Missing configuration value for 'HeaderForwarding:KnownProxies'.")]
    public List<KnownProxy> KnownProxies { get; init; } = [];
}

public class KnownNetwork
{
    [Required(ErrorMessage = "Missing configuration value for 'Prefix' in a 'HeaderForwarding:KnownNetworks' entry.")]
    public string Prefix { get; init; } = string.Empty;

    [Required(ErrorMessage = "Missing configuration value for 'PrefixLength' in a 'HeaderForwarding:KnownNetworks' entry.")]
    [Range(0, 128, ErrorMessage = "The 'PrefixLength' in a 'HeaderForwarding:KnownNetworks' entry must be between 0 and 128.")]
    public int PrefixLength { get; init; }
}

public class KnownProxy
{
    [Required(ErrorMessage = "Missing configuration value for 'IpAddress' in a 'HeaderForwarding:KnownProxies' entry.")]
    public string IpAddress { get; init; } = string.Empty;
}

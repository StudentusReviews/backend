namespace AnonymousStudentReviews.Infrastructure.Options;

public class HeaderForwardingOptions
{
    public const string SectionName = "HeaderForwarding";

    public List<KnownNetwork> KnownNetworks { get; set; } = [];
    public List<KnownProxy> KnownProxies { get; set; } = [];
}

public class KnownNetwork
{
    public required string Prefix { get; set; }
    public int PrefixLength { get; set; }
}

public class KnownProxy
{
    public required string IpAddress { get; set; }
}

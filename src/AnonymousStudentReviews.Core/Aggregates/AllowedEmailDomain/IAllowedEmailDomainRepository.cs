using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;

public interface IAllowedEmailDomainRepository
{
    Task<Result<AllowedEmailDomain>> FindByDomainAsync(string domain);
    Task<bool> IsEmailDomainAllowed(string emailDomain);
}

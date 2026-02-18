using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;

public class ViewByIdApplicationToAddAUniversityService : IViewByIdApplicationToAddAUniversityService
{
    private readonly IApplicationToAddAUniversityRepository _applicationRepository;

    public ViewByIdApplicationToAddAUniversityService(IApplicationToAddAUniversityRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<Result<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>> ExecuteAsync(Guid id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        return application;
    }
}

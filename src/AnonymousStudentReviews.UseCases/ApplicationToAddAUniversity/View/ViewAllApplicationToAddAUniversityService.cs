using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;

public class ViewAllApplicationToAddAUniversityService : IViewAllApplicationsToAddAUniversityService
{
    private readonly IApplicationToAddAUniversityRepository _applicationRepository;

    public ViewAllApplicationToAddAUniversityService(IApplicationToAddAUniversityRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<Result<List<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>>> ExecuteAsync()
    {
        var applications = _applicationRepository.GetAll();
        return applications.Result;
    }
}

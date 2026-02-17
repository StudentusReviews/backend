using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;

public class ViewApplicationToAddAUniversityService : IViewApplicationToAddAUniversityService
{
    private readonly IApplicationToAddAUniversityRepository _applicationRepository;

    public ViewApplicationToAddAUniversityService(IApplicationToAddAUniversityRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<Result<List<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>>> ExecuteAsync()
    {
        var applications = _applicationRepository.GetAll();
        return applications.Result;
    }

    public async Task<Result<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>> ExecuteAsync(Guid id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        return application;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;

namespace AnonymousStudentReviews.UseCases.AppToAddAUni.View;

public class ViewAppToAddAUniService : IViewAppToAddAUniService
{
    private readonly IApplicationRepository _applicationRepository;

    ViewAppToAddAUniService(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<Result<List<Core.Aggregates.AppToAddAUni.AppToAddAUni>>> ExecuteAsync()
    {
        var applications = _applicationRepository.GetAll();
        return applications.Result;
    }

    public async Task<Result<Core.Aggregates.AppToAddAUni.AppToAddAUni>> ExecuteAsync(Guid id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        return application;
    }
}

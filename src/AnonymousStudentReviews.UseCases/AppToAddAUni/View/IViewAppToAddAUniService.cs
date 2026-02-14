using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.AppToAddAUni.View;

public interface IViewAppToAddAUniService
{
    public Task<Result<List<Core.Aggregates.AppToAddAUni.AppToAddAUni>>> ExecuteAsync();
    public Task<Result<Core.Aggregates.AppToAddAUni.AppToAddAUni>> ExecuteAsync(Guid id);
}

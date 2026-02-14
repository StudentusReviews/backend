using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;

public interface IViewApplicationToAddAUniversityService
{
    public Task<Result<List<Core.Aggregates.ApplicationToAddAUniversity.ApplicationToAddAUniversity>>> ExecuteAsync();
    public Task<Result<Core.Aggregates.ApplicationToAddAUniversity.ApplicationToAddAUniversity>> ExecuteAsync(Guid id);
}

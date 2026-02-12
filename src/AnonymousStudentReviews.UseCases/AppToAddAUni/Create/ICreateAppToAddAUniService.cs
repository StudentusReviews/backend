using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;

namespace AnonymousStudentReviews.UseCases.AppToAddAUni.Create;

public interface ICreateAppToAddAUniService
{
    Task<Result<Core.Aggregates.AppToAddAUni.AppToAddAUni>> ExecuteAsync(CreateAppToAddAUniDto dto);
}

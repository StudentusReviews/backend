using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.AppToAddAUni.Delete;

public interface IDeleteAppToAddAUniService
{
    public Task<Result> ExecuteAsync(Guid appId);
}

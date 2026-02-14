using AnonymousStudentReviews.Core.Abstractions;
<<<<<<< HEAD
=======
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity;
>>>>>>> 7eca784 (Changed names of classes. AppToAddAUni -> ApplicationToAddAUniversity)

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Create;

public interface ICreateApplicationToAddAUniversityService
{
<<<<<<< HEAD
    Task<Result<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>> ExecuteAsync(CreateApplicationToAddAUniversityDto dto);
=======
    Task<Result<Core.Aggregates.ApplicationToAddAUniversity.ApplicationToAddAUniversity>> ExecuteAsync(CreateApplicationToAddAUniversityDto dto);
>>>>>>> 7eca784 (Changed names of classes. AppToAddAUni -> ApplicationToAddAUniversity)
}

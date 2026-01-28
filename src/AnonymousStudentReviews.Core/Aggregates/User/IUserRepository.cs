namespace AnonymousStudentReviews.Core.Aggregates.User;

public interface IUserRepository
{
    Task<bool> UserWithEmailHashExistsAsync(string emailHash);
    void CreateUser(User user);
    void ConfirmUser(User user);
}

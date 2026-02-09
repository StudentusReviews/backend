namespace AnonymousStudentReviews.Infrastructure.Email;

public interface IViewToStringRenderer
{
    Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
}

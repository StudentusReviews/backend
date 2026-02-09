using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace AnonymousStudentReviews.Infrastructure.Email;

public class ViewToStringRenderer : IViewToStringRenderer
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly ITempDataProvider _tempDataProvider;

    public ViewToStringRenderer(IRazorViewEngine razorViewEngine, IHttpContextAccessor httpContextAccessor,
        ITempDataProvider tempDataProvider)
    {
        _razorViewEngine = razorViewEngine;
        _httpContextAccessor = httpContextAccessor;
        _tempDataProvider = tempDataProvider;
    }

    public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
    {
        var razorViewEngineResult = _razorViewEngine.GetView(null, viewName, false);

        if (razorViewEngineResult.View == null)
        {
            throw new Exception("Could not find the View file. Searched locations:\r\n" +
                                string.Join("\r\n", razorViewEngineResult.SearchedLocations));
        }

        var view = razorViewEngineResult.View;

        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            throw new InvalidOperationException("Http context is inaccessible");
        }

        var actionContext =
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

        await using var outputStringWriter = new StringWriter();

        var viewContext = new ViewContext(
            actionContext,
            view,
            new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            },
            new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
            outputStringWriter,
            new HtmlHelperOptions());

        await view.RenderAsync(viewContext);

        return outputStringWriter.ToString();
    }
}

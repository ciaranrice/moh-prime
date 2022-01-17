using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using RazorEngine;
using RazorEngine.Templating;

using Prime.Services.Razor;
using Prime.Models;
using Microsoft.Extensions.Logging;

namespace Prime.Services
{
    public class RazorConverterService : IRazorConverterService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailTemplateService _emailTemplateService;

        protected readonly ILogger _logger;


        public RazorConverterService(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IEmailTemplateService emailTemplateService,
            IHttpContextAccessor contextAccessor,
            ILogger<RazorConverterService> logger)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _contextAccessor = contextAccessor;
            _emailTemplateService = emailTemplateService;

            _logger = logger;
        }

        public async Task<string> RenderTemplateToStringAsync<TModel>(RazorTemplate<TModel> template, TModel viewModel)
        {
            // Need to create a new service scope since httpContext is not avaialble in root DI scope
            // when used outside of webapi, e.g. in a de-coupled system to render HTML
            using var serviceScope = _serviceProvider.CreateScope();
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceScope.ServiceProvider
            };
            var routeData = new RouteData();
            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());

            var view = GetView(actionContext, template.ViewPath);

            using var output = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                view,
                new ViewDataDictionary<TModel>(
                    metadataProvider: new EmptyModelMetadataProvider(),
                    modelState: new ModelStateDictionary())
                {
                    Model = viewModel
                },
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                output,
                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            return output.ToString();
        }

        public string RenderStringTemplateToString<TModel>(string template, TModel model)
        {
            _logger.LogDebug("RazorConverterService.RenderStringTemplateToString called ...");
            try
            {
                Guid guid = Guid.NewGuid();
                _logger.LogDebug("Before Engine.Razor.RunCompile ...");
                var rendered = Engine.Razor.RunCompile(template, guid.ToString(), typeof(TModel), model);
                _logger.LogDebug("After Engine.Razor.RunCompile ...");
                return rendered;
            }
            // Implicitly rethrow the exception to preserve stack trace:
            // https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2200
            catch (TemplateCompilationException e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }

        }

        public async Task<string> RenderEmailTemplateToString<TModel>(EmailTemplateType type, TModel viewModel)
        {
            _logger.LogDebug("RazorConverterService.RenderEmailTemplateToString called ...");
            var emailTemplate = await _emailTemplateService.GetEmailTemplateByTypeAsync(type);
            var rendered = RenderStringTemplateToString(emailTemplate.Template, viewModel);
            _logger.LogDebug("RazorConverterService.RenderEmailTemplateToString completing ...");
            return rendered;
        }

        private IView GetView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }
    }
}

using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace UmbracoVirtualRedirectsTest.ContentFinders;

/// <summary>
/// Custom content finder that creates a virtual page at /virtual-test
/// This is used to test whether redirect methods respect content finders
/// </summary>
public class VirtualPageContentFinder : IContentFinder
{
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly ITemplateService _templateService;

    public VirtualPageContentFinder(
        IUmbracoContextAccessor umbracoContextAccessor,
        ITemplateService templateService)
    {
        _umbracoContextAccessor = umbracoContextAccessor;
        _templateService = templateService;
    }

    public Task<bool> TryFindContent(IPublishedRequestBuilder request)
    {
        // Check if the requested path is our virtual page
        var path = request.Uri.GetAbsolutePathDecoded();
        
        if (!path.Equals("/virtual-test", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(false);
        }

        // Get the Umbraco context
        if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
        {
            return Task.FromResult(false);
        }

        // Get the home page (or any published content) to use as a base
        #pragma warning disable CS0618 // Type or member is obsolete
        var homePage = umbracoContext.Content?.GetAtRoot().FirstOrDefault();
        #pragma warning restore CS0618 // Type or member is obsolete
        
        if (homePage == null)
        {
            return Task.FromResult(false);
        }

        // Set the published content - this creates a "virtual" page
        // The URL is /virtual-test but the content is actually the home page
        request.SetPublishedContent(homePage);
        
        // Try to get and set the VirtualTest template
        var template = _templateService.GetAsync("VirtualTest").GetAwaiter().GetResult();
        if (template != null)
        {
            request.SetTemplate(template);
        }
        
        return Task.FromResult(true);
    }
}

using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace UmbracoVirtualRedirectsTest.Controllers;

/// <summary>
/// Surface controller to test the behavior of different redirect methods
/// with content finders and virtual pages
/// </summary>
public class RedirectTestSurfaceController : SurfaceController
{
    public RedirectTestSurfaceController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
    }

    /// <summary>
    /// Test method using RedirectToCurrentUmbracoPage()
    /// According to the discussion: Should redirect to the current published content item
    /// (whatever was resolved on the way in), avoiding content finders
    /// </summary>
    [HttpPost]
    public IActionResult TestRedirectToCurrentUmbracoPage(string testValue)
    {
        // Add a temp data message to show on the redirect
        TempData["Message"] = $"RedirectToCurrentUmbracoPage called with value: {testValue}";
        TempData["Method"] = "RedirectToCurrentUmbracoPage()";
        TempData["CurrentUrl"] = Request.Path.ToString();
        
        // Get the current published content for logging
        if (CurrentPage != null)
        {
            TempData["ContentId"] = CurrentPage.Id;
            TempData["ContentName"] = CurrentPage.Name;
            TempData["ContentUrl"] = CurrentPage.Url();
        }

        return RedirectToCurrentUmbracoPage();
    }

    /// <summary>
    /// Test method using RedirectToCurrentUmbracoUrl()
    /// According to the discussion: Should redirect to the current path,
    /// going through content finders again
    /// </summary>
    [HttpPost]
    public IActionResult TestRedirectToCurrentUmbracoUrl(string testValue)
    {
        // Add a temp data message to show on the redirect
        TempData["Message"] = $"RedirectToCurrentUmbracoUrl called with value: {testValue}";
        TempData["Method"] = "RedirectToCurrentUmbracoUrl()";
        TempData["CurrentUrl"] = Request.Path.ToString();
        
        // Get the current published content for logging
        if (CurrentPage != null)
        {
            TempData["ContentId"] = CurrentPage.Id;
            TempData["ContentName"] = CurrentPage.Name;
            TempData["ContentUrl"] = CurrentPage.Url();
        }

        return RedirectToCurrentUmbracoUrl();
    }

    /// <summary>
    /// Test method using CurrentUmbracoPage()
    /// According to the discussion: Does a return View() keeping the current URL,
    /// probably not going through content finders either
    /// </summary>
    [HttpPost]
    public IActionResult TestCurrentUmbracoPage(string testValue)
    {
        // Add a view data message to show in the view
        ViewData["Message"] = $"CurrentUmbracoPage called with value: {testValue}";
        ViewData["Method"] = "CurrentUmbracoPage()";
        ViewData["CurrentUrl"] = Request.Path.ToString();
        
        // Get the current published content for logging
        if (CurrentPage != null)
        {
            ViewData["ContentId"] = CurrentPage.Id;
            ViewData["ContentName"] = CurrentPage.Name;
            ViewData["ContentUrl"] = CurrentPage.Url();
        }

        return CurrentUmbracoPage();
    }
}

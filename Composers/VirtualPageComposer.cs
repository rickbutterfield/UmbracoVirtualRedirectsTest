using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using UmbracoVirtualRedirectsTest.ContentFinders;

namespace UmbracoVirtualRedirectsTest.Composers;

/// <summary>
/// Composer to register the custom VirtualPageContentFinder
/// </summary>
public class VirtualPageComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        // Append our custom content finder to the collection
        builder.ContentFinders().Append<VirtualPageContentFinder>();
    }
}

using Application.Abstractions.LinkService;

namespace API.Services;

internal sealed class LinkServices : ILinkServices
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public LinkServices(
        LinkGenerator linkGenerator, 
        IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public Link Generate(string endpointName, object? routeValue, string rel, string method)
    {
        return new Link(
            _linkGenerator.GetUriByName(
                _httpContextAccessor.HttpContext, 
                endpointName, 
                routeValue), 
                rel, 
                method);
    }
}

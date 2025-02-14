namespace Application.Abstractions.LinkService;

public interface ILinkServices
{
    Link Generate(string endpointName, object? routeValue, string rel, string method);
}

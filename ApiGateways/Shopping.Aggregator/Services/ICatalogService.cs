using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface ICatalogService
{
    Task<IEnumerable<CatalogModel>?> GetCatalog();
    Task<CatalogModel?> GetCatalog(string id);
    Task<IEnumerable<CatalogModel>?> GetCatalogByCategory(string category);
}

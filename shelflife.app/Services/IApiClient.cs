using shelflife.DTO;

namespace shelflife.app.Services;

/// <summary>
/// Abstraction over the REST API used by the app.  Exposes methods for
/// retrieving and persisting items.  Using an interface simplifies
/// dependency injection and unit testing.
/// </summary>
public interface IApiClient
{
    Task<List<DtoItem>> GetItems(string? search = null);
    Task<DtoItem?> GetItem(int id);
    Task<int?> CreateItem(DtoCreateItem dto);
    Task<bool> UpdateItem(int id, DtoUpdateItem dto);
    Task<bool> DeleteItem(int id);
}
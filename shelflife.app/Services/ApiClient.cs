using System.Net.Http.Json;
using shelflife.DTO;

namespace shelflife.app.Services;

/// <summary>
/// Concrete implementation of <see cref="IApiClient"/> that uses
/// <see cref="HttpClient"/> to communicate with the backend REST API.  The
/// BaseAddress is configured by <see cref="Constants.BaseUrl"/> when the
/// dependency is registered.  All calls will throw if the server returns
/// non‑success responses; callers should handle null results where
/// appropriate.
/// </summary>
public class ApiClient : IApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        // Ensure that the HttpClient has the correct BaseAddress.  On Android
        // this will point to 10.0.2.2 so that the emulator can reach the
        // host machine.  Constants.BaseUrl contains the full scheme and
        // authority (including port).
        http.BaseAddress = new Uri(Constants.BaseUrl);
        _http = http;
    }

    public async Task<List<DtoItem>> GetItems(string? search = null)
    {
        var url = "/api/items";
        if (!string.IsNullOrWhiteSpace(search))
        {
            url += $"?search={Uri.EscapeDataString(search)}";
        }
        try
        {
            var items = await _http.GetFromJsonAsync<List<DtoItem>>(url);
            return items ?? new List<DtoItem>();
        }
        catch
        {
            // In case of any network or deserialization error, return an
            // empty list rather than crashing the UI.  Logging could be
            // added here for more insight in a real world scenario.
            return new List<DtoItem>();
        }
    }

    public async Task<DtoItem?> GetItem(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<DtoItem>($"/api/items/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<int?> CreateItem(DtoCreateItem dto)
    {
        var response = await _http.PostAsJsonAsync("/api/items", dto);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        try
        {
            var data = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
            if (data != null && data.TryGetValue("id", out var id))
            {
                return id;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateItem(int id, DtoUpdateItem dto)
    {
        var response = await _http.PutAsJsonAsync($"/api/items/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteItem(int id)
    {
        var response = await _http.DeleteAsync($"/api/items/{id}");
        return response.IsSuccessStatusCode;
    }
}
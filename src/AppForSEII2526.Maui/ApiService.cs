// ApiService.cs
using AppForSEII2526.Maui.Models.DTOs;
using System.Net.Http;
using System.Net.Http.Json;

public class ApiService
{
    private readonly HttpClient client;

    public ApiService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<IList<ClassForPlanDTO>> GetClassesForPlanning(DateTime? date = null, string[]? types = null)
    {
        try
        {
            var query = new List<string>();
            if (date.HasValue)
                query.Add($"date={date.Value:yyyy-MM-dd}");
            if (types != null && types.Any())
                query.AddRange(types.Select(t => $"types={Uri.EscapeDataString(t)}"));

            var url = "api/Class/GetClassesForPlanning"; // Agregué "api/Class/"
            if (query.Any())
                url += "?" + string.Join("&", query);

            Console.WriteLine($"Calling API: {url}");

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<IList<ClassForPlanDTO>>();
                Console.WriteLine($"API call successful: {result?.Count ?? 0} classes received");
                return result ?? new List<ClassForPlanDTO>();
            }
            else
            {
                Console.WriteLine($"API call failed: {response.StatusCode}");
                return new List<ClassForPlanDTO>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in API call: {ex.Message}");
            return new List<ClassForPlanDTO>();
        }
    }
}
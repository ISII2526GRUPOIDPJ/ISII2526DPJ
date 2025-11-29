using AppForSEII2526.Maui.Models.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Diagnostics;

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

            // SEGUNDA OPCIÓN: Formato separado por comas
            if (types != null && types.Any())
                query.Add($"types={string.Join(",", types.Select(Uri.EscapeDataString))}");

            var url = "api/Class/GetClassesForPlanning";
            if (query.Any())
                url += "?" + string.Join("&", query);

            Debug.WriteLine($"🔍 URL generada: {url}");

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<IList<ClassForPlanDTO>>();
                Debug.WriteLine($"✅ Clases recibidas: {result?.Count ?? 0}");
                return result ?? new List<ClassForPlanDTO>();
            }
            else
            {
                Debug.WriteLine($"❌ Error API: {response.StatusCode}");
                return new List<ClassForPlanDTO>();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"💥 Exception: {ex.Message}");
            return new List<ClassForPlanDTO>();
        }
    }
}
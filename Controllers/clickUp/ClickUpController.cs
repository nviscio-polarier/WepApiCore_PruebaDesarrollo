using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApiCore.Class.clickUp;

namespace WebApiCore.Controllers;
[Route("[controller]")]
public class ClickUpController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ClickUpController(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.Add("Authorization", "pk_49655475_3367A8NYSAD5WAX1BQW1D4O1AV9R502W");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [Route("GetTasks")]
    [HttpGet]
    public async Task<IActionResult> GetTasks(DateTime fechaDesde, DateTime fechaHasta)
    {
        try
        {
            // Convertir fechas a Unix time in milliseconds
            var startDate = new DateTimeOffset(fechaDesde).ToUnixTimeMilliseconds();
            var endDate = new DateTimeOffset(fechaHasta).ToUnixTimeMilliseconds();

            var url = $"https://api.clickup.com/api/v2/team/9015386592/time_entries?start_date={startDate}&end_date={endDate}&assignee=81691234%2C49674809%2C49674807%2C81691233&include_task_tags=true&include_location_names=true";
            var response = await GetApiResponse<ClickUp.TimeEntriesResponse>(url);

            // Manejo de posibles valores nulos en task
            var formattedEntries = response.Data
                .Where(entry => entry.Task != null)
                .Select(entry => new FormattedTimeEntry
                {
                    Username = entry.User.Username,
                    Start = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(entry.Start)).DateTime,
                    Stop = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(entry.End)).DateTime,
                    FolderName = entry.TaskLocation.FolderName,
                    TaskName = entry.Task.Name,
                    Tags = entry.TaskTags?.Select(tag => tag.Name).ToList() ?? new List<string>(),
                    TaskId = entry.Task.Id,
                    ListId = entry.TaskLocation.ListId,
                    FolderId = entry.TaskLocation.FolderId,
                    SpaceId = entry.TaskLocation.SpaceId
                }).ToList();

            return Ok(formattedEntries);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    public class FormattedTimeEntry
    {
        public string Username { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string FolderName { get; set; }
        public string TaskName { get; set; }
        public List<string> Tags { get; set; }
        public string TaskId { get; set; }
        public string ListId { get; set; }
        public string FolderId { get; set; }
        public string SpaceId { get; set; }
    }


    private async Task<T> GetApiResponse<T>(string url)
    {
        int retries = 5;
        int delay = 10000; // Comienza con 5 segundos

        for (int i = 0; i < retries; i++)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(jsonResponse);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(delay);
                delay *= 2; // Aumenta el tiempo de espera exponencialmente
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
        }

        throw new HttpRequestException("Exceeded retry attempts due to Too Many Requests (429).");
    }


}
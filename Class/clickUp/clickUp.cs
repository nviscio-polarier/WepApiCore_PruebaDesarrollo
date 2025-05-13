namespace WebApiCore.Class.clickUp
{
    using System.Text.Json.Serialization;
    using System.Collections.Generic;
    using System.Text.Json;

    public class ClickUp
    {
        public class TimeEntriesResponse
        {
            [JsonPropertyName("data")]
            public List<TimeEntry> Data { get; set; }
        }

        public class TimeEntry
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("task")]
            [JsonConverter(typeof(TaskConverter))] // Usar un convertidor personalizado
            public Task Task { get; set; }

            [JsonPropertyName("wid")]
            public string Wid { get; set; }

            [JsonPropertyName("user")]
            public User User { get; set; }

            [JsonPropertyName("billable")]
            public bool Billable { get; set; }

            [JsonPropertyName("start")]
            public string Start { get; set; }

            [JsonPropertyName("end")]
            public string End { get; set; }

            [JsonPropertyName("duration")]
            public string Duration { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("task_location")]
            public TaskLocation TaskLocation { get; set; }

            [JsonPropertyName("task_tags")]
            public List<TaskTag> TaskTags { get; set; }

            [JsonPropertyName("task_url")]
            public string TaskUrl { get; set; }
        }

        public class Task
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("status")]
            public Status Status { get; set; }

            [JsonPropertyName("custom_type")]
            public int? CustomType { get; set; }
        }

        public class Status
        {
            [JsonPropertyName("status")]
            public string Name { get; set; }

            [JsonPropertyName("color")]
            public string Color { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("orderindex")]
            public int OrderIndex { get; set; }
        }

        public class User
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("username")]
            public string Username { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("color")]
            public string Color { get; set; }

            [JsonPropertyName("initials")]
            public string Initials { get; set; }

            [JsonPropertyName("profilePicture")]
            public string? ProfilePicture { get; set; }
        }

        public class TaskLocation
        {
            [JsonPropertyName("list_id")]
            public string ListId { get; set; }

            [JsonPropertyName("folder_id")]
            public string FolderId { get; set; }

            [JsonPropertyName("space_id")]
            public string SpaceId { get; set; }

            [JsonPropertyName("list_name")]
            public string ListName { get; set; }

            [JsonPropertyName("folder_name")]
            public string FolderName { get; set; }

            [JsonPropertyName("space_name")]
            public string SpaceName { get; set; }
        }

        public class TaskTag
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("tag_fg")]
            public string TagForeground { get; set; }

            [JsonPropertyName("tag_bg")]
            public string TagBackground { get; set; }

            [JsonPropertyName("creator")]
            public int Creator { get; set; }
        }

        // Convertidor personalizado para manejar "task" como string o como objeto
        public class TaskConverter : JsonConverter<Task>
        {
            public override Task Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String && reader.GetString() == "0")
                {
                    return null; // Devuelve null si es "0"
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    return JsonSerializer.Deserialize<Task>(ref reader, options); // Deserializa normalmente si es un objeto
                }

                throw new JsonException("Formato inesperado para 'task'");
            }

            public override void Write(Utf8JsonWriter writer, Task value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, options);
            }
        }
    }
}




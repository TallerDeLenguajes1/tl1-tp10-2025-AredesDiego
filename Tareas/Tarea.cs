using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Manejo.Tarea
{
    public class Tarea
    {
        [JsonPropertyName("userId")]
        public int userId { get; set; }

        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("completed")]
        public bool completed { get; set; }
    }
}

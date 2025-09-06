using System.Text.Json.Serialization;

namespace TokyoGarden.Api.DTOs
{
    public class KategoriaDTO
    {
        public int Id { get; set; }
        [JsonPropertyName("nazwaKategorii")]
        public string? NazwaKategorii { get; set; }
    }
}

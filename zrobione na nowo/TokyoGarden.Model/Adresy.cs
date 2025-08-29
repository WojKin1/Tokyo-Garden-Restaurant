using System.ComponentModel.DataAnnotations;

namespace TokyoGarden.Model
{
    public class Adresy
    {
        [Key] public int id { get; set; }
        public string? miasto { get; set; }
        public string? nr_domu { get; set; }
        public string? nr_mieszkania { get; set; }
        public string? ulica { get; set; }
    }
}

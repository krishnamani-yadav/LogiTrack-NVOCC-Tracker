using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LogiTrackApi.Models;

public class Milestone
{
    public int Id { get; set; }

    public int ShipmentId { get; set; }

    [Required]
    [MaxLength(100)]
    public string MilestoneName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public bool IsCompleted { get; set; } = false;

    [JsonIgnore]
    public Shipment? Shipment { get; set; }
}

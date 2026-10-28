using System.ComponentModel.DataAnnotations;

namespace LogiTrackApi.Models;

public class Shipment
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string ContainerNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string OriginPort { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string DestinationPort { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string VesselName { get; set; } = string.Empty;

    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string CurrentStatus { get; set; } = "Booked";

    public List<Milestone> Milestones { get; set; } = new();
}

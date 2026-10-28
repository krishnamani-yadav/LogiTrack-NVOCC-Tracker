using Microsoft.EntityFrameworkCore;
using LogiTrackApi.Models;

namespace LogiTrackApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<Milestone> Milestones => Set<Milestone>();

    public static void SeedData(AppDbContext context)
    {
        if (context.Shipments.Any())
        {
            return; // Data already seeded
        }

        var baseTime = DateTime.UtcNow;

        var shipment1 = new Shipment
        {
            ContainerNumber = "ECXU9021441",
            OriginPort = "Shanghai (CNSHA)",
            DestinationPort = "Los Angeles (USLAX)",
            VesselName = "EVER GIVEN / V.042W",
            BookingDate = baseTime.AddDays(-10),
            CurrentStatus = "In Transit",
            Milestones = new List<Milestone>
            {
                new Milestone
                {
                    MilestoneName = "Booked",
                    Location = "Shanghai Freight Center",
                    Timestamp = baseTime.AddDays(-10),
                    IsCompleted = true
                },
                new Milestone
                {
                    MilestoneName = "Gate In",
                    Location = "Yangshan Deepwater Port CY",
                    Timestamp = baseTime.AddDays(-8),
                    IsCompleted = true
                },
                new Milestone
                {
                    MilestoneName = "Loaded on Vessel",
                    Location = "Yangshan Berth 4",
                    Timestamp = baseTime.AddDays(-7),
                    IsCompleted = true
                },
                new Milestone
                {
                    MilestoneName = "In Transit",
                    Location = "North Pacific Route (34.2N, 168.4W)",
                    Timestamp = baseTime.AddDays(-2),
                    IsCompleted = true
                },
                new Milestone
                {
                    MilestoneName = "Customs Cleared",
                    Location = "Port of Los Angeles CBP Inspection",
                    Timestamp = baseTime.AddDays(3),
                    IsCompleted = false
                },
                new Milestone
                {
                    MilestoneName = "Delivered",
                    Location = "Inland Logistics Hub - Long Beach",
                    Timestamp = baseTime.AddDays(5),
                    IsCompleted = false
                }
            }
        };

        var shipment2 = new Shipment
        {
            ContainerNumber = "MSKU4820195",
            OriginPort = "Singapore (SGSIN)",
            DestinationPort = "Rotterdam (NLRTM)",
            VesselName = "MAERSK MC-KINNEY MOLLER / V.220E",
            BookingDate = baseTime.AddDays(-5),
            CurrentStatus = "Gate In",
            Milestones = new List<Milestone>
            {
                new Milestone
                {
                    MilestoneName = "Booked",
                    Location = "Singapore PSA Logistics Gate",
                    Timestamp = baseTime.AddDays(-5),
                    IsCompleted = true
                },
                new Milestone
                {
                    MilestoneName = "Gate In",
                    Location = "PSA Keppel Terminal Yard 12",
                    Timestamp = baseTime.AddDays(-2),
                    IsCompleted = true
                },
                new Milestone
                {
                    MilestoneName = "Loaded on Vessel",
                    Location = "Pasir Panjang Berth 14",
                    Timestamp = baseTime.AddDays(1),
                    IsCompleted = false
                },
                new Milestone
                {
                    MilestoneName = "In Transit",
                    Location = "Indian Ocean Route via Suez Canal",
                    Timestamp = baseTime.AddDays(12),
                    IsCompleted = false
                },
                new Milestone
                {
                    MilestoneName = "Customs Cleared",
                    Location = "APM Terminals Maasvlakte II",
                    Timestamp = baseTime.AddDays(22),
                    IsCompleted = false
                },
                new Milestone
                {
                    MilestoneName = "Delivered",
                    Location = "Rotterdam Europoort Distribution Hub",
                    Timestamp = baseTime.AddDays(25),
                    IsCompleted = false
                }
            }
        };

        context.Shipments.AddRange(shipment1, shipment2);
        context.SaveChanges();
    }
}

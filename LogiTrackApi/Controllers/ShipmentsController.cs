using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogiTrackApi.Data;
using LogiTrackApi.Models;
using System.ComponentModel.DataAnnotations;

namespace LogiTrackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ShipmentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/shipments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shipment>>> GetAllShipments()
    {
        var shipments = await _context.Shipments
            .Include(s => s.Milestones)
            .OrderByDescending(s => s.Id)
            .ToListAsync();

        return Ok(shipments);
    }

    // GET /api/shipments/track/{containerNumber}
    [HttpGet("track/{containerNumber}")]
    public async Task<ActionResult<Shipment>> Track(string containerNumber)
    {
        if (string.IsNullOrWhiteSpace(containerNumber))
        {
            return BadRequest(new { message = "Container number cannot be empty." });
        }

        var normalizedNumber = containerNumber.Trim().ToLowerInvariant();

        var shipment = await _context.Shipments
            .Include(s => s.Milestones)
            .FirstOrDefaultAsync(s => s.ContainerNumber.ToLower() == normalizedNumber);

        if (shipment == null)
        {
            return NotFound(new { message = $"Shipment with container number '{containerNumber}' was not found." });
        }

        // Return milestones in sequence
        shipment.Milestones = shipment.Milestones.OrderBy(m => m.Id).ToList();

        return Ok(shipment);
    }

    // POST /api/shipments
    [HttpPost]
    public async Task<ActionResult<Shipment>> CreateShipment([FromBody] CreateShipmentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cleanContainer = request.ContainerNumber.Trim().ToUpperInvariant();

        var exists = await _context.Shipments.AnyAsync(s => s.ContainerNumber == cleanContainer);
        if (exists)
        {
            return BadRequest(new { message = $"Container number '{cleanContainer}' is already registered." });
        }

        var now = DateTime.UtcNow;
        var shipment = new Shipment
        {
            ContainerNumber = cleanContainer,
            OriginPort = request.OriginPort.Trim(),
            DestinationPort = request.DestinationPort.Trim(),
            VesselName = request.VesselName.Trim(),
            BookingDate = now,
            CurrentStatus = "Booked",
            Milestones = new List<Milestone>
            {
                new Milestone
                {
                    MilestoneName = "Booked",
                    Location = $"{request.OriginPort.Trim()} Port Authority Hub",
                    Timestamp = now,
                    IsCompleted = true
                },
                new Milestone
                {
                    MilestoneName = "Gate In",
                    Location = $"{request.OriginPort.Trim()} Inbound Container Yard",
                    Timestamp = now.AddDays(2),
                    IsCompleted = false
                },
                new Milestone
                {
                    MilestoneName = "Loaded on Vessel",
                    Location = $"{request.OriginPort.Trim()} Deepwater Quay",
                    Timestamp = now.AddDays(4),
                    IsCompleted = false
                },
                new Milestone
                {
                    MilestoneName = "In Transit",
                    Location = $"International Maritime Route towards {request.DestinationPort.Trim()}",
                    Timestamp = now.AddDays(9),
                    IsCompleted = false
                },
                new Milestone
                {
                    MilestoneName = "Customs Cleared",
                    Location = $"{request.DestinationPort.Trim()} Clearance & Border Inspection",
                    Timestamp = now.AddDays(18),
                    IsCompleted = false
                },
                new Milestone
                {
                    MilestoneName = "Delivered",
                    Location = $"{request.DestinationPort.Trim()} Final Inland Destination Hub",
                    Timestamp = now.AddDays(21),
                    IsCompleted = false
                }
            }
        };

        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Track), new { containerNumber = shipment.ContainerNumber }, shipment);
    }

    // PUT /api/shipments/{id}/advance
    [HttpPut("{id}/advance")]
    public async Task<ActionResult<Shipment>> AdvanceMilestone(int id)
    {
        var shipment = await _context.Shipments
            .Include(s => s.Milestones)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
        {
            return NotFound(new { message = $"Shipment with ID {id} not found." });
        }

        var nextMilestone = shipment.Milestones
            .OrderBy(m => m.Id)
            .FirstOrDefault(m => !m.IsCompleted);

        if (nextMilestone == null)
        {
            return BadRequest(new { message = "Shipment has already completed all milestones ('Delivered')." });
        }

        nextMilestone.IsCompleted = true;
        nextMilestone.Timestamp = DateTime.UtcNow;
        shipment.CurrentStatus = nextMilestone.MilestoneName;

        await _context.SaveChangesAsync();

        shipment.Milestones = shipment.Milestones.OrderBy(m => m.Id).ToList();

        return Ok(shipment);
    }
}

public class CreateShipmentRequest
{
    [Required(ErrorMessage = "Container number is required")]
    [MaxLength(20)]
    public string ContainerNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Origin port is required")]
    [MaxLength(100)]
    public string OriginPort { get; set; } = string.Empty;

    [Required(ErrorMessage = "Destination port is required")]
    [MaxLength(100)]
    public string DestinationPort { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vessel name is required")]
    [MaxLength(100)]
    public string VesselName { get; set; } = string.Empty;
}

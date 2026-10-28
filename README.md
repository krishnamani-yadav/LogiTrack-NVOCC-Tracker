# 🚢 LogiTrack — NVOCC Container Shipment & Milestone Tracker

A real-time Non-Vessel Operating Common Carrier (NVOCC) shipment and container milestone tracking web application built with **ASP.NET Core Web API**, **Entity Framework Core (In-Memory)**, and a modern **Bootstrap 5** user interface.

---

## 📌 Features

- **Container & Bill of Lading (B/L) Tracking:** Search and fetch real-time operational status by Container ID or B/L number.
- **Visual Milestone Progression:** Interactive multi-stage tracking stepper (Gate-In, Loaded, Vessel Departure, Transshipment, Discharged, Gate-Out/Delivered).
- **RESTful Endpoints:** Full CRUD architecture using ASP.NET Core controllers to view, book, and update shipments.
- **Lightweight Architecture:** Zero-dependency in-memory database powered by Entity Framework Core In-Memory provider for immediate testing.
- **Self-Contained Frontend:** Embedded single-page interface served directly through static file middleware (`wwwroot`).

---

## 🛠️ Tech Stack

- **Backend:** C# / .NET 8.0, ASP.NET Core Web API
- **Data Layer:** Entity Framework Core 8.0 (In-Memory Store)
- **Frontend:** HTML5, CSS3, JavaScript (Fetch API), Bootstrap 5
- **Tooling:** Visual Studio Code, Git

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher

### Installation & Run

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/krishnamani-yadav/LogiTrack-NVOCC-Tracker.git](https://github.com/krishnamani-yadav/LogiTrack-NVOCC-Tracker.git)
   cd LogiTrack-NVOCC-Tracker/LogiTrackApi
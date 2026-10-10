using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Vehicles.Core.Models;

namespace Vehicles.Core;

public class Garage
{
    public string Name { get; }

    public ObservableCollection<Vehicle> Vehicles { get; } = new();

    public Garage(string name)
    {
        Name = name;
    }

    public void AddVehicle(Vehicle vehicle)
    {
        foreach (Vehicle otherVehicle in Vehicles)
        {
            otherVehicle.AddLogEntry($"Signaal: {vehicle.Make} {vehicle.Model} lisati garaaži");

            _ = otherVehicle.FlashLightsAsync();
        }

        Vehicles.Add(vehicle);

        vehicle.AddLogEntry($"Sõiduk lisati garaaži: {Name}");
    }

    public void RemoveVehicle(Vehicle vehicle)
    {
        if (!Vehicles.Remove(vehicle))
        {
            return;
        }
            
        foreach (Vehicle otherVehicle in Vehicles)
        {
            otherVehicle.AddLogEntry($"Tuled vilguvad: {vehicle.Make} {vehicle.Model} eemaldati garaažist");

            _ = otherVehicle.FlashLightsAsync();
        }
    }
}

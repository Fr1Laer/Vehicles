using System;
using System.Collections.ObjectModel;
using Vehicles.Core.Models;

namespace Vehicles.Core;

public class VehicleManager
{
    public ObservableCollection<Vehicle> Vehicles { get; } = new();

    public void AddVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
        {
            throw new ArgumentNullException(nameof(vehicle));
        }

        Vehicles.Add(vehicle);
    }

    public void RemoveVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
        { 
            throw new ArgumentNullException(nameof(vehicle));
        }

        Vehicles.Remove(vehicle);
    }

    public string MoveVehicle(Vehicle vehicle, double km)
    {
        if (vehicle == null)
        { 
            throw new ArgumentNullException(nameof(vehicle));
        }

        return vehicle.Move(km);
    }
}
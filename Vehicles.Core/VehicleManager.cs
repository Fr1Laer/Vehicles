using System;
using System.Buffers.Binary;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Vehicles.Core.Models;

namespace Vehicles.Core;

public class VehicleManager
{
    // CarCarage and BoatGarage properties
    public Garage CarGarage { get; } = new("Autode garaaž");

    public Garage BoatGarage { get; } = new("Paatide garaaž");

    public ObservableCollection<Vehicle> Vehicles { get; } = new();

    // updated AddVehicle function
    public void AddVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
        {
            throw new ArgumentNullException(nameof(vehicle));
        }

        // adding cars and boats to garage
        if (vehicle is Car || vehicle is AmphibiousCar)
        {
            CarGarage.AddVehicle(vehicle);
        }
        else if (vehicle is Boat)
        {
            BoatGarage.AddVehicle(vehicle);
        }

        Vehicles.Add(vehicle);
    }

    // updated RemoveVehicle function
    public void RemoveVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
        { 
            throw new ArgumentNullException(nameof(vehicle));
        }

        if (CarGarage.Vehicles.Contains(vehicle))
        {
            CarGarage.RemoveVehicle(vehicle);
        }
        else if (BoatGarage.Vehicles.Contains(vehicle))
        {
            BoatGarage.RemoveVehicle(vehicle);
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
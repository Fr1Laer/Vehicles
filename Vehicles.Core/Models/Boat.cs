using Vehicles.Core.Interfaces;
using Vehicles.Core.Properties;

namespace Vehicles.Core.Models;

public class Boat : Vehicle, ISwimmable
{
    public Boat(string make, string model)
        : base(make, model)
    {
    }

    public override string Move(double km)
    {
        return Swim(km);
    }

    public string Swim(double km)
    {
        ValidateDistance(km);

        Odometer += km;

        string message = string.Format(
            Resources.VehicleSwam,
            Make,
            Model,
            km);

        AddLog(message);

        return message;
    }
}
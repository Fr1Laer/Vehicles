using Vehicles.Core.Interfaces;
using Vehicles.Core.Properties;

namespace Vehicles.Core.Models;

public class AmphibiousCar : Vehicle, IDriveable, ISwimmable
{
    public AmphibiousCar(string make, string model)
        : base(make, model)
    {
    }

    public override string Move(double km)
    {
        ValidateDistance(km);

        Odometer += km;

        string message = string.Format(
            Resources.VehicleMoved,
            Make,
            Model,
            km);

        AddLog(message);

        return message;
    }

    public string Drive(double km)
    {
        ValidateDistance(km);

        Odometer += km;

        string message = string.Format(
            Resources.VehicleMoved,
            Make,
            Model,
            km);

        AddLog(message);

        return message;
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
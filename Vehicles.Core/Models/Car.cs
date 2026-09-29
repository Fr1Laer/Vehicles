using Vehicles.Core.Interfaces;
using Vehicles.Core.Properties;

namespace Vehicles.Core.Models;

public class Car : Vehicle, IDriveable
{
    public Car(string make, string model)
        : base(make, model)
    {
    }

    public override string Move(double km)
    {
        return Drive(km);
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
}
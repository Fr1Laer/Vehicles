using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Vehicles.Core.Properties;

namespace Vehicles.Core.Models;

public abstract class Vehicle : INotifyPropertyChanged
{
    public string Make { get; }

    public string Model { get; }

    private double _odometer;

    public double Odometer
    {
        get => _odometer;
        protected set
        {
            if (_odometer == value)
                return;

            _odometer = value;
            OnPropertyChanged();
        }
    }

    public string VehicleType => GetType().Name;

    public ObservableCollection<string> Log { get; } = new();

    protected Vehicle(string make, string model)
    {
        if (string.IsNullOrWhiteSpace(make))
        {
            throw new ArgumentException(Resources.InvalidMake);
        }

        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException(Resources.InvalidModel);
        }

        Make = make.Trim();
        Model = model.Trim();
    }

    public abstract string Move(double km);

    protected void AddLog(string message)
    {
        Log.Add($"{DateTime.Now:HH:mm:ss} - {message}");
    }

    protected static void ValidateDistance(double km)
    {
        if (km <= 0)
        {
            throw new ArgumentException(Resources.InvalidDistance);
        }
            
    }

    // Odometer-väärtuse automaatne uuendamine DataGridis
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vehicles.Core;
using Vehicles.Core.Interfaces;
using Vehicles.Core.Models;

namespace Vehicles.WpfApp;

public partial class MainWindow : Window
{
    private readonly VehicleManager _vehicleManager = new();

    public MainWindow()
    {
        InitializeComponent();

        VehicleTypeComboBox.ItemsSource = new[]
        {
            "Car",
            "Boat",
            "AmphibiousCar"
        };

        VehicleTypeComboBox.SelectedIndex = 0;

        VehicleDataGrid.ItemsSource = _vehicleManager.Vehicles;

        UpdateButtons();
    }

    private void AddVehicle_Click(object sender, RoutedEventArgs e)
    {
        string make = MakeTextBox.Text.Trim();
        string model = ModelTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(make))
        {
            MessageBox.Show(
                Vehicles.Core.Properties.Resources.InvalidMake,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            MakeTextBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(model))
        {
            MessageBox.Show(
                Vehicles.Core.Properties.Resources.InvalidModel,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            ModelTextBox.Focus();
            return;
        }

        // tühi muutuja tulevase transpordi jaoks 
        Vehicle? vehicle = null;

        // transpordiliigi valik
        switch (VehicleTypeComboBox.SelectedItem?.ToString())
        {
            case "Car":
                vehicle = new Car(make, model);
                break;

            case "Boat":
                vehicle = new Boat(make, model);
                break;

            case "AmphibiousCar":
                vehicle = new AmphibiousCar(make, model);
                break;
        }

        if (vehicle == null)
            return;

        try
        {
            _vehicleManager.AddVehicle(vehicle);

            MakeTextBox.Clear();
            ModelTextBox.Clear();

            VehicleTypeComboBox.SelectedIndex = 0;

            VehicleDataGrid.SelectedItem = vehicle;
            VehicleDataGrid.ScrollIntoView(vehicle);

            UpdateButtons();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private void Move_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleDataGrid.SelectedItem is not Vehicle vehicle)
        {
            ShowVehicleRequired();
            return;
        }

        if (!TryGetDistance(out double km))
            return;

        try
        {
            string message = _vehicleManager.MoveVehicle(vehicle, km);

            RefreshSelectedVehicle();

            MessageBox.Show(
                message,
                "Liikumine",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private void Drive_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleDataGrid.SelectedItem is not Vehicle vehicle)
        {
            ShowVehicleRequired();
            return;
        }

        if (vehicle is not IDriveable driveable)
            return;

        if (!TryGetDistance(out double km))
            return;

        try
        {
            string message = driveable.Drive(km);

            RefreshSelectedVehicle();

            MessageBox.Show(
                message,
                "Sõitmine",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private void Swim_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleDataGrid.SelectedItem is not Vehicle vehicle)
        {
            ShowVehicleRequired();
            return;
        }

        if (vehicle is not ISwimmable swimmable)
            return;

        if (!TryGetDistance(out double km))
            return;

        try
        {
            string message = swimmable.Swim(km);

            RefreshSelectedVehicle();

            MessageBox.Show(
                message,
                "Ujumine",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private void DeleteVehicle_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleDataGrid.SelectedItem is not Vehicle vehicle)
        {
            MessageBox.Show(
                "Vali tabelist sõiduk, mida kustutada!",
                "Teavitus",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            return;
        }

        MessageBoxResult result = MessageBox.Show(
            $"Kas soovid sõiduki {vehicle.Make} {vehicle.Model} kustutada?",
            "Sõiduki kustutamine",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        _vehicleManager.RemoveVehicle(vehicle);

        VehicleDataGrid.SelectedItem = null;

        RefreshSelectedVehicle();
        UpdateButtons();
    }

    private void VehicleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshSelectedVehicle();
        UpdateButtons();
    }

    private bool TryGetDistance(out double km)
    {
        km = 0;

        string text = DistanceTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(text))
        {
            MessageBox.Show(
                Vehicles.Core.Properties.Resources.InvalidNumber,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            DistanceTextBox.Focus();
            return false;
        }

        if (!double.TryParse(
                text,
                NumberStyles.Float,
                CultureInfo.CurrentCulture,
                out km))
        {
            MessageBox.Show(
                Vehicles.Core.Properties.Resources.InvalidNumber,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            DistanceTextBox.SelectAll();
            DistanceTextBox.Focus();
            return false;
        }

        if (km <= 0)
        {
            MessageBox.Show(
                Vehicles.Core.Properties.Resources.InvalidDistance,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            DistanceTextBox.SelectAll();
            DistanceTextBox.Focus();
            return false;
        }

        return true;
    }

    private void RefreshSelectedVehicle()
    {
        if (VehicleDataGrid.SelectedItem is not Vehicle vehicle)
        {
            SelectedVehicleTextBlock.Text = "Sõidukit ei ole valitud.";

            VehicleLogListBox.ItemsSource = null;

            return;
        }

        SelectedVehicleTextBlock.Text =
            $"Automark: {vehicle.Make}\n" +
            $"Mudel: {vehicle.Model}\n" +
            $"Transpordiliik: {vehicle.VehicleType}\n" +
            $"Läbitud: {vehicle.Odometer:F1} km";

        VehicleLogListBox.ItemsSource = vehicle.Log;
    }

    private void UpdateButtons()
    {
        if (VehicleDataGrid.SelectedItem is not Vehicle vehicle)
        {
            MoveButton.IsEnabled = false;
            DriveButton.IsEnabled = false;
            SwimButton.IsEnabled = false;

            return;
        }

        MoveButton.IsEnabled = true;
        DriveButton.IsEnabled = vehicle is IDriveable;
        SwimButton.IsEnabled = vehicle is ISwimmable;
    }

    private void ShowVehicleRequired()
    {
        MessageBox.Show(
            Vehicles.Core.Properties.Resources.VehicleRequired,
            "Teavitus",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    // sisestada tohib ainult tähti, tühikuid, sidekriipse ja numbreid. Ülejäänud märgid blokeeritakse
    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        foreach (char character in e.Text)
        {
            if (!char.IsLetterOrDigit(character) &&
                character != ' ' &&
                character != '-')
            {
                e.Handled = true;
                return;
            }
        }
    }
}
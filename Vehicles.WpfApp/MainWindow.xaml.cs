using System;
using System.Globalization;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vehicles.Core;
using Vehicles.Core.Interfaces;
using Vehicles.Core.Models;

namespace Vehicles.WpfApp;

public partial class MainWindow : Window
{
    private readonly VehicleManager _vehicleManager = new();

    // new variables _selectedVehicle and _changingSelection
    private Vehicle? _selectedVehicle;

    private bool _changingSelection;

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

        // CarGarageDataGrid and BoatGarageDataGrid

        CarGarageDataGrid.ItemsSource = _vehicleManager.CarGarage.Vehicles;

        BoatGarageDataGrid.ItemsSource = _vehicleManager.BoatGarage.Vehicles;

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

            SelectVehicle(vehicle);

            if (vehicle is Boat)
            {
                BoatGarageDataGrid.ScrollIntoView(vehicle);
            }
            else
            {
                CarGarageDataGrid.ScrollIntoView(vehicle);
            }    

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
        if (_selectedVehicle == null)
        {
            ShowVehicleRequired();
            return;
        }

        if (!TryGetDistance(out double km))
            return;

        try
        {
            string message = _vehicleManager.MoveVehicle(_selectedVehicle, km);

            DistanceTextBox.Clear();

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
        if (_selectedVehicle == null)
        {
            ShowVehicleRequired();
            return;
        }

        if (_selectedVehicle is not IDriveable driveable)
            return;

        if (!TryGetDistance(out double km))
            return;

        try
        {
            string message = driveable.Drive(km);

            DistanceTextBox.Clear();

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
        if (_selectedVehicle == null)
        {
            ShowVehicleRequired();
            return;
        }

        if (_selectedVehicle is not ISwimmable swimmable)
            return;

        if (!TryGetDistance(out double km))
            return;

        try
        {
            string message = swimmable.Swim(km);

            DistanceTextBox.Clear();

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
        if (_selectedVehicle == null)
        {
            ShowVehicleRequired();
            return;
        }

        Vehicle vehicle = _selectedVehicle;

        MessageBoxResult result = MessageBox.Show(
            $"Kas soovid sõiduki {vehicle.Make} {vehicle.Model} kustutada?",
            "Sõiduki kustutamine",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        _vehicleManager.RemoveVehicle(vehicle);

        ClearSelection();
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
        if (_selectedVehicle == null)
        {
            SelectedVehicleTextBlock.Text = "Sõidukit ei ole valitud.";

            VehicleLogListBox.ItemsSource = null;

            return;
        }

        SelectedVehicleTextBlock.Text =
            $"Automark: {_selectedVehicle.Make}\n" +
            $"Mudel: {_selectedVehicle.Model}\n" +
            $"Transpordiliik: {_selectedVehicle.VehicleType}\n" +
            $"Läbitud: {_selectedVehicle.Odometer:F1} km";

        VehicleLogListBox.ItemsSource = _selectedVehicle.Log;
    }

    private void UpdateButtons()
    {
        if (_selectedVehicle == null)
        {
            MoveButton.IsEnabled = false;
            DriveButton.IsEnabled = false;
            SwimButton.IsEnabled = false;

            return;
        }

        MoveButton.IsEnabled = true;
        DriveButton.IsEnabled = _selectedVehicle is IDriveable;
        SwimButton.IsEnabled = _selectedVehicle is ISwimmable;
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

    private void CarGarageDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_changingSelection)
            return;

        Vehicle? vehicle = CarGarageDataGrid.SelectedItem as Vehicle;

        SelectVehicleFromGrid(vehicle, CarGarageDataGrid);
    }

    private void BoatGarageDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_changingSelection)
            return;

        Vehicle? vehicle = BoatGarageDataGrid.SelectedItem as Vehicle;

        SelectVehicleFromGrid(vehicle, BoatGarageDataGrid);
    }

    private void SelectVehicleFromGrid(Vehicle? vehicle, DataGrid sourceGrid)
    {
        _changingSelection = true;

        if (sourceGrid == CarGarageDataGrid)
        {
            BoatGarageDataGrid.SelectedItem = null; 
        }
        else
        {
            CarGarageDataGrid.SelectedItem = null;
        }

        _changingSelection = false;

        _selectedVehicle = vehicle;

        RefreshSelectedVehicle();
        UpdateButtons();    
    }

    private void SelectVehicle(Vehicle vehicle)
    {
        _changingSelection = true;

        if (vehicle is Boat)
        {
            CarGarageDataGrid.SelectedItem = null;
            BoatGarageDataGrid.SelectedItem = vehicle;
        }
        else
        {
            CarGarageDataGrid.SelectedItem = vehicle;
            BoatGarageDataGrid.SelectedItem = null;
        }

        _changingSelection = false;

        _selectedVehicle = vehicle;

        RefreshSelectedVehicle();
        UpdateButtons();
    }

    private void ClearSelection()
    {
        _changingSelection = true;

        CarGarageDataGrid.SelectedItem = null;
        BoatGarageDataGrid.SelectedItem = null;

        _changingSelection = false;

        _selectedVehicle = null;

        RefreshSelectedVehicle();
        UpdateButtons();
    }

}
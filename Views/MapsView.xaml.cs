using Microsoft.Maui.Maps;
using TasksManagementApp.Models;
using TasksManagementApp.ViewModels;
using Microsoft.Maui.Controls.Maps;
using TasksManagementApp.Helpers;

namespace TasksManagementApp.Views;


public partial class MapsView : ContentPage
{
	public MapsView(MapsViewModel vm)
	{
        vm.OnUpdateMapEvent += OnUpdateMap;
        this.BindingContext = vm;
		InitializeComponent();
    }

    public void OnUpdateMap()
    {
#if ANDROID
        //Clear all routes and pins from the map
        map.MapElements.Clear();

        MapsViewModel vm = (MapsViewModel)this.BindingContext;

        //Create two pins for origin and destination and add them to the map
        Pin pin1 = new Pin
        {
            Type = PinType.Place,
            Location = new Location(vm.RouteOrigin.Latitude, vm.RouteOrigin.Longitude),
            Label = vm.RouteOrigin.Name,
            Address = ""
        };
        map.Pins.Add(pin1);
        Pin pin2 = new Pin
        {
            Type = PinType.Place,
            Location = new Location(vm.RouteDestination.Latitude, vm.RouteDestination.Longitude),
            Label = vm.RouteDestination.Name,
            Address = ""
        };
        map.Pins.Add(pin2);

        //Move the map to show the environment of the origin place! with radius of 5 KM... should be changed
        //according to the specific needs
        MapSpan span = MapSpan.FromCenterAndRadius(pin1.Location, Distance.FromKilometers(5));
        map.MoveToRegion(span);

        //Create the polyline between origin and destination
        GoogleDirection directions = vm.RouteDirections;
        Microsoft.Maui.Controls.Maps.Polyline path = new Microsoft.Maui.Controls.Maps.Polyline()
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 15
        };
        //run through each leg of the route, then, through each step
        foreach (Leg leg in directions.Routes[0].Legs)
        {
            foreach (Step step in leg.Steps)
            {
                var p = step.Polyline;
                //Decode all positions of the line in this specific step!
                IEnumerable<Location> positions = PolylineHelper.Decode(p.Points);

                //Add the positions to the line
                foreach (Location pos in positions)
                {
                    path.Geopath.Add(pos);
                }

            }
        }
        //Add the line to the map!
        map.MapElements.Add(path);
#endif
    }

    /*public async Task<bool> RequestLocationPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        return status == PermissionStatus.Granted;
    }

    private async void InitializeMap()
    {
        if (await RequestLocationPermissionAsync())
        {
            map.IsShowingUser = true;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(37.7749, -122.4194), Distance.FromMiles(1)));
        }
        else
        {
            // Handle permission denied
        }
    }*/
}
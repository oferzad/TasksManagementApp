using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;
using TasksManagementApp.Models;
namespace TasksManagementApp.Services;
public class GeocodingService
{
    public static async Task<AddressComponents> GetAddressComponentsAsync(string address)
    {
        IEnumerable<Location> locations = await Geocoding.GetLocationsAsync(address);
        Location? location = locations?.FirstOrDefault();

        if (location == null)
        {
            return null;
        }

        IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
        Placemark? placemark = placemarks?.FirstOrDefault();

        if (placemark == null)
        {
            return null;
        }

        return new AddressComponents
        {
            Street = placemark.Thoroughfare,
            Number = placemark.SubThoroughfare,
            City = placemark.Locality,
            State = placemark.AdminArea,
            Country = placemark.CountryName
        };
    }
    
    public static async Task<Location> GetCurrentLocation()
    {
        try
        {

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            Location? location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            return location;
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
            return null;
        }
    }

    public static async Task<AddressComponents> GetLocationAddress(Location location)
    {
        IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
        Placemark? placemark = placemarks?.FirstOrDefault();

        if (placemark == null)
        {
            return null;
        }

        return new AddressComponents
        {
            Street = placemark.Thoroughfare,
            Number = placemark.SubThoroughfare,
            City = placemark.Locality,
            State = placemark.AdminArea,
            Country = placemark.CountryName
        };
    }

    //This method gets two pairs of latitude and longitude and returns the distance between them in kilometers
    public static double DistanceBetweenTwoLocations(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6371; // Radius of the earth in km
        var dLat = Deg2Rad(lat2 - lat1);  // deg2rad below
        var dLon = Deg2Rad(lon2 - lon1);
        var a =
          Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
          Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
          Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
          ;
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c; // Distance in km
        return d;
    }

    //this method converts degrees to radians
    private static double Deg2Rad(double deg)
    {
        return deg * (Math.PI / 180);
    }

}


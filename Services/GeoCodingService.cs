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


}


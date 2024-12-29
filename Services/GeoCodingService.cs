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
}


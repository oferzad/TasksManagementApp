using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



#if ANDROID
using Android.Content.PM;
using Android.OS;
#endif

namespace TasksManagementApp.Services;

    public class GoogleMapsApiService
    {
        static string _googleMapsKey;

        private const string ApiBaseAddress = "https://maps.googleapis.com/maps/";
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
        public static void Initialize()
        {
        _googleMapsKey =  GetGoogleMapsApiKey();
        }

        private static string GetGoogleMapsApiKey()
        {
#if ANDROID
            ApplicationInfo appInfo = Android.App.Application.Context.PackageManager.GetApplicationInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.MetaData);
            Bundle bundle = appInfo.MetaData;
            return bundle.GetString("com.google.android.geo.API_KEY");

#else
            return "PLACE HERE YOUR API KEY";
#endif
    }
    public async Task<GoogleDirection> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude)
        {
            GoogleDirection googleDirection = new GoogleDirection();

            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={originLatitude},{originLongitude}&destination={destinationLatitude},{destinationLongitude}&key={_googleMapsKey}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        googleDirection = JsonConvert.DeserializeObject<GoogleDirection>(json);

                    }
                }
            }

            return googleDirection;
        }

        public async Task<GooglePlaceAutoCompleteResult> GetPlaces(string text)
        {
            GooglePlaceAutoCompleteResult results = null;

            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/autocomplete/json?input={Uri.EscapeUriString(text)}&key={_googleMapsKey}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        results = JsonConvert.DeserializeObject<GooglePlaceAutoCompleteResult>(json);


                    }
                }
            }

            return results;
        }

    
    public async Task<GooglePlace> GetPlaceDetails(string placeId)
        {
            GooglePlace result = null;
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/details/json?placeid={Uri.EscapeUriString(placeId)}&key={_googleMapsKey}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        result = new GooglePlace(JObject.Parse(json));
                    }
                }
            }

            return result;
        }

    public async Task<AddressComponents> GetAddressComponentsAsync(string address)
    {
        using var httpClient = new HttpClient();
        var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_googleMapsKey}";
        var response = await httpClient.GetStringAsync(url);
        var json = JObject.Parse(response);

        if (json["status"].ToString() != "OK")
        {
            return null;
        }

        var result = json["results"][0];
        var components = new AddressComponents();

        foreach (var component in result["address_components"])
        {
            var types = component["types"].ToObject<string[]>();
            if (types.Contains("street_number"))
            {
                components.Number = component["long_name"].ToString();
            }
            else if (types.Contains("route"))
            {
                components.Street = component["long_name"].ToString();
            }
            else if (types.Contains("locality"))
            {
                components.City = component["long_name"].ToString();
            }
            else if (types.Contains("country"))
            {
                components.Country = component["long_name"].ToString();
            }
        }

        return components;
    }
}

   

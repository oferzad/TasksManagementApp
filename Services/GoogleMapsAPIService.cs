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
            _googleMapsKey = GetGoogleMapsApiKey();
        }

        private static string GetGoogleMapsApiKey()
        {
#if ANDROID
            ApplicationInfo appInfo = Android.App.Application.Context.PackageManager.GetApplicationInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.MetaData);
            Bundle bundle = appInfo.MetaData;
            return bundle.GetString("com.google.android.geo.API_KEY");

#else
            return "AIzaSyCz8nXSYXgR0n"+"fzko2h6dJlvtYt3M0LFwM";
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
    }


1. Full code exmaples and classes can be found in https://github.com/ericnewton76/gmaps-api-net
2. You have to get Google Maps API KEY from https://console.cloud.google.com/
3. Copy and paste your google maps API KEY into the android manifest.xml! Do not keep it hard coded in the code.
4. To use Google places only (for address verification and places search), you need to copy GooglePLaces.cs  into your project.
5. To use map control (works only for android)  and directions, you need to copy GoogleDirections.cs into your project and add UseMaps to your app builder in mauiProgram class.
6. For all cases you need to copy GoogleMapsAPIService.cs into your project and you should initialize it (See MauiProgram.cs)


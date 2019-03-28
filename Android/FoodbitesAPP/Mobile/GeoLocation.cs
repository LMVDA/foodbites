using System;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace FoodbitesAPP
{
    public static class Coordenadas {
        public static double Latitude { get; set; } = 0;
        public static double Longitude { get; set; } = 0;
        public static GeoLocation Locator;
    }

	//var locator2 = CrossGeolocator.Current;
	//locator2.DesiredAccuracy = 100;
	//var position2 = await locator2.GetPositionAsync(10000); //timeout = 10 seg
	//Console.WriteLine("Position Status: {0}", position2.Timestamp);
	//Console.WriteLine("Position Latitude: {0}", position2.Latitude);
	//Console.WriteLine("Position Longitude: {0}", position2.Longitude);


	// ver: https://github.com/xamarin/recipes/blob/master/android/os_device_resources/gps/get_current_device_location/Activity1.cs
	// ver: https://developer.xamarin.com/guides/android/platform_features/maps_and_location/location/
	// ver: https://developer.xamarin.com/recipes/android/os_device_resources/gps/get_current_device_location/

	public class GeoLocation : Java.Lang.Object, ILocationListener
	{

		LocationManager locMgr;
		string tag = "GeoLocation";

        public GeoLocation(Context ctx) {
            // initialize location manager
            locMgr = ctx.GetSystemService (Context.LocationService) as LocationManager;
            Coordenadas.Locator = this;
        }

        public void UpdateLocation() {
            // pass in the provider (GPS), 
            // the minimum time between updates (in seconds), 
            // the minimum distance the user needs to move to generate an update (in meters),
            // and an ILocationListener (recall that this class impletents the ILocationListener interface)

            //if (locMgr.AllProviders.Contains(LocationManager.NetworkProvider)
            //    && locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
            //{
            //    locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 3, 1, this);
            //}
            //else
            //{
            //    Log.Debug(tag, "The Network Provider does not exist or is not enabled!");
            //}


            // Comment the line above, and uncomment the following, to test 
            // the GetBestProvider option. This will determine the best provider
            // at application launch. Note that once the provide has been set
            // it will stay the same until the next time this method is called

            var locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Medium;
            string locationProvider = locMgr.GetBestProvider(locationCriteria, true);
            Log.Debug(tag, "Starting location updates with " + locationProvider.ToString());
            locMgr.RequestLocationUpdates (locationProvider, 2000, 1, this);
        }

        public void OnLocationChanged(Location location)
        {
			Log.Debug(tag, " Latitude: " + location.Latitude);
            Log.Debug(tag, " Longitude: " + location.Longitude);
            Coordenadas.Latitude = location.Latitude;
            Coordenadas.Longitude = location.Longitude;
        }

        public void OnProviderDisabled(string provider)
		{
			Log.Debug(tag, provider + " disabled by user");
        }

        public void OnProviderEnabled(string provider)
		{
			Log.Debug(tag, provider + " enabled by user");
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
		{
			Log.Debug(tag, provider + " availability has changed to " + status.ToString());
        }
    }
}

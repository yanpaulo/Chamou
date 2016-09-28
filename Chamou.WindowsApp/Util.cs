using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Chamou.WindowsApp
{
    public static class Util
    {
        public static async Task<BasicGeoposition> RequestGeopositionAsync()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                //Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 1 };
                Geolocator geolocator = new Geolocator();

                // Carry out the operation
                Geoposition pos = await geolocator.GetGeopositionAsync();
                return pos.Coordinate.Point.Position;
            }

            throw new InvalidOperationException("Access to Geoposition not allowed");
        }
    }
}

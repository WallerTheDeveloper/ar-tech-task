using System;
using System.Collections;
using CesiumForUnity;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace Services
{
    public class NavigationCalculationService
    {
        public IEnumerator CalculateEverySeconds(float seconds, AREarthManager earthManager, CesiumGeoreference cesiumGeoreference)
        {
            while (true)
            {
                CalculateDistance(earthManager, cesiumGeoreference);
                yield return new WaitForSeconds(seconds);
            }
        }
        public double CalculateDistance(AREarthManager earthManager, CesiumGeoreference cesiumGeoreference)
        {
            var pose = earthManager.EarthState == EarthState.Enabled &&
                       earthManager.EarthTrackingState == TrackingState.Tracking ?
                earthManager.CameraGeospatialPose : new GeospatialPose();
            
            var distance = Haversine(pose.Latitude, pose.Longitude, cesiumGeoreference.latitude, cesiumGeoreference.longitude);
            
            return distance;
        }
        private double Haversine(double initialLatitude, double initialLongitude, double targetLatitude, double targetLongitude) 
        {
            int earthRadius = 6371;
            
            double dLat = ConvertToRadians(targetLatitude - initialLatitude);
            double dLon = ConvertToRadians(targetLongitude - initialLongitude);
            
            initialLatitude = ConvertToRadians(initialLatitude);
            targetLatitude = ConvertToRadians(targetLatitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(initialLatitude) * Math.Cos(targetLatitude);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            
            return earthRadius * c;
        }

        private static double ConvertToRadians(double angle) 
        {
            return Math.PI * angle / 180.0;
        }
    }
}
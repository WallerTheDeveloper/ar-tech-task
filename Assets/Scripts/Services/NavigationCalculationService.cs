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
        public IEnumerator CalculateEverySeconds(float seconds, GeospatialPose pose, double targetLatitude, double targetLongitude)
        {
            while (true)
            {
                CalculateDistance(pose, targetLatitude, targetLongitude);
                yield return new WaitForSeconds(seconds);
            }
        }
        public double CalculateDistance(GeospatialPose pose, double targetLatitude, double targetLongitude)
        {
            var distance = Haversine(pose.Latitude, pose.Longitude, targetLatitude, targetLongitude);
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
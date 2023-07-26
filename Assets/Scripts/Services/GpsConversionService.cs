using UnityEngine;

namespace Services
{
    public class GpsConversionService
    {
        private readonly UserLocationService _userLocationService;
        private float metersPerLat;
        private float metersPerLon;

        public GpsConversionService(UserLocationService userLocationService)
        {
            _userLocationService = userLocationService;
        }
        public Vector3 ConvertGPStoUCS(double latitude, double longitude)
        {
            double userLatitude = _userLocationService.GetUserLocation().Latitude;
            double userLongitude = _userLocationService.GetUserLocation().Longitude;
            
            FindMetersPerLat(_userLocationService.GetUserLocation().Latitude);
  
#if UNITY_EDITOR
            userLatitude = 52.5162994656517;
            userLongitude = 13.4712489301791;
#endif
            
            double zPosition  = metersPerLat * (latitude - userLatitude); //Calc current lat
            double xPosition  = metersPerLon * (longitude- userLongitude); //Calc current lon
            return new Vector3((float)xPosition, 0, (float)zPosition);
        }
        private void FindMetersPerLat(double lat) // Compute lengths of degrees
        {
            float m1 = 111132.92f;    // latitude calculation term 1
            float m2 = -559.82f;        // latitude calculation term 2
            float m3 = 1.175f;      // latitude calculation term 3
            float m4 = -0.0023f;        // latitude calculation term 4
            float p1 = 111412.84f;    // longitude calculation term 1
            float p2 = -93.5f;      // longitude calculation term 2
            float p3 = 0.118f;      // longitude calculation term 3
	    
            lat *= Mathf.Deg2Rad;
	
            // Calculate the length of a degree of latitude and longitude in meters
            metersPerLat = m1 + (m2 * Mathf.Cos(2 * (float)lat)) + (m3 * Mathf.Cos(4 * (float)lat)) + (m4 * Mathf.Cos(6 * (float)lat));
            metersPerLon = (p1 * Mathf.Cos((float)lat)) + (p2 * Mathf.Cos(3 * (float)lat)) + (p3 * Mathf.Cos(5 * (float)lat));	   
        }
    }
}
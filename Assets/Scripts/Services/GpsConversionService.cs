using UnityEngine;

namespace Services
{
    public class GpsConversionService : IConververtor
    {
        private readonly ILocationService _userLocationService;
        private float metersPerLat;
        private float metersPerLon;

        public GpsConversionService(ILocationService userLocationService)
        {
            _userLocationService = userLocationService;
        }
        public Vector3 ConvertGPStoUCS(double latitude, double longitude)
        {
            double userLatitude = _userLocationService.GetUserLocation().Latitude;
            double userLongitude = _userLocationService.GetUserLocation().Longitude;
            
            FindMetersPerLat(_userLocationService.GetUserLocation().Latitude);

            double zPosition  = metersPerLat * (latitude - userLatitude);
            double xPosition  = metersPerLon * (longitude- userLongitude);
            return new Vector3((float)xPosition, 0, (float)zPosition);
        }
        private void FindMetersPerLat(double lat)
        {
            float m1 = 111132.92f;  
            float m2 = -559.82f;    
            float m3 = 1.175f;      
            float m4 = -0.0023f;    
            float p1 = 111412.84f;  
            float p2 = -93.5f;      
            float p3 = 0.118f;      
	    
            lat *= Mathf.Deg2Rad;
	
            metersPerLat = m1 + (m2 * Mathf.Cos(2 * (float)lat)) + (m3 * Mathf.Cos(4 * (float)lat)) + (m4 * Mathf.Cos(6 * (float)lat));
            metersPerLon = (p1 * Mathf.Cos((float)lat)) + (p2 * Mathf.Cos(3 * (float)lat)) + (p3 * Mathf.Cos(5 * (float)lat));	   
        }
    }
}
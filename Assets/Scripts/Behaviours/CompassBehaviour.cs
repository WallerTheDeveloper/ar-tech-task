using System;
using CesiumForUnity;
using Data;
using Google.XR.ARCoreExtensions;
using Services;
using UnityEngine;

namespace Behaviours
{
    public class CompassBehaviour : MonoBehaviour
    {
        [SerializeField] private LocationData _locationData;
        
        [SerializeField] private GameObject _compassPrefab;

        [SerializeField] private AREarthManager _arEarthManager;

        
        private float metersPerLat;
        private float metersPerLon;
        
        private NavigationCalculationService _navigationCalculationService;
        private UserLocationService _userLocationService;
    
        private void Start() // don't change to Awake
        {
            _navigationCalculationService = new NavigationCalculationService();
            
            _userLocationService = new UserLocationService();
            _userLocationService.Init(_arEarthManager);
        }

        private void Update()
        {
            _navigationCalculationService.CalculateDistance(_userLocationService.GetUserLocation(),
                _locationData.TargetLatitude, _locationData.TargetLongitude);
            UpdateCompassRotation();
        }

        private void UpdateCompassRotation()
        {
            double targetLatitude = _locationData.TargetLatitude;
            double targetLongitude = _locationData.TargetLongitude;

            Vector3 targetPosition = ConvertGPStoUCS(targetLatitude, targetLongitude);
            
            Vector3 direction = (targetPosition - _compassPrefab.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction); // Calculate the rotation needed to look at the target position
            
            _compassPrefab.transform.rotation = targetRotation; // Apply the rotation to the compass
        }

        private Vector3 ConvertGPStoUCS(double latitude, double longitude)
        {

            var userLatitude = _userLocationService.GetUserLocation().Latitude;
            var userLongitude = _userLocationService.GetUserLocation().Longitude;
            
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



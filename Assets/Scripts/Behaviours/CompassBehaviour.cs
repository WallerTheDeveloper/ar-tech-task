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

        private GpsConversionService _conversionService;
        private NavigationCalculationService _navigationCalculationService;
        private UserLocationService _userLocationService;
    
        private void Start() // don't change to Awake
        {
            InitServices();
        }

        private void InitServices()
        {
            _navigationCalculationService = new NavigationCalculationService();

            _userLocationService = new UserLocationService();
            _userLocationService.Init(_arEarthManager);

            _conversionService = new GpsConversionService(_userLocationService);
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

            // Vector3 compassCoordinates = _conversionService.ConvertUCStoGPS(_compassPrefab.transform.position);
            // compassCoordinates = _conversionService.ConvertGPStoUCS(compassCoordinates.x, compassCoordinates.z);
            Vector3 targetPosition = _conversionService.ConvertGPStoUCS(targetLatitude, targetLongitude);

            print("Compass position: " + _compassPrefab.transform.position);
            // _compassPrefab.transform.position = compassCoordinates;
            _compassPrefab.transform.LookAt(targetPosition);

            
            // Vector3 direction = (targetPosition - _compassPrefab.transform.position).normalized;
            
            // Quaternion targetRotation = Quaternion.LookRotation(direction); // Calculate the rotation needed to look at the target position
            
            // _compassPrefab.transform.rotation = targetRotation; // Apply the rotation to the compass
        }
    }
}



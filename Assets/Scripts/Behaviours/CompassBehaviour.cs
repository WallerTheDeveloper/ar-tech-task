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

        private NavigationCalculationService _navigationCalculationService;
        private UserLocationService _userLocationService;
    
        private void Start() // don't change to Awake
        {
            _navigationCalculationService = new NavigationCalculationService();
            
            _userLocationService = new UserLocationService();
            _userLocationService.Init(_arEarthManager, _locationData);
            
            StartCoroutine(
                _navigationCalculationService.CalculateEverySeconds(1f, _userLocationService.GetUserLocation(), _locationData.TargetLatitude, _locationData.TargetLongitude)
                );
        }

        private void Update()
        {
            UpdateCompassRotation();
        }
        
        private void UpdateCompassRotation()
        {
            double targetLatitude = _locationData.TargetLatitude;
            double targetLongitude = _locationData.TargetLongitude;
            
            Vector3 targetPosition = ConvertToUnityCoordinates(targetLatitude, targetLongitude);

            Vector3 direction = (targetPosition - _compassPrefab.transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            _compassPrefab.transform.rotation = targetRotation;
        }

        private Vector3 ConvertToUnityCoordinates(double latitude, double longitude)
        {
            int sceneHeight = 100; // Set the height of your Unity scene in Unity units (e.g., meters or feet)
            int maxLatitude = 90;  // Set the maximum latitude range you want to represent in your scene

            // Calculate the conversion factor for latitude to Unity's Y-coordinate system
            float latitudeToUnityConversionFactor = sceneHeight / (2 * maxLatitude);

            
            // Get the user's current latitude and longitude
            double userLatitude = _userLocationService.GetUserLocation().Latitude;
            double userLongitude = _userLocationService.GetUserLocation().Longitude;
            
            // Convert latitude to Unity's Y-coordinate system
            float y = (float)(latitude - userLatitude) * latitudeToUnityConversionFactor;

            // Convert longitude to Unity's X-coordinate system
            float x = (float)(longitude - userLongitude);

            // Since we are using a 2D projection on the X-Z plane, set the Z-coordinate to 0
            float z = 0f;

            return new Vector3(x, y, z);
        }
    }
}



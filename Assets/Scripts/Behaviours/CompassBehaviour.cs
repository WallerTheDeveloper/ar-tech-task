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
            int sceneHeight = 100;
            int maxLatitude = 90;

            float latitudeToUnityConversionFactor = sceneHeight / (2 * maxLatitude);

            float y = (float) (latitude - _locationData.TargetLatitude) * latitudeToUnityConversionFactor;
            
            float z = (float) (longitude - _locationData.TargetLongitude);
            
            return new Vector3(0f, y, z);
        }
    }
}



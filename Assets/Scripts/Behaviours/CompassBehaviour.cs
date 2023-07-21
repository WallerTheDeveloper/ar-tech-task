using CesiumForUnity;
using Google.XR.ARCoreExtensions;
using Services;
using UnityEngine;

namespace Behaviours
{
    public class CompassBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
    
        [SerializeField] private float distance = 1.5f;
    
        [SerializeField] private GameObject _compassPrefab;

        [SerializeField] private CesiumGeoreference _cesiumGeoreference;

        [SerializeField] private AREarthManager _arEarthManager;

        private NavigationCalculationService _navigationCalculation;
    
        private void Start()
        {
            _navigationCalculation = new NavigationCalculationService();
        
            PlaceCompassInFrontOfCamera();
        
            StartCoroutine(_navigationCalculation.CalculateEverySeconds(1f, _arEarthManager, _cesiumGeoreference));
        }

        private void Update()
        {
            UpdateCompassRotation();
        }

        private void PlaceCompassInFrontOfCamera()
        {
            Vector3 positionInFrontOfCamera = Camera.main.transform.position + Camera.main.transform.forward * distance;
            transform.position = positionInFrontOfCamera;
        }
        private void UpdateCompassRotation()
        {
            // Get the target latitude and longitude from the NavigationCalculationService
            double targetLatitude = _cesiumGeoreference.latitude;
            double targetLongitude = _cesiumGeoreference.longitude;

            // Convert target latitude and longitude to Unity's coordinate system
            Vector3 targetPosition = ConvertToUnityCoordinates(targetLatitude, targetLongitude);

            // Calculate the direction from the compass to the target position
            Vector3 direction = (targetPosition - _compassPrefab.transform.position).normalized;

            // Calculate the rotation needed to look at the target position
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Apply the rotation to the compass
            _compassPrefab.transform.rotation = targetRotation;
        }

        private Vector3 ConvertToUnityCoordinates(double latitude, double longitude)
        {
            int sceneHeight = 100;
            int maxLatitude = 90;

            // Calculate the conversion factor for latitude to Unity's Y-coordinate system
            float latitudeToUnityConversionFactor = sceneHeight / (2 * maxLatitude);

            // Convert latitude to Unity's Y-coordinate system
            float y = (float)(latitude - _cesiumGeoreference.latitude) * latitudeToUnityConversionFactor;

            // Assuming your geospatial API uses WGS84 standard for latitude and longitude.
            // Unity uses a left-handed coordinate system where Y is up, Z is forward, and X is right.
            // We don't have a direct conversion factor for longitude, so we will directly use it as Z-coordinate.
            float z = (float)(longitude - _cesiumGeoreference.longitude);

            return new Vector3(0f, y, z);
        }
    }
}



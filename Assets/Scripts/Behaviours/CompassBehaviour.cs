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

        private IConververtor _conversionService;
        private INavigationCalculator _navigationCalculationService;
        private ILocationService _userLocationService;
    
        private void Start() // don't change to Awake
        {
            InitServices();
        }

        private void InitServices()
        {
            _navigationCalculationService = new NavigationCalculationService();

            _userLocationService = new UserLocationService(_arEarthManager);

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

            Vector3 targetPosition = _conversionService.ConvertGPStoUCS(targetLatitude, targetLongitude);

            _compassPrefab.transform.LookAt(targetPosition);
        }
    }
}



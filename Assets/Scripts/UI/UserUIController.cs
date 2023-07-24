using Configuration;
using Data;
using Google.XR.ARCoreExtensions;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UI
{
    [RequireComponent(typeof(LocationServicesConfiguration))]
    public class UserUIController : MonoBehaviour
    {
        // public double CurrentDistance { get; private set; }

        [SerializeField] private AREarthManager earthManager;
    
        [SerializeField] private ARCoreExtensions arcoreExtensions;

        [SerializeField] private AREarthManager _arEarthManager;
    
        [SerializeField] private TextMeshProUGUI geospatialStatusText;

        [SerializeField] private TextMeshProUGUI _scanPrompt;
            
        [SerializeField] private LocationData _locationData;

        [SerializeField] private LocationDataChannel _locationChannel;
        
        private UserLocationService _userLocationService;
        private NavigationCalculationService _navigationCalculation;
        
        
        private void Start()
        {
            _navigationCalculation = new NavigationCalculationService();
            _userLocationService = new UserLocationService();
            
            _userLocationService.Init(_arEarthManager);
        }

        private void OnEnable()
        {
            _scanPrompt.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (earthManager == null)
            {
                return;
            }

            if (ARSession.state != ARSessionState.SessionInitializing &&
                ARSession.state != ARSessionState.SessionTracking)
            {
                return;
            }
            // Check feature support and enable Geospatial API when it's supported.
            var featureSupport = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
            switch (featureSupport)
            {
                case FeatureSupported.Unknown:
                    break;
                case FeatureSupported.Unsupported:
                    Debug.Log("The Geospatial API is not supported by this device.");
                    break;
                case FeatureSupported.Supported:
                    if (arcoreExtensions.ARCoreExtensionsConfig.GeospatialMode == GeospatialMode.Disabled)
                    {
                        arcoreExtensions.ARCoreExtensionsConfig.GeospatialMode =
                            GeospatialMode.Enabled;
                        arcoreExtensions.ARCoreExtensionsConfig.StreetscapeGeometryMode =
                            StreetscapeGeometryMode.Enabled;
                    }
                    break;
            }

            var pose = earthManager.EarthState == EarthState.Enabled &&
                       earthManager.EarthTrackingState == TrackingState.Tracking ?
                earthManager.CameraGeospatialPose : new GeospatialPose();
            var supported = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);

            var distance = _navigationCalculation.CalculateDistance(pose, _locationData.TargetLatitude,
                _locationData.TargetLongitude);
            
            _locationChannel.CurrentDistance = distance;
            
            if(geospatialStatusText != null)
            {
                string text =
                        ARSession.state == ARSessionState.SessionInitializing ? "Session initializing..." 
                            : $"SessionState: {ARSession.state}\n" +
                              $"LocationServiceStatus: {Input.location.status}\n" +
                              $"FeatureSupported: {supported}\n" +
                              $"EarthState: {earthManager.EarthState}\n" +
                              $"EarthTrackingState: {earthManager.EarthTrackingState}\n" +
                              $"  LAT/LNG: {pose.Latitude:F6}, {pose.Longitude:F6}\n" +
                              $"  HorizontalAcc: {pose.HorizontalAccuracy:F6}\n" +
                              $"  ALT: {pose.Altitude:F2}\n" +
                              $"  VerticalAcc: {pose.VerticalAccuracy:F2}\n" +
                              $"  EunRotation: {pose.EunRotation:F2}\n" +
                              $"  OrientationYawAcc: {pose.OrientationYawAccuracy:F2}\n" +
                              $"  Distance to Target: {distance:F3}"
                    ;
                geospatialStatusText.SetText(text);
            }
            
            // _scanPrompt.gameObject.SetActive(_userLocationService.HasReachedTarget());
        }

        public void ShowScanText()
        {
            _scanPrompt.gameObject.SetActive(true);
        }

        private void TaskTextShow()
        {
            
        }
    }
}
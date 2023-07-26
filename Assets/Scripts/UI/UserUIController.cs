using System.Collections;
using System.Collections.Generic;
using Configuration;
using Data;
using Google.XR.ARCoreExtensions;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace UI
{
    [RequireComponent(typeof(LocationServicesConfiguration))]
    public class UserUIController : MonoBehaviour
    {
        [SerializeField] private AREarthManager earthManager;
    
        [SerializeField] private ARCoreExtensions arcoreExtensions;

        [SerializeField] private AREarthManager _arEarthManager;
    
        [SerializeField] private TextMeshProUGUI geospatialStatusText;

        [SerializeField] private TextMeshProUGUI _userMissionText;

        [SerializeField] private TextMeshProUGUI _scanPrompt;
            
        [SerializeField] private LocationData _locationData;

        [SerializeField] private LocationDataChannel _locationChannel;

        [SerializeField] private int _fadeEffectDuration;
        
        [SerializeField] private string[] _textsToDisplay;

        private IEnumerator _currentFade;
        
        private ILocationService _userLocationService;
        private INavigationCalculator _navigationCalculation;
        private ITextFader _fadingEffectService;
        
        private Queue<string> _instructionTextContainer = new();

        public void ShowScanText(bool show)
        {
            if (show)
            {
                _scanPrompt.gameObject.SetActive(true);
            }
            else
            {
                _scanPrompt.gameObject.SetActive(false);
            }
        }
        private void Start()
        {
            _instructionTextContainer = new Queue<string>();
            
            InitServices();

            _fadingEffectService.StartShowing();
        }
        private void OnEnable()
        {
            _scanPrompt.gameObject.SetActive(false);
        }
        private void Update()
         {
             if (IsSessionInitializing())
             {
                 return;
             }
             EnableGeospatialIfSupported();

             GeospatialPose pose = _userLocationService.GetUserLocation();
         
             var distance = _navigationCalculation.CalculateDistance(pose, _locationData.TargetLatitude,
                 _locationData.TargetLongitude);
             
             UpdateCurrentDistance(distance);
             
    #if UNITY_EDITOR
             pose.Latitude = 52.5162994656517;
             pose.Longitude = 13.4712489301791;
             distance = _navigationCalculation.CalculateDistance(pose, _locationData.TargetLatitude,
                 _locationData.TargetLongitude);
             UpdateCurrentDistance(distance);
    #endif
             ShowCurrentInfo(pose, distance);
         }

        private void ShowCurrentInfo(GeospatialPose pose, double distance)
        {
            if (geospatialStatusText != null)
            {
                string text =
                        ARSession.state == ARSessionState.SessionInitializing
                            ? "Session initializing..."
                            : 
                            // $"SessionState: {ARSession.state}\n" +
                              // $"LocationServiceStatus: {Input.location.status}\n" +
                              // $"EarthState: {earthManager.EarthState}\n" +
                              // $"EarthTrackingState: {earthManager.EarthTrackingState}\n" +
                              $"Your latitude/longitude: {pose.Latitude:F6}, {pose.Longitude:F6}\n" +
                              // $"  HorizontalAcc: {pose.HorizontalAccuracy:F6}\n" +
                              $"Your altitude: {pose.Altitude:F2}\n" +
                              // $"  VerticalAcc: {pose.VerticalAccuracy:F2}\n" +
                              // $"  EunRotation: {pose.EunRotation:F2}\n" +
                              // $"  OrientationYawAcc: {pose.OrientationYawAccuracy:F2}\n" +
                              $"  Distance to Target: {distance:F3} km"
                    ;
                geospatialStatusText.SetText(text);
            }
        }

        private void EnableGeospatialIfSupported()
        {
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
        }

        private bool IsSessionInitializing()
        {
            return ARSession.state != ARSessionState.SessionInitializing &&
                   ARSession.state != ARSessionState.SessionTracking;
        }

        private void UpdateCurrentDistance(double distance)
        {
            _locationChannel.CurrentDistance = distance;
        }

        private void InitServices()
        {
            _navigationCalculation = new NavigationCalculationService();
            _userLocationService = new UserLocationService(_arEarthManager);
            _fadingEffectService = new TextFadingEffectService(this, _instructionTextContainer, _textsToDisplay, _fadeEffectDuration, _userMissionText);

            // _userLocationService.Init(_arEarthManager);
        }
    }
}
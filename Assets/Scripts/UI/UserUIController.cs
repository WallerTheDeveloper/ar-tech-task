using System.Collections;
using System.Collections.Generic;
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
        
        private UserLocationService _userLocationService;
        private NavigationCalculationService _navigationCalculation;
        private UIFadingEffectService _fadingEffectService;
        
        private Queue<string> _instructionTextContainer = new();

        public void ShowScanText()
        {
            _scanPrompt.gameObject.SetActive(true);
        }
        private void Start()
        {
            _instructionTextContainer = new Queue<string>();
            
            InitServices();

            _fadingEffectService.AddTextToQueue();
        }
        private void OnEnable()
        {
            _scanPrompt.gameObject.SetActive(false);
        }
        private void Update()
         {
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
             
             var pose = _userLocationService.GetUserLocation();
             
             var supported = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
         
             var distance = _navigationCalculation.CalculateDistance(pose, _locationData.TargetLatitude,
                 _locationData.TargetLongitude);
             
             _locationChannel.CurrentDistance = distance;
    #if UNITY_EDITOR
             pose.Latitude = 52.5162994656517;
             pose.Longitude = 13.4712489301791;
             distance = _navigationCalculation.CalculateDistance(pose, _locationData.TargetLatitude,
                 _locationData.TargetLongitude);
    #endif
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
                               $"  Distance to Target: {distance:F3} km"
                     ;
                 geospatialStatusText.SetText(text);
             }
         }
        private void InitServices()
        {
            _navigationCalculation = new NavigationCalculationService();
            _userLocationService = new UserLocationService();
            _fadingEffectService = new UIFadingEffectService(this, _instructionTextContainer, _textsToDisplay, _fadeEffectDuration, _userMissionText);

            _userLocationService.Init(_arEarthManager);
        }
    }
}
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
        
        private Queue<string> _userInstructions = new Queue<string>();
        private List<Coroutine> _fadeCoroutines;
        
        private void Start()
        {
            _fadeCoroutines = new List<Coroutine>();
            _userInstructions = new Queue<string>();
            
            _navigationCalculation = new NavigationCalculationService();
            _userLocationService = new UserLocationService();

            _userLocationService.Init(_arEarthManager);
            
            foreach (string text in _textsToDisplay)
            {
                _userInstructions.Enqueue(text);
            }
            ShowNextText();
        }
        
        private void OnEnable()
        {
            _scanPrompt.gameObject.SetActive(false);
        }
        
        public void ShowScanText()
        {
            _scanPrompt.gameObject.SetActive(true);
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
         
             // var pose = earthManager.EarthState == EarthState.Enabled &&
             //            earthManager.EarthTrackingState == TrackingState.Tracking ?
             //     earthManager.CameraGeospatialPose : new GeospatialPose();
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
                               $"  Distance to Target: {distance:F3}"
                     ;
                 geospatialStatusText.SetText(text);
             }
         }

         private IEnumerator FadeInAndOutEffectFor(int seconds, TextMeshProUGUI textToFade, string text)
        {
            textToFade.SetText(text);
            for (float i = 0; i <= seconds; i += Time.deltaTime)
            {
                textToFade.color = new Color(1, 1, 1, i);
                yield return null;
            }
            for (float i = seconds; i >= 0; i -= Time.deltaTime)
            {
                textToFade.color = new Color(1, 1, 1, i);
                yield return null;
            }
            ShowNextText();
        }

        private void ShowNextText()
        {
            if (_userInstructions.Count > 0)
            {
                string nextText = _userInstructions.Dequeue();
                Coroutine currentFade =
                    StartCoroutine(FadeInAndOutEffectFor(_fadeEffectDuration, _userMissionText, nextText));
                _fadeCoroutines.Add(currentFade);
            }
            else
            {
                foreach (Coroutine fadeCoroutine in _fadeCoroutines)
                {
                    StopCoroutine(fadeCoroutine);
                }
                print("All stopped");
            }
        }
    }
}
﻿using System;
using Configuration;
using Google.XR.ARCoreExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(LocationServicesConfiguration))]
public class UserLocationManager : MonoBehaviour
{
    [SerializeField]
    private AREarthManager earthManager;
    
    [SerializeField]
    private ARCoreExtensions arcoreExtensions;
    
    [SerializeField]
    private TextMeshProUGUI geospatialStatusText;
    private void Update()
    {
        if (!Debug.isDebugBuild || earthManager == null)
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

        if(geospatialStatusText != null)
        {
            geospatialStatusText.text =
                $"SessionState: {ARSession.state}\n" +
                $"LocationServiceStatus: {Input.location.status}\n" +
                $"FeatureSupported: {supported}\n" +
                $"EarthState: {earthManager.EarthState}\n" +
                $"EarthTrackingState: {earthManager.EarthTrackingState}\n" +
                $"  LAT/LNG: {pose.Latitude:F6}, {pose.Longitude:F6}\n" +
                $"  HorizontalAcc: {pose.HorizontalAccuracy:F6}\n" +
                $"  ALT: {pose.Altitude:F2}\n" +
                $"  VerticalAcc: {pose.VerticalAccuracy:F2}\n" +
                $"  EunRotation: {pose.EunRotation:F2}\n" +
                $"  OrientationYawAcc: {pose.OrientationYawAccuracy:F2}";
        }
    }
}
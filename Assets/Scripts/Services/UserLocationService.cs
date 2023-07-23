using System;
using Data;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARSubsystems;

namespace Services
{
    public class UserLocationService
    {
        private AREarthManager _earthManager;
        private LocationData _locationData;
        
        public void Init(AREarthManager earthManager, LocationData locationData)
        {   
            _earthManager = earthManager;
            _locationData = locationData;
        }
        public GeospatialPose GetUserLocation()
        {
            GeospatialPose pose = _earthManager.EarthState == EarthState.Enabled &&
                              _earthManager.EarthTrackingState == TrackingState.Tracking ?
            _earthManager.CameraGeospatialPose : new GeospatialPose();

            return pose;
        }

        public bool HasReachedTarget()
        {
            var userLocation = GetUserLocation();

            if (userLocation.Latitude == _locationData.TargetLatitude && userLocation.Longitude == _locationData.TargetLongitude)
            {
                return true;
            }

            return false;
        }
    }
}
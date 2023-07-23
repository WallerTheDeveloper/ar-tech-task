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
            
            if (Math.Abs(userLocation.Latitude - _locationData.TargetLatitude) < 0.00000001f &&
                Math.Abs(userLocation.Longitude - _locationData.TargetLongitude) < 0.00000001f)
            {
                return true;
            }

            return false;
        }
    }
}
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARSubsystems;

namespace Services
{
    public class UserLocationService : ILocationService
    {
        private AREarthManager _earthManager;

        public UserLocationService(AREarthManager earthManager)
        {
            _earthManager = earthManager;
        }
        // public void Init(AREarthManager earthManager)
        // {   
        //     _earthManager = earthManager;
        // }
        public GeospatialPose GetUserLocation()
        {
            GeospatialPose pose = _earthManager.EarthState == EarthState.Enabled &&
                              _earthManager.EarthTrackingState == TrackingState.Tracking ?
            _earthManager.CameraGeospatialPose : new GeospatialPose();

            return pose;
        }
    }
}
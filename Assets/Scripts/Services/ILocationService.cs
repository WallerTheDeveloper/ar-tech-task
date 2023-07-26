using Google.XR.ARCoreExtensions;

namespace Services
{
    public interface ILocationService
    {
        public GeospatialPose GetUserLocation();
    }
}
using Google.XR.ARCoreExtensions;

namespace Services
{
    public interface INavigationCalculator
    {
        double CalculateDistance(GeospatialPose pose, double targetLatitude, double targetLongitude);
    }
}
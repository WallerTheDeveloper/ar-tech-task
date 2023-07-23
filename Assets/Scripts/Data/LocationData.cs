using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "New Location Data", menuName = "Custom Data/Location Data", order = 0)]
    public class LocationData : ScriptableObject
    {
        public double TargetLatitude;
        public double TargetLongitude;
    }
}
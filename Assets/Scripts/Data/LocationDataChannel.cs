using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Location Data Channel", menuName = "Custom Data/Location Data Channel", order = 0)]
    public class LocationDataChannel : ScriptableObject
    {
        public double CurrentDistance;
    }
}
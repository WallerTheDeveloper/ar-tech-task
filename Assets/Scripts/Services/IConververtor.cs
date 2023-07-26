using UnityEngine;

namespace Services
{
    public interface IConververtor
    {
        Vector3 ConvertGPStoUCS(double latitude, double longitude);
    }
}
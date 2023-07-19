using System.Collections;
using UnityEngine;

namespace Configuration
{
    public class LocationServicesConfiguration : MonoBehaviour
    {
        private Coroutine _locationServiceLauncher;
        private void OnEnable()
        {
            _locationServiceLauncher = StartCoroutine(StartLocationService());
        }

        private void OnDisable()
        {
            StopCoroutine(StartLocationService());
            _locationServiceLauncher = null;
            Input.location.Stop();
        }

        private IEnumerator StartLocationService()
        {
            if (!Input.location.isEnabledByUser)
            {
                yield break;
            }
        
            Input.location.Start();

            while (Input.location.status == LocationServiceStatus.Initializing)
            {
                yield return null;
            }

            if (Input.location.status != LocationServiceStatus.Running)
            {
                Input.location.Stop();
            }
        }
    }
}
using Services;
using UnityEngine;
using UnityEngine.Events;

public class MarkerController : MonoBehaviour
{
    [SerializeField] private float _distanceThresholdInKm;
    [SerializeField] private UnityEvent OnReachedTarget;

    private UserLocationService _userLocationService;
    private IDistanceInformant _distanceInformant;
    private void OnEnable()
    {
        transform.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnReachedTarget?.Invoke();
    }
    
    private void Update()
    {
        if (_distanceInformant.CurrentDistance <= _distanceThresholdInKm)
        {
            transform.gameObject.SetActive(true);
        }
    }
}
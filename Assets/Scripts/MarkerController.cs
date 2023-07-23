using System;
using Data;
using Services;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    private NavigationCalculationService _navigationCalculationService;
    private UserLocationService _userLocationService;
    
    [SerializeField] private float _approachThresholdInMeters;
    [SerializeField] private LocationData _locationData;
    private void Awake()
    {
        _navigationCalculationService = new NavigationCalculationService();
    }

    private void Update()
    {
        ShowMarkerAfterApproaching(_approachThresholdInMeters);
    }

    private void ShowMarkerAfterApproaching(float meters)
    {
        var distance = _navigationCalculationService.CalculateDistance(_userLocationService.GetUserLocation(), _locationData.TargetLatitude, _locationData.TargetLongitude);
        transform.gameObject.SetActive(distance <= meters);
    }
}
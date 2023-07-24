﻿using Data;
using Services;
using UnityEngine;
using UnityEngine.Events;

public class MarkerController : MonoBehaviour
{
    [SerializeField] private float _distanceThresholdInKm;
    [SerializeField] private UnityEvent OnReachedTarget;
    [SerializeField] private Renderer[] _childRenderersToHide;
    [SerializeField] private LocationDataChannel _locationChannel;
    
    private UserLocationService _userLocationService;
    
    
    private void OnEnable()
    {
        ShowObject(false);

    }

    private void ShowObject(bool enabledValue)
    {
        foreach (var childRenderer in _childRenderersToHide)
        {
            childRenderer.enabled = enabledValue;
        }
    }
    private void OnTriggerEnter (Collider col)
    {
        print("TRIGGER");

        OnReachedTarget?.Invoke();
    }
    
    private void Update()
    {
        if (_locationChannel.CurrentDistance <= _distanceThresholdInKm)
        {
            ShowObject(true);
        }
    }
}
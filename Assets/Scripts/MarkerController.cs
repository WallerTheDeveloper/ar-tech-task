﻿using Data;
using Services;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolUnityEvent : UnityEvent<bool>
{
}
public class MarkerController : MonoBehaviour
{
    public BoolUnityEvent OnReachedTarget;
    [SerializeField] private float _distanceThresholdInKm;
    [SerializeField] private Renderer[] _childRenderersToHide;
    [SerializeField] private LocationDataChannel _locationChannel;
    
    private UserLocationService _userLocationService;
    
    private void Awake()
    {
        ShowObject(false);
        
    }
    private void Update()
    {
        if (_locationChannel.CurrentDistance <= _distanceThresholdInKm)
        {
            ShowObject(true);
        }
        else
        {
            ShowObject(false);
        }
    }
    private void OnTriggerStay (Collider col)
    {
        print("TRIGGER STAY");

        OnReachedTarget?.Invoke(true);
    }
    private void OnTriggerExit(Collider other)
    {
        print("TRIGGER EXIT");

        OnReachedTarget?.Invoke(false);
    }
    private void ShowObject(bool enabledValue)
    {
        foreach (var childRenderer in _childRenderersToHide)
        {
            childRenderer.enabled = enabledValue;
        }
    }
    
}
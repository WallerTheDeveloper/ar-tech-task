using System.Collections.Generic;
using Data;
using Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class BoolUnityEvent : UnityEvent<bool>
{
}
public class MarkerController : MonoBehaviour
{
    [SerializeField] private CustomProjectTags _rendererObjectsTag;
    [SerializeField] private float _showObjectThresholdInKm;
    [SerializeField] private LocationDataChannel _locationChannel;
    
    private UserLocationService _userLocationService;
    private List<Renderer> _renderersToHide = new();
    
    [SerializeField] private BoolUnityEvent OnReachedTarget;

    private string rendererObjectsTag;
    private void Awake()
    {
        rendererObjectsTag = _rendererObjectsTag.ToString();
        FindMarkerMeshes(rendererObjectsTag, transform);
        ShowObject(false);
    }
    
    private void Update()
    {
        if (_locationChannel.CurrentDistance <= _showObjectThresholdInKm)
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
    private void FindMarkerMeshes(string tag, Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.CompareTag(tag))
            {
                _renderersToHide.Add(child.GetComponent<Renderer>());
            }
            FindMarkerMeshes(tag, child);
        }
    }
    private void ShowObject(bool enabledValue)
    {
        foreach (var renderer in _renderersToHide)
        {
            renderer.enabled = enabledValue;
        }
    }
    
}
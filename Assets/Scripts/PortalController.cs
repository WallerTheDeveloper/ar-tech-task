using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class PortalController : MonoBehaviour
{
    public Transform portal;
    public ARTrackedImageManager trackedImageManager;
    private Vector3 initialScale;

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    
    private void InitPortal()
    {
        foreach (ARTrackedImage trackedImage in trackedImageManager.trackables)
        {
            if (trackedImage.tag == "Portal")
            {
                portal = trackedImage.transform;
                break; // Found the portal, no need to continue the loop
            }
        }
        initialScale = portal.transform.localScale;
    }
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        InitPortal();
        
        foreach (ARTrackedImage trackedImage in trackedImageManager.trackables)
        {
            float distanceToMarker = Vector3.Distance(Camera.main.transform.position, trackedImage.transform.position);
            portal.transform.localScale = initialScale * distanceToMarker;   
        }
        // Debug.Log(
        //     $"There are {trackedImageManager.trackables.count} images being tracked.");
        // foreach (ARTrackedImage trackedImage in eventArgs.added)
        // {
        //     if (trackedImage.tag == "Portal")
        //     {
        //         portal.gameObject.SetActive(true);
        //     }
        // }
        //
        // foreach (ARTrackedImage trackedImage in eventArgs.updated)
        // {
        //     if (trackedImage.tag == "Portal")
        //     {
        //         float distanceToMarker = Vector3.Distance(Camera.main.transform.position, trackedImage.transform.position);
        //         portal.transform.localScale = initialScale * distanceToMarker;
        //     }
        // }
        //
        // foreach (ARTrackedImage trackedImage in eventArgs.removed)
        // {
        //     if (trackedImage.tag == "Portal")
        //     {
        //         portal.gameObject.SetActive(false);
        //     }
        // }
    }
}
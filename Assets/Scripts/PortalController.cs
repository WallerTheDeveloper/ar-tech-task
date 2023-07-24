using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class PortalController : MonoBehaviour
{
    
    public Transform portal;
    public ARTrackedImageManager trackedImageManager;
    private Vector3 initialScale;
    private bool portalFound;

    private void Start()
    {
        portalFound = false;
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    
    private void InitPortal()
    {
        if (portal == null)
        {
            foreach (ARTrackedImage trackedImage in trackedImageManager.trackables)
            {
                if (trackedImage.tag == "Portal")
                {
                    portal = trackedImage.transform;
                    initialScale = portal.localScale;
                    portalFound = true;
                    break; // Found the portal, no need to continue the loop
                }
            }
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (!portalFound)
        {
            InitPortal();
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.tag == "Portal" && trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                float distanceToMarker = Vector3.Distance(Camera.main.transform.position, trackedImage.transform.position);
                portal.localScale = initialScale * distanceToMarker;
                portal.position = trackedImage.transform.position;
                portal.rotation = trackedImage.transform.rotation;
            }
        }
    }
    // public Transform portal;
    // public ARTrackedImageManager trackedImageManager;
    // private Vector3 initialScale;
    //
    // private void OnEnable()
    // {
    //     trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    // }
    //
    // private void InitPortal()
    // {
    //     foreach (ARTrackedImage trackedImage in trackedImageManager.trackables)
    //     {
    //         if (trackedImage.tag == "Portal")
    //         {
    //             portal = trackedImage.transform;
    //             break; // Found the portal, no need to continue the loop
    //         }
    //     }
    //     initialScale = portal.transform.localScale;
    // }
    // void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    // {
    //     InitPortal();
    //     
    //     foreach (ARTrackedImage trackedImage in trackedImageManager.trackables)
    //     {
    //         float distanceToMarker = Vector3.Distance(Camera.main.transform.position, trackedImage.transform.position);
    //         portal.transform.localScale = initialScale * distanceToMarker;   
    //     }
    // }
}
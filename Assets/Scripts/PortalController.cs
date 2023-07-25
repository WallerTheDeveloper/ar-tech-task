using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class PortalController : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    
    private Transform portal;
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
                    
                    Vector3 eulerRotationOffset = new Vector3(270f, 0f, 0f);
                    portal.Rotate(eulerRotationOffset, Space.Self);
                    
                    break;
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
}
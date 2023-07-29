using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PortalController : MonoBehaviour
{
    [SerializeField] private CustomProjectTags _trackedObjectTag;
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
                if (trackedImage.CompareTag(_trackedObjectTag.ToString()))
                {
                    portal = trackedImage.transform;
                    initialScale = portal.localScale;
                    portalFound = true;
                   
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
            if (trackedImage.CompareTag(_trackedObjectTag.ToString()) && trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                float distanceToMarker = Vector3.Distance(Camera.main.transform.position, trackedImage.transform.position);
            
                float scaleMultiplier = 0.01f; // Adjust this value to control the scaling speed
                float newScale = Mathf.Clamp(initialScale.x + distanceToMarker * scaleMultiplier, 1f, 10f);
            
                portal.localScale = new Vector3(newScale, newScale, newScale);

                portal.position = trackedImage.transform.position;
                portal.rotation = trackedImage.transform.rotation * Quaternion.Euler(-270f, 0f, 0f);
            }
        }
    }
    // void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    // {
    //     if (!portalFound)
    //     {
    //         InitPortal();
    //         
    //     }
    //     foreach (ARTrackedImage trackedImage in eventArgs.updated)
    //     {
    //         if (trackedImage.CompareTag(_trackedObjectTag.ToString()) && trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
    //         {
    //             float distanceToMarker = Vector3.Distance(Camera.main.transform.position, trackedImage.transform.position);
    //             
    //             portal.localScale = -initialScale * distanceToMarker;
    //             print("local scale: " + portal.localPosition);
    //             print("Init scale: " + initialScale);
    //             print("distance: " + distanceToMarker);
    //
    //             portal.position = trackedImage.transform.position;
    //
    //             portal.rotation = trackedImage.transform.rotation * Quaternion.Euler(-270f, 0f, 0f);
    //             
    //             Debug.Log("Image position: " + trackedImage.transform.position + "Image rotation: " + trackedImage.transform.rotation);
    //             print("Position: " + portal.position + " rotation: " + portal.rotation);
    //         }
    //     }
    // }
}
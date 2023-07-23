using Data;
using Google.XR.ARCoreExtensions;
using Services;
using TMPro;
using UnityEngine;

public class UserUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scanPrompt;
    [SerializeField] private LocationData _locationData;
    [SerializeField] private AREarthManager _arEarthManager;
    
    private UserLocationService _userLocationService;

    private void Start()
    {
        _userLocationService = new UserLocationService();
        _userLocationService.Init(_arEarthManager, _locationData);
    }

    private void Update()
    {
        _scanPrompt.gameObject.SetActive(_userLocationService.HasReachedTarget());
    }
}
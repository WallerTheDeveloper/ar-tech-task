using System;
using UnityEngine;

namespace Behaviours
{
    public class PortalBehaviour : MonoBehaviour
    {
        private void Start()
        {
            transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
    }
}
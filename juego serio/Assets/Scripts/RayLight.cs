using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLight : MonoBehaviour
{
    [SerializeField] private int direction = 2;
    private void Start()
    {
        EventManager.instance.SuscribeToEvent("GetLights", GetLightRay);

    }


    private void GetLightRay()
    {
        EventManager.instance.RaiseEvent("RayActivated", transform.position, direction);
    }
}

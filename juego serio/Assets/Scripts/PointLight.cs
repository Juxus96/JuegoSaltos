using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour
{
    [SerializeField] private int radius;

    private void Start()
    {
        EventManager.instance.SuscribeToEvent("GetLights", GetPointLight);

    }

    private void GetPointLight()
    {
        EventManager.instance.RaiseEvent("LightMoved", transform.position, radius);
    }
}

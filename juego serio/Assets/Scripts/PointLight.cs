using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        EventManager.instance.SuscribeToEvent("GetLights", GetPointLight);

    }


    private void GetPointLight()
    {
        EventManager.instance.RaiseEvent("LightMoved", transform.position, 1);
    }
}

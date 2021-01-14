using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            EventManager.instance.RaiseEvent("Input_W");

        if(Input.GetKeyDown(KeyCode.A))
            EventManager.instance.RaiseEvent("Input_A");

        if (Input.GetKeyDown(KeyCode.S))
            EventManager.instance.RaiseEvent("Input_S");

        if (Input.GetKeyDown(KeyCode.D))
            EventManager.instance.RaiseEvent("Input_D");

    }
}

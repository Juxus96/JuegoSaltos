using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePlayer : MonoBehaviour
{
    [SerializeField] private Camera playerCam;

    private void Start()
    {
        playerCam.gameObject.SetActive(true);
    }
}

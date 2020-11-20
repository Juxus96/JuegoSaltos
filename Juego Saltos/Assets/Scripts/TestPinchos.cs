using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPinchos : MonoBehaviour
{
    [SerializeField]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().ResetPlayer();
        }
    }
}

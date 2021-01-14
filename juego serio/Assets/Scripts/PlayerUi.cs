using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private GameObject[] arrows;

    private void Start()
    {
        EventManager.instance.SuscribeToEvent("PlayerCanWalk", EnableArrows);
        EventManager.instance.SuscribeToEvent("PlayerCantWalk",DisableArrows);
    }

    private void EnableArrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive(true);
        }
    }

    private void DisableArrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive(false);
        }
    }
}

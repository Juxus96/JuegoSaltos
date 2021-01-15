using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private GameObject[] playerArrows;
    [SerializeField] private GameObject[] lightArrows;

    private void Start()
    {
        EventManager.instance.SuscribeToEvent("PlayerAction", () => DisableAll());
        EventManager.instance.SuscribeToEvent("DisableVisual", () => DisableAll());
        EventManager.instance.SuscribeToEvent("PlayerTurn", () => { EnablePlayerArrows(); DisableLightArrows(); });
        EventManager.instance.SuscribeToEvent("LightTurn", () => { DisablePlayerArrows(); EnableLightArrows(); });
    }

    private void EnablePlayerArrows()
    {
        for (int i = 0; i < playerArrows.Length; i++)
        {
            playerArrows[i].SetActive(true);
        }
    }

    private void DisablePlayerArrows()
    {
        for (int i = 0; i < lightArrows.Length; i++)
        {
            playerArrows[i].SetActive(false);
        }
    }private void EnableLightArrows()
    {
        for (int i = 0; i < playerArrows.Length; i++)
        {
            lightArrows[i].SetActive(true);
        }
    }

    private void DisableLightArrows()
    {
        for (int i = 0; i < lightArrows.Length; i++)
        {
            lightArrows[i].SetActive(false);
        }
    }

    private void DisableAll()
    {
        DisablePlayerArrows(); 
        DisableLightArrows();
    }
}

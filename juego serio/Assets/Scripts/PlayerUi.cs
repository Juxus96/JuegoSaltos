using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private GameObject[] playerArrows;
    [SerializeField] private GameObject[] lightArrows;
    [SerializeField] private MovementData playerMovementData;
    [SerializeField] private MovementData lightMovementData;

    private void Start()
    {
        EventManager.instance.SuscribeToEvent("PlayerAction", () => DisableAll());
        EventManager.instance.SuscribeToEvent("DisableVisual", () => DisableAll());

        EventManager.instance.SuscribeToEvent("PlayerTurn", () => { EnablePlayerArrows(); DisableLightArrows(); });
        EventManager.instance.SuscribeToEvent("LightTurn", () => { DisablePlayerArrows(); EnableLightArrows(); });
    }

    private void EnablePlayerArrows()
    {
        
        playerArrows[0].SetActive(playerMovementData.canMoveW);
        playerArrows[1].SetActive(playerMovementData.canMoveA);
        playerArrows[2].SetActive(playerMovementData.canMoveS);
        playerArrows[3].SetActive(playerMovementData.canMoveD);
    }

    private void DisablePlayerArrows()
    {
        for (int i = 0; i < lightArrows.Length; i++)
        {
            playerArrows[i].SetActive(false);
        }
    }
    
    private void EnableLightArrows()
    {
        lightArrows[0].SetActive(lightMovementData.canMoveW);
        lightArrows[1].SetActive(lightMovementData.canMoveA);
        lightArrows[2].SetActive(lightMovementData.canMoveS);
        lightArrows[3].SetActive(lightMovementData.canMoveD);
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

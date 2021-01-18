using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public enum TileState
    {
        LIGHT,
        DISCOVERED,
        DARK,
    }
    private TileState tileState;

    [SerializeField] private bool walkable;

    public Sprite lightSprite; 
    public Sprite darkSprite;
    private SpriteRenderer currentSprite;

    private void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        tileState = TileState.DARK;
        UpdateTile();
    }

    public void SetState(TileState newState)
    {
        tileState = newState;
        UpdateTile();
    }

    private void UpdateTile()
    {
        if (tileState == TileState.LIGHT)
        {
            walkable = true;
            currentSprite.sprite = lightSprite;
        }
        else
        {
            walkable = false;
            currentSprite.sprite = darkSprite;
        }

    }

    public void DoAction()
    {
        if (!walkable)
            EventManager.instance.RaiseEvent("PlayerInDark");
        else
            EventManager.instance.RaiseEvent("PlayerSafe");

    }

}

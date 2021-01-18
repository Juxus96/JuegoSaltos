using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public enum TileState
    {
        LIGHT,
        DARK,
    }
    private TileState tileState;

    [SerializeField] private bool walkable;
    public bool stairs;
    [SerializeField] private Sprite assetLight;
    [SerializeField] private Sprite assetDark;
    [SerializeField] private SpriteRenderer assetRenderer;

    [SerializeField] private Sprite lightSprite; 
    [SerializeField] private Sprite darkSprite;
    private SpriteRenderer currentSprite;

    private bool discovered;
    private bool hasAsset;

    private void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        discovered = false;
        hasAsset = assetRenderer != null;
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
            discovered = true;
            currentSprite.sprite = lightSprite;
            if(hasAsset)
                assetRenderer.sprite = assetLight;
        }
        else
        {
            walkable = false;
            currentSprite.sprite = darkSprite;
            if (hasAsset)
                if(discovered)
                    assetRenderer.sprite = assetDark;
                else
                    assetRenderer.sprite = null;

        }

    }

    public void DoAction()
    {
        if (!walkable)
            EventManager.instance.RaiseEvent("PlayerInDark");
        else
        {
            EventManager.instance.RaiseEvent("PlayerSafe");

        }

    }

}

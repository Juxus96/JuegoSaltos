using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public enum TileState
    {
        LIGHT,
        DARK,
    }
    protected TileState tileState;
    public enum TileType
    {
        BASIC = 0,
        STAIR = 10
    }
    public TileType tileType;
    [SerializeField] protected AssetData assetData;

    [SerializeField] protected Sprite lightSprite;
    [SerializeField] protected Sprite darkSprite;
    [SerializeField] protected Tile[] directionalTiles;
    public int layer;
    protected SpriteRenderer assetRenderer;
    protected SpriteRenderer spriteRenderer;
    protected bool discovered;


    public abstract void SteppedIn();
    public abstract Tile GetDirectionalTile(int i);
    public abstract void SetDirectionalTile(Tile tile, int dir);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        discovered = false;

        if (assetData != null)
            CreateAsset();

        tileState = TileState.DARK;
        UpdateTile();

    }
    public void SetState(TileState newState)
    {
        tileState = newState;
        UpdateTile();
    }

    protected void UpdateTile()
    {
        if (tileState == TileState.LIGHT)
        {
            discovered = true;
            spriteRenderer.sprite = lightSprite;
        }
        else
        {
            spriteRenderer.sprite = darkSprite;
        }

    }

    public bool IsDark()
    {
        return tileState == TileState.DARK;
    }

    protected void CreateAsset()
    {
        GameObject go = Instantiate(new GameObject(assetData.assetName), transform);
        assetRenderer = go.AddComponent<SpriteRenderer>();
        assetRenderer.transform.position += Vector3.up * assetData.assetOffset;
    }
}

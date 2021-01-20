﻿using System.Collections;
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
    [SerializeField] protected AssetData assetData;

    [SerializeField] protected Sprite lightSprite;
    [SerializeField] protected Sprite darkSprite;
    [SerializeField] public Tile WTile;
    [SerializeField] public Tile ATile;
    [SerializeField] public Tile STile;
    [SerializeField] public Tile DTile;
    protected SpriteRenderer assetRenderer;
    protected SpriteRenderer spriteRenderer;
    protected bool discovered;


    public abstract bool Walkable(Vector2 from);
    public abstract void SteppedIn();

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

    protected void CreateAsset()
    {
        GameObject go = Instantiate(new GameObject(assetData.assetName), transform);
        assetRenderer = go.AddComponent<SpriteRenderer>();
        assetRenderer.transform.position += Vector3.up * assetData.assetOffset;
    }
}
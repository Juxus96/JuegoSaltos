using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetData : ScriptableObject
{
    public string assetName;
    public Sprite assetLight;
    public Sprite assetDark;

    public float assetOffset;

    public abstract void DoAction();

}

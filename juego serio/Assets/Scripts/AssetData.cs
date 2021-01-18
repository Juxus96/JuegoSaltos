using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetData : ScriptableObject
{

    public Sprite assetLight;
    public Sprite assetDark;

    public float offset;

    public abstract void DoAction();

}

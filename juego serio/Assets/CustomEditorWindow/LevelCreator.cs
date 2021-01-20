using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelCreator : EditorWindow
{
    private enum DIRECTION
    {
        LEFT,
        RIGHT
    }

    private enum TILETYPE
    {
        FLOOR,
        STAIRS,
    }
    private Transform levelBase;
    private GameObject tileToSpawn;
    private TILETYPE tileType = TILETYPE.FLOOR;
    private DIRECTION direction = DIRECTION.RIGHT;
    private Vector2 tilePosition;
    private int visibilityLayer = 30;
    private int layer = 0;

    private float layerOffset = 0.6f;
    [MenuItem("Tools/Level Creator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelCreator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Creator");

        #region Create Tiles
        levelBase = EditorGUILayout.ObjectField("Level base", levelBase, typeof(Transform), true) as Transform;
        tileType = (TILETYPE)EditorGUILayout.EnumPopup("Tile type", tileType);
        if(tileType == TILETYPE.STAIRS)
            direction = (DIRECTION)EditorGUILayout.EnumPopup("Direction", direction);
        tilePosition = EditorGUILayout.Vector2Field("TilePosition", tilePosition);
        if (GUILayout.Button("Row Up"))
            tilePosition.y += 0.3f;
        if (GUILayout.Button("Row Down"))
            tilePosition.y -= 0.3f;
        visibilityLayer = EditorGUILayout.IntField("Visibility Layer", visibilityLayer);
        layer = EditorGUILayout.IntSlider("Layer", layer, 0, 3);
        tileToSpawn = EditorGUILayout.ObjectField("Tile To Spawn", tileToSpawn, typeof(GameObject), true) as GameObject;

        if(GUILayout.Button("Spawn Tile"))
        {
            SpawnTile();
        }

        if(GUILayout.Button("Connect TIles"))
        {
            ConnectTiles();
        }
        #endregion
    }

    private void SpawnTile()
    {
        if(tileToSpawn == null)
        {
            Debug.LogError("No tile added.");
            return;
        }

        GameObject tile = Instantiate(tileToSpawn, levelBase);
        tile.transform.position = tilePosition + Vector2.up * layer * layerOffset;
        tilePosition.x += 1.2f;
        visibilityLayer--;
    }

    private void ConnectTiles()
    {
        List<Tile> allTiles = new List<Tile>();

        for (int i = 0; i < levelBase.childCount; i++)
        {
            allTiles.Add(levelBase.GetChild(i).GetComponent<Tile>());
        }

        for (int i = 0; i < allTiles.Count; i++)
        {
            Tile tile = allTiles[i];
            tile.WTile = GetTileByPos((Vector2)tile.transform.position + Helpers.WDirection, allTiles);
            tile.ATile = GetTileByPos((Vector2)tile.transform.position + Helpers.ADirection, allTiles);
            tile.STile = GetTileByPos((Vector2)tile.transform.position + Helpers.SDirection, allTiles);
            tile.DTile = GetTileByPos((Vector2)tile.transform.position + Helpers.DDirection, allTiles);
        }
    }

    private Tile GetTileByPos(Vector2 pos, List<Tile> allTiles)
    {
        int i = 0;
        for (; i < allTiles.Count && Vector2.Distance(allTiles[i].transform.position, pos) > 0.01f; i++) ;
        return i < allTiles.Count ? allTiles[i] : null;

    }
}

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
    private Vector2 tilePosition = new Vector2(-4.8f, -3f);
    private int layer = 0;
    List<Tile> allTiles = new List<Tile>();
    private GameObject testTilePrefab;
    private GameObject testTile;


    private float layerOffset = Helpers.OffsetBetTiles.y * 2;
    [MenuItem("Tools/Level Creator")]
    public static void Init()
    {
        GetWindow(typeof(LevelCreator));

    }

    private void OnEnable()
    {
        testTilePrefab = (GameObject)Resources.Load("Prefabs/GroundTile", typeof(GameObject));
        testTile = Instantiate(testTilePrefab);
        testTile.GetComponent<SpriteRenderer>().sortingOrder = 10000;
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Creator");
        #region Create Tiles
        levelBase = EditorGUILayout.ObjectField("Level base", levelBase, typeof(Transform), true) as Transform;
        tileType = (TILETYPE)EditorGUILayout.EnumPopup("Tile type", tileType);
        if (tileType == TILETYPE.STAIRS)
            direction = (DIRECTION)EditorGUILayout.EnumPopup("Direction", direction);
        

        tilePosition = EditorGUILayout.Vector2Field("TilePosition", tilePosition);
        UpdateTestTile();

        layer = EditorGUILayout.IntSlider("Layer", layer, 0, 3);

        if (GUILayout.Button("Spawn Tile"))
        {
            SpawnTile();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Down", GUILayout.Width(position.width / 2)))
            tilePosition.y -= Helpers.OffsetBetTiles.y;
        if (GUILayout.Button("Up", GUILayout.Width(position.width / 2)))
            tilePosition.y += Helpers.OffsetBetTiles.y;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Left", GUILayout.Width(position.width / 2)))
            tilePosition.x -= Helpers.OffsetBetTiles.x;
        if (GUILayout.Button("Right", GUILayout.Width(position.width / 2)))
            tilePosition.x += Helpers.OffsetBetTiles.x;
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Connect TIles"))
        {
            ConectTiles();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("DELETE TILE"))
        {
            Tile tileToDestroy = Selection.activeTransform.gameObject.GetComponent<Tile>();
            allTiles.Remove(tileToDestroy);
            DestroyImmediate(tileToDestroy.gameObject);
        }

        #endregion
    }

    private void SpawnTile()
    {
        SetTile();
        if (tileToSpawn == null || levelBase == null)
        {
            Debug.LogError("No tile or transform added.");
            return;
        }
        if (tileToSpawn.GetComponent<Tile>() == null)
        {
            Debug.LogError("The attached game object isnt a tile.");
            return;
        }

        if (GetTileByPos(tilePosition + Vector2.up * layer * layerOffset) == null)
        {
            GameObject tile = Instantiate(tileToSpawn, levelBase);
            allTiles.Add(tile.GetComponent<Tile>());
            tile.transform.position = tilePosition + Vector2.up * layer * layerOffset;
            tile.GetComponent<Tile>().layer = layer;
            tile.GetComponent<SpriteRenderer>().sortingOrder = (int)(-tilePosition.y * 100);

        }
    }

    private void ConectTiles()
    {
        if (levelBase == null)
        {
            Debug.LogError("No transform added.");
            return;
        }
        for (int i = 0; i < levelBase.childCount; i++)
        {
            allTiles.Add(levelBase.GetChild(i).GetComponent<Tile>());
        }

        for (int i = 0; i < allTiles.Count; i++)
        {
            Tile tile = allTiles[i];
            if (tile.GetType() == typeof(StairTile))
            {
                StairTile stairTile = (StairTile)tile;
                Tile UpTile, downTile;
                int upperTilePos, lowerTilePos;

                if (stairTile.stairDirection == StairTile.DIRECTION.LEFT)
                {
                    upperTilePos = Helpers.W;
                    lowerTilePos = Helpers.S;
                }
                else
                {
                    upperTilePos = Helpers.D;
                    lowerTilePos = Helpers.A;
                }

                UpTile = GetTileByPos((Vector2)tile.transform.position + Helpers.Directions[upperTilePos]);
                downTile = GetTileByPos((Vector2)tile.transform.position + Helpers.Directions[lowerTilePos] - Vector2.up * layerOffset);

                UpTile.SetDirectionalTile(stairTile, lowerTilePos);
                downTile.SetDirectionalTile(stairTile, upperTilePos);
                stairTile.SetDirectionalTile(UpTile, upperTilePos);
                stairTile.SetDirectionalTile(downTile, lowerTilePos);
            }
            else
            {
                for (int j = 0; j < Helpers.Directions.Length; j++)
                {
                    tile.SetDirectionalTile(GetTileByPos((Vector2)tile.transform.position + Helpers.Directions[j]), j);
                }
            }
        }
    }

    private Tile GetTileByPos(Vector2 pos)
    {
        int i = 0;
        for (; i < allTiles.Count && Vector2.Distance(allTiles[i].transform.position, pos) > 0.01f; i++) ;
        return i < allTiles.Count ? allTiles[i] : null;

    }

    private void SetTile()
    {
        switch (tileType)
        {
            case TILETYPE.FLOOR:
                tileToSpawn = (GameObject)Resources.Load("Prefabs/GroundTile", typeof(GameObject));
                break;
            case TILETYPE.STAIRS:
                switch (direction)
                {
                    case DIRECTION.LEFT:
                        tileToSpawn = (GameObject)Resources.Load("Prefabs/StairsTileLeft", typeof(GameObject));
                        break;
                    case DIRECTION.RIGHT:
                        tileToSpawn = (GameObject)Resources.Load("Prefabs/StairsTileRight", typeof(GameObject));
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    private void UpdateTestTile()
    {
        Vector2 testTilePos = testTile.transform.position = tilePosition + layer * Vector2.up * layerOffset;

        bool freeSpace = GetTileByPos(testTile.transform.position) == null &&
            GetTileByPos(testTilePos + Vector2.up * Helpers.OffsetBetTiles.y * 2) == null &&
            GetTileByPos(testTilePos + Vector2.up * Helpers.OffsetBetTiles.y * 3) == null &&
            GetTileByPos(testTilePos + Vector2.up    * Helpers.OffsetBetTiles.y) == null &&
            GetTileByPos(testTilePos + Vector2.down  * Helpers.OffsetBetTiles.y) == null &&
            GetTileByPos(testTilePos + Vector2.right * Helpers.OffsetBetTiles.x) == null &&
            GetTileByPos(testTilePos + Vector2.left  * Helpers.OffsetBetTiles.x) == null;
        testTile.GetComponent<SpriteRenderer>().color = freeSpace? Color.green : Color.red;
    }

    private void OnDisable()
    {
        DestroyImmediate(testTile);
    }
}


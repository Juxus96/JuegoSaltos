using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform lightTransform;
    [SerializeField] private int lightRadius;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector2 offsetBetTiles;
    [SerializeField] private int maxTurnsInTheDark;
    [SerializeField] private float levelOffset;

    private int turnsInTheDark;

    private Vector2 direction;
    private Vector2 targetDir;
    private Vector2 targetPos;
    private Vector2 startPos;

    private bool playerTurn = true;
    private bool lightTurn = false;
    private bool moving = false;
    private bool followMode;

    private void Start()
    {
        startPos = lightTransform.position;

        EventManager.instance.SuscribeToEvent("PlayerTurn", () => { playerTurn = true; lightTurn = false; });
        EventManager.instance.SuscribeToEvent("PlayerInDark", () => PlayerInTheDark());
        EventManager.instance.SuscribeToEvent("PlayerSafe", () => turnsInTheDark = 0);


        EventManager.instance.SuscribeToEvent("Input_W", () => { if (CanMove()) SetDirectionW(); });
        EventManager.instance.SuscribeToEvent("Input_A", () => { if (CanMove()) SetDirectionA(); });
        EventManager.instance.SuscribeToEvent("Input_S", () => { if (CanMove()) SetDirectionS(); });
        EventManager.instance.SuscribeToEvent("Input_D", () => { if (CanMove()) SetDirectionD(); });
    }

    private void Update()
    {
        if(playerTurn)
        {
            if (targetDir != Vector2.zero)
            {
                Move();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (lightTurn)
                {
                    if (startPos != (Vector2)lightTransform.position)
                    {
                        EventManager.instance.RaiseEvent("PlayerAction");
                        playerTurn = false;
                        lightTurn = false;
                        startPos = lightTransform.position;
                    }
                    else
                    {
                        lightTurn = false;
                        EventManager.instance.RaiseEvent("PlayerTurn");

                    }
                }
                else
                {
                    lightTurn = true;
                    followMode = false;
                    EventManager.instance.RaiseEvent("LightTurn");

                }
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                print("a");
                followMode = true;
                lightTransform.position = playerTransform.position;
                EventManager.instance.RaiseEvent("LightMoved", lightTransform.position, lightRadius);
            }
        }
    }

    private void CheckMove()
    {
        // Checks the tile under the player
        EventManager.instance.RaiseEvent("PlayerMoved", playerTransform.position);
    }

    private void Move()
    {
        if (lightTurn)
            lightTransform.position = targetPos;
        else
        {
            playerTransform.Translate(targetDir * Time.deltaTime);
            if(followMode)
            {
                lightTransform.Translate(targetDir * Time.deltaTime);
            }
        }

        //Reset on reaching the target pos
        if (Vector3.Distance((lightTurn ? lightTransform : playerTransform).position, targetPos) < 0.01f)
        {

            (lightTurn ? lightTransform : playerTransform).position = targetPos;
            direction = targetPos = targetDir = Vector3.zero;
            moving = false;

            // if the player isnt moving the light end his turn
            if(!lightTurn)
            {
                playerTurn = false;
                EventManager.instance.RaiseEvent("PlayerAction");
                if(followMode)
                    EventManager.instance.RaiseEvent("LightMoved", lightTransform.position, lightRadius);

            }
            else
            {
                // Makes the arrows appear again
                EventManager.instance.RaiseEvent("LightTurn");

                // Updates the tiles beneath the light 
                EventManager.instance.RaiseEvent("LightMoved", lightTransform.position, lightRadius);
            }

            CheckMove();
        }
    }

    // TO-DO ADD RAYCASTS FOR WALLS
    private bool CanMove()
    {
        return !moving && playerTurn;
    }
    
    public void SetDirectionW()
    {
        direction = Vector2.left + Vector2.up;
        SetTargetDir();
    }
    public void SetDirectionA()
    {
        direction = Vector2.left + Vector2.down;
        SetTargetDir();
    }
    public void SetDirectionS()
    {
        direction = Vector2.right + Vector2.down;
        SetTargetDir();
    }
    public void SetDirectionD()
    {
        direction = Vector2.right + Vector2.up;
        SetTargetDir();
    }

    private void SetTargetDir()
    {
        moving = true;
        targetDir.x = direction.x * offsetBetTiles.x;
        targetDir.y = direction.y * offsetBetTiles.y;
        targetPos = (Vector2)(lightTurn ? lightTransform : playerTransform).position + targetDir;

        bool sameLevelTile = EventManager.instance.RaiseFuncEvent("CheckMove", targetPos);
        bool stairsUp = EventManager.instance.RaiseFuncEvent("CheckStairs", targetPos + Vector2.up * levelOffset);
        bool stairsDown = EventManager.instance.RaiseFuncEvent("CheckStairs", targetPos);

        if (!sameLevelTile && !stairsUp && !stairsDown)
        {
            targetPos = targetDir = Vector2.zero;
            moving = false;
        }
        else
        {
            if(!sameLevelTile && stairsUp)
            {
                targetPos += offsetBetTiles + Vector2.up * offsetBetTiles.y*2;
                targetDir.y += offsetBetTiles.y;
            }
            else if(sameLevelTile && stairsDown)
            {
                targetPos -= offsetBetTiles + Vector2.up * offsetBetTiles.y*2;
                targetDir.y -= offsetBetTiles.y;
            }
            targetDir.Normalize();

            // disables the visuals while moving
            EventManager.instance.RaiseEvent("DisableVisual");
        }

    }

    private void PlayerInTheDark()
    {
        if(++turnsInTheDark > maxTurnsInTheDark)
        {
            EventManager.instance.RaiseEvent("PlayerDied");
            lightTurn = false;
            playerTurn = true;
        }
    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform lightTransform;
    [SerializeField] private int lightRadius;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector2 distanceBetCubes;
    [SerializeField] private int maxTurnsInTheDark;

    private int turnsInTheDark;

    private Vector2 direction;
    private Vector3 targetDir;
    private Vector3 targetPos;
    private Vector2 startPos;

    private bool playerTurn = true;
    private bool lightTurn = false;
    private bool moving = false;

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
            if (targetDir != Vector3.zero)
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
                    EventManager.instance.RaiseEvent("LightTurn");

                }
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
        (lightTurn ? lightTransform : playerTransform).Translate(targetDir * Time.deltaTime);

        //Reset on reaching the target pos
        if (Vector3.Distance((lightTurn ? lightTransform : playerTransform).position, targetPos) < 0.01f)
        {

            (lightTurn ? lightTransform : playerTransform).position = targetPos;
            targetDir = Vector3.zero;
            targetPos = Vector3.zero;
            direction = Vector2.zero;

            moving = false;

            // if the player isnt moving the light end his turn
            if(!lightTurn)
            {
                playerTurn = false;
                EventManager.instance.RaiseEvent("PlayerAction");
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
        moving = false;
        targetDir.x = direction.x * distanceBetCubes.x;
        targetDir.y = direction.y * distanceBetCubes.y;

        targetPos = (lightTurn ? lightTransform : playerTransform).position + targetDir;
        targetDir.Normalize();

        // disables the visuals while moving
        EventManager.instance.RaiseEvent("DisableVisual");

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

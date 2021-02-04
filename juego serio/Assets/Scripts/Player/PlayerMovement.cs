using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform lightTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int lightRadius;
    [SerializeField] private int maxTurnsInTheDark;
    [SerializeField] private MovementData playerMoveData;
    [SerializeField] private MovementData lightMoveData;

    private int turnsInTheDark;
    private Transform movingTransform;

    private Vector2 targetDir;
    private Vector2 targetPos;
    private Vector2 startPos;

    private bool lightTurn = false;
    private bool moving;
    private bool followMode;

    private void Start()
    {
        playerMoveData.Init(playerTransform);
        lightMoveData.Init(lightTransform);

        startPos = lightTransform.position;
        movingTransform = playerTransform;

        EventManager.instance.SuscribeToEvent("PlayerTurn", PlayerTurn);
        EventManager.instance.SuscribeToEvent("GetLights", GetPlayerLight);
        EventManager.instance.SuscribeToTransformEvent("GetPlayer", () => playerTransform);

        EventManager.instance.SuscribeToEvent("Input_W", () => { SetDirection(Helpers.W); });
        EventManager.instance.SuscribeToEvent("Input_A", () => { SetDirection(Helpers.A); });
        EventManager.instance.SuscribeToEvent("Input_S", () => { SetDirection(Helpers.S); });
        EventManager.instance.SuscribeToEvent("Input_D", () => { SetDirection(Helpers.D); });
    }

    private void Update()
    {
        if(!moving)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (lightTurn)
                {
                    if (startPos != (Vector2)lightTransform.position)
                    {
                        // Move the enemies
                        CheckPlayerTile();
                    }
                    lightTurn = false;
                    movingTransform = playerTransform;
                    startPos = lightTransform.position;
                    EventManager.instance.RaiseEvent("PlayerVisuals");

                }
                else
                {
                    lightTurn = true;
                    movingTransform = lightTransform;
                    followMode = false;
                    startPos = lightTransform.position;
                    EventManager.instance.RaiseEvent("LightVisuals");

                }
            }
            else if(Input.GetKeyDown(KeyCode.F))
            {
                followMode = true;
                lightTransform.position = playerTransform.position;
                EventManager.instance.RaiseEvent("PlayerVisuals");
                EventManager.instance.RaiseEvent("UpdateLight");
                CheckPlayerTile();
            }
        }
        else if (targetDir != Vector2.zero)
        {
            Move();
        }
    }

    private void Move()
    {
        if (lightTurn)
        {
            lightTransform.position = targetPos;
            EventManager.instance.RaiseEvent("UpdateLight");
        }
        else
        {
            playerTransform.Translate(targetDir * Time.deltaTime);
            if(followMode)
            {
                lightTransform.position = targetPos;
                EventManager.instance.RaiseEvent("UpdateLight");

            }
        }

        //Reset on reaching the target pos
        if (Vector3.Distance((lightTurn ? lightTransform : playerTransform).position, targetPos) < 0.01f)
        {
            movingTransform.position = targetPos;
            targetPos = targetDir = Vector3.zero;

            if (!lightTurn)
                CheckPlayerTile();

            EventManager.instance.RaiseEvent("CheckTile", movingTransform.position);

        }
    }
    
    public void SetDirection(int direction)
    {
        if (!moving && (lightTurn ? lightMoveData : playerMoveData).canMove[direction])
        {
            moving = true;
            targetPos = EventManager.instance.RaiseVect2Event("CheckTileMovement", movingTransform.position, Helpers.Directions[direction]);
            targetDir = targetPos - (Vector2)movingTransform.position;

            targetDir.Normalize();

            // disables the visuals while moving
            EventManager.instance.RaiseEvent("DisableVisual");
        }
    }
   

    private void CheckPlayerTile()
    {
        if(EventManager.instance.RaiseBoolEvent("PlayerTileCheck", playerTransform.position))
        {
            if(++turnsInTheDark >= maxTurnsInTheDark)
            {
                turnsInTheDark = 0;
                lightTurn = false;
                movingTransform = playerTransform;
                EventManager.instance.RaiseEvent("PlayerDied");
                EventManager.instance.RaiseEvent("PlayerVisuals");
            }
        }
        else
        {
            turnsInTheDark = 0;
        }
    }

    private void GetPlayerLight()
    {
        EventManager.instance.RaiseEvent("LightMoved", lightTransform.position, lightRadius);
    }

    private void PlayerTurn()
    {
        moving = false;

        if(lightTurn)
        {
            EventManager.instance.RaiseEvent("LightVisuals");
        }
        else
        {
            EventManager.instance.RaiseEvent("PlayerVisuals");
        }


    }
}

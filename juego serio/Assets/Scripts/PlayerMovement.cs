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

    private Vector2 targetDir;
    private Vector2 targetPos;
    private Vector2 startPos;

    private bool playerTurn = true;
    private bool lightTurn = false;
    private bool followMode;

    private void Start()
    {
        playerMoveData.Init(playerTransform);
        lightMoveData.Init(lightTransform);

        startPos = lightTransform.position;

        EventManager.instance.SuscribeToEvent("PlayerTurn", () => { playerTurn = true; lightTurn = false; });
        EventManager.instance.SuscribeToEvent("PlayerInDark", () => PlayerInTheDark());
        EventManager.instance.SuscribeToEvent("PlayerSafe", () => turnsInTheDark = 0);
        EventManager.instance.SuscribeToEvent("GetLights", GetPlayerLight);


        EventManager.instance.SuscribeToEvent("Input_W", () => { if (playerTurn) SetDirectionW(); });
        EventManager.instance.SuscribeToEvent("Input_A", () => { if (playerTurn) SetDirectionA(); });
        EventManager.instance.SuscribeToEvent("Input_S", () => { if (playerTurn) SetDirectionS(); });
        EventManager.instance.SuscribeToEvent("Input_D", () => { if (playerTurn) SetDirectionD(); });
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
                    }
                    else
                    {
                        lightTurn = false;
                        EventManager.instance.RaiseEvent("PlayerTurn");

                    }
                    startPos = lightTransform.position;

                }
                else
                {
                    lightTurn = true;
                    followMode = false;
                    EventManager.instance.RaiseEvent("LightTurn");
                    startPos = lightTransform.position;

                }
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                followMode = true;
                lightTransform.position = playerTransform.position;
                EventManager.instance.RaiseEvent("PlayerTurn");
                EventManager.instance.RaiseEvent("UpdateLight");
            }
        }
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
                lightTransform.position = targetPos;

            }
        }

        //Reset on reaching the target pos
        if (Vector3.Distance((lightTurn ? lightTransform : playerTransform).position, targetPos) < 0.01f)
        {

            EventManager.instance.RaiseEvent("MovementUpdate");


            (lightTurn ? lightTransform : playerTransform).position = targetPos;
            targetPos = targetDir = Vector3.zero;


            // if the player isnt moving the light end his turn
            if(!lightTurn)
            {
                playerTurn = false;
                EventManager.instance.RaiseEvent("PlayerAction");
                if(followMode)
                    EventManager.instance.RaiseEvent("UpdateLight");

            }
            else
            {
                // Makes the arrows appear again
                EventManager.instance.RaiseEvent("LightTurn");

                // Updates the tiles beneath the light 
                EventManager.instance.RaiseEvent("UpdateLight");
            }

        }
    }
    
    public void SetDirectionW()
    {
        if ((lightTurn ? lightMoveData : playerMoveData).canMoveW)
            SetTargetDir(Helpers.WDirection);
    }
    public void SetDirectionA()
    {
        if((lightTurn ? lightMoveData : playerMoveData).canMoveA)
            SetTargetDir(Helpers.ADirection);
    }
    public void SetDirectionS()
    {
        if((lightTurn ? lightMoveData : playerMoveData).canMoveS)
            SetTargetDir(Helpers.SDirection);
    }
    public void SetDirectionD()
    {
        if((lightTurn ? lightMoveData : playerMoveData).canMoveD)
            SetTargetDir(Helpers.DDirection);
    }

    private void SetTargetDir(Vector2 direction)
    {
        targetPos = EventManager.instance.RaiseVect2Event("CheckTileMovement", (lightTurn ? lightTransform : playerTransform).position, direction);
        targetDir = targetPos - (Vector2)(lightTurn ? lightTransform : playerTransform).position;
        
        targetDir.Normalize();

        // disables the visuals while moving
        EventManager.instance.RaiseEvent("DisableVisual");

    }

    private void PlayerInTheDark()
    {
        if(++turnsInTheDark > maxTurnsInTheDark)
        {
            EventManager.instance.RaiseEvent("PlayerDied");
            turnsInTheDark = 0;
            lightTurn = false;
            playerTurn = true;
        }
    }

    private void GetPlayerLight()
    {
        EventManager.instance.RaiseEvent("LightMoved", lightTransform.position, lightRadius);
    }
}

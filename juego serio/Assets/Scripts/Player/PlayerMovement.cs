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

    private bool lightTurn = false;
    private bool moving;
    private bool followMode;

    private void Start()
    {
        playerMoveData.Init(playerTransform);
        lightMoveData.Init(lightTransform);

        startPos = lightTransform.position;

        EventManager.instance.SuscribeToEvent("PlayerTurn", () => { moving = false; lightTurn = false; });
        EventManager.instance.SuscribeToEvent("PlayerInDark", () => PlayerInTheDark());
        EventManager.instance.SuscribeToEvent("PlayerSafe", () => turnsInTheDark = 0);
        EventManager.instance.SuscribeToEvent("GetLights", GetPlayerLight);


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
                    }
                    lightTurn = false;
                    EventManager.instance.RaiseEvent("PlayerTurn");
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
            else if(Input.GetKeyDown(KeyCode.F))
            {
                followMode = true;
                lightTransform.position = playerTransform.position;
                EventManager.instance.RaiseEvent("PlayerTurn");
                EventManager.instance.RaiseEvent("UpdateLight");
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
                EventManager.instance.RaiseEvent("PlayerTurn");
                if(followMode)
                    EventManager.instance.RaiseEvent("UpdateLight");

            }
            else
            {
                // Makes the arrows appear again
                EventManager.instance.RaiseEvent("LightTurn");

                // Updates the tiles beneath the light 
                EventManager.instance.RaiseEvent("UpdateLight");
                moving = false;
            }

        }
    }
    
    public void SetDirection(int direction)
    {
        if (!moving && (lightTurn ? lightMoveData : playerMoveData).canMove[direction])
        {
            moving = true;
            targetPos = EventManager.instance.RaiseVect2Event("CheckTileMovement", (lightTurn ? lightTransform : playerTransform).position, Helpers.Directions[direction]);
            targetDir = targetPos - (Vector2)(lightTurn ? lightTransform : playerTransform).position;

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
            turnsInTheDark = 0;
            lightTurn = false;
        }
    }

    private void GetPlayerLight()
    {
        EventManager.instance.RaiseEvent("LightMoved", lightTransform.position, lightRadius);
    }
}

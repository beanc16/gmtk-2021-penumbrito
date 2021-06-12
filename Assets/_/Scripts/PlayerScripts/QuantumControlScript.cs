using Assets.__.Scripts;
using Assets.__.Scripts.PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class QuantumControlScript : MonoBehaviour
{
    static float COS_45 = 0.70710f;

    public float JumpForce = 10f;
    public float MoveVelocity = 5f;

    [ReadOnly] public Vector2 DesiredVelocity;

    private ControllablePlayer[] cachedControllablePlayers;
    private GameModel gameModel;
    private float startingJumpForce;
    private float startingMoveVelocity;
    private Vector3[] startingSize;

    // Start is called before the first frame update
    void Start()
    {
        this.startingJumpForce = JumpForce;
        this.startingMoveVelocity = MoveVelocity;

        var gameObjects = GameObject.FindGameObjectsWithTag("Player");
        cachedControllablePlayers = new ControllablePlayer[gameObjects.Length];
        this.startingSize = new Vector3[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            cachedControllablePlayers[i] = gameObjects[i].GetComponent<ControllablePlayer>();
            this.startingSize[i] = gameObjects[i].transform.localScale;
        }

        this.gameModel = GameModel.GetInstance();
        this.gameModel.RegisterPlayerControlScript(this);
    }

    public void UpdatePlayerEffects()
    {
        var effects = gameModel.ActivePlayerEffects;

        if (effects.ContainsKey(PlayerEffect.Jump))
        {
            this.JumpForce = effects[PlayerEffect.Jump] > 0 ?
                this.startingJumpForce * 3 :
                this.startingJumpForce;
        }

        if (effects.ContainsKey(PlayerEffect.Speed))
        {
            this.MoveVelocity = effects[PlayerEffect.Speed] > 0 ?
                this.startingMoveVelocity * 1.5f :
                this.startingMoveVelocity;
        }
    }

    enum EDirections : int
    {
        Right,
        Left,
        Count
    }

    // Update is called once per frame
    void Update()
    {
        if (gameModel.ActivePanels.Count == 0)
        {
            return;
        }

        if(gameModel.CountInWinZone == this.cachedControllablePlayers.Length)
        {
            SceneHandler.LoadScene("GameOver");
            return;
        }

        bool[] validDirections = new bool[(int)EDirections.Count];
        for (int i = 0; i < (int)EDirections.Count; ++i)
        {
            validDirections[i] = true;
        }

        foreach (var player in cachedControllablePlayers)
        {
            if (gameModel.ActivePanels[player.PlayerIndex] == false)
            {
                continue;
            }

            Vector2 playerPosition = player.transform.position;
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider)
            {
                List<ContactPoint2D> contacts = new List<ContactPoint2D>();
                playerCollider.GetContacts(contacts);

                foreach (ContactPoint2D contact in contacts)
                {
                    Debug.DrawLine(contact.point, playerPosition, Color.red, 0f);
                    Vector2 revContactNormal = -contact.normal;

                    if (Vector2.Dot(revContactNormal, Vector2.right) >= COS_45)
                    {
                        validDirections[(int)EDirections.Right] = false;
                    }
                    if (Vector2.Dot(revContactNormal, Vector2.left) >= COS_45)
                    {
                        validDirections[(int)EDirections.Left] = false;
                    }

                    //Jumping
                    if (Vector2.Dot(revContactNormal, Vector2.down) >= COS_45 &&
                        (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
                    {
                        Rigidbody2D playerRigidBody = player.GetComponent<Rigidbody2D>();
                        playerRigidBody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                    }
                }
            }
        }

        DesiredVelocity = new Vector2(0f, 0f);
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) &&
            validDirections[(int)EDirections.Right])
        {
            DesiredVelocity += Vector2.right * MoveVelocity;
        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) &&
            validDirections[(int)EDirections.Left])
        {
            DesiredVelocity += Vector2.left * MoveVelocity;
        }

        foreach (var player in cachedControllablePlayers)
        {
            if (gameModel.ActivePanels[player.PlayerIndex] == false)
            {
                continue;
            }
            player.transform.position += (Vector3)DesiredVelocity * Time.deltaTime;
        }
    }
}

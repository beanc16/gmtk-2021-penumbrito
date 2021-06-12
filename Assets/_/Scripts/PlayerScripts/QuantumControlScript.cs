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
    public float JumpTime = 2f;

    public float GravityForce = -9.81f;

    public float MoveVelocity = 5f;

    [ReadOnly] public float CurrentGravityForce;
    [ReadOnly] public Vector2 DesiredVelocity;

    private float jumpTimer = 0f;

    private ControllablePlayer[] cachedControllablePlayers;
    private GameModel gameModel;
    private float startingJumpForce;
    private float startingMoveVelocity;
    private Vector3[] startingSize;

    private bool[] validDirections;

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

        this.CurrentGravityForce = GravityForce;
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
        Up,
        Down,
        Count
    }

    // Update is called once per frame
    void Update()
    {
        if (gameModel.CountInWinZone == this.cachedControllablePlayers.Length)
        {
            SceneHandler.LoadScene("GameOver");
            return;
        }

        this.validDirections = new bool[(int)EDirections.Count];
        for (int i = 0; i < (int)EDirections.Count; ++i)
        {
            this.validDirections[i] = true;
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

                    // For each contact, populate the valid directions we can move in
                    Vector2 revContactNormal = -contact.normal;
                    PopulateValidDirections(revContactNormal);
                }
            }
        }

        ComputeGravityForce();
        this.DesiredVelocity = GetDesiredVelocityFromValidDirections();

        foreach (var player in cachedControllablePlayers)
        {
            if (gameModel.ActivePanels[player.PlayerIndex] == false)
            {
                continue;
            }
            // ???
            if (Time.deltaTime < 0.1f)
            {
                player.transform.position += (Vector3)DesiredVelocity * Time.deltaTime;
            }
        }
    }

    void ComputeGravityForce()
    {
        if (!this.validDirections[(int)EDirections.Down])
        {
            if (CurrentGravityForce <= 0f)
            {
                if (GetInputFromDirection(EDirections.Up))
                {
                    this.CurrentGravityForce = this.JumpForce;
                    this.jumpTimer = this.JumpTime;
                }
                else
                {
                    this.CurrentGravityForce = this.GravityForce;
                    this.jumpTimer = 0f;
                }
            }
        }

        if (this.jumpTimer > 0f)
        {
            this.jumpTimer -= Time.deltaTime;
            // 0 (Gravity Force) -> 1 (Jump Force) this is reversed cause lazy
            this.CurrentGravityForce = Mathf.Lerp(this.GravityForce, this.JumpForce, jumpTimer / this.JumpTime);
        }
    }

    bool GetInputFromDirection(EDirections direction)
    {
        switch (direction) {
            case EDirections.Up:
                return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
            case EDirections.Left:
                return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            case EDirections.Right:
                return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        }
        return false;
    }

    void PopulateValidDirections(Vector2 revContactNormal)
    {
        if (Vector2.Dot(revContactNormal, Vector2.right) >= COS_45)
        {
            this.validDirections[(int)EDirections.Right] = false;
        }
        if (Vector2.Dot(revContactNormal, Vector2.left) >= COS_45)
        {
            this.validDirections[(int)EDirections.Left] = false;
        }
        if (Vector2.Dot(revContactNormal, Vector2.up) >= COS_45)
        {
            this.validDirections[(int)EDirections.Up] = false;
        }
        if (Vector2.Dot(revContactNormal, Vector2.down) >= COS_45)
        {
            this.validDirections[(int)EDirections.Down] = false;
        }
    }

    Vector2 GetDesiredVelocityFromValidDirections()
    {
        Vector2 desiredVelocity = new Vector2(0f,0f);

        if (GetInputFromDirection(EDirections.Right) && this.validDirections[(int)EDirections.Right])
        {
            desiredVelocity += Vector2.right* this.MoveVelocity;
        }
        if (GetInputFromDirection(EDirections.Left) && this.validDirections[(int)EDirections.Left])
        {
            desiredVelocity += Vector2.left * this.MoveVelocity;
        }
        if (this.CurrentGravityForce > 0f)
        {
            if (this.validDirections[(int)EDirections.Up])
            {
                desiredVelocity += Vector2.up * this.CurrentGravityForce;
            }
        }
        else
        {
            if (this.validDirections[(int)EDirections.Down])
            {
                desiredVelocity += Vector2.up * this.CurrentGravityForce;
            }
        }

        return desiredVelocity;
    }
}

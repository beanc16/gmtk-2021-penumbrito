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

    public float GravityForce = -9.81f;

    public float JumpGravityMultiplierMax = 1.5f;
    public float JumpGravityMultiplierMin = 0.5f;

    public float DashTime = 1f;
    public float DashForce = 10f;
    public float DashDecelerationMultiplier = 3f;

    public float MoveAccelMult = 0.5f;
    public float MoveVelocity = 5f;

    [ReadOnly] public float CurrentGravityForce;
    [ReadOnly] public float CurrentDashForce;
    [ReadOnly] public Vector2 DesiredVelocity;

    private ControllablePlayer[] cachedControllablePlayers;
    private GameModel gameModel;
    private float startingJumpForce;
    private float startingMoveVelocity;
    private Vector3[] startingSize;

    private bool[] validDirections;

    private bool isJumping;
    private bool isDashing;
    private bool initialFall;

    private bool canDash;
    private Vector2 dashDirection;
    private bool isFacingLeft;

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
        this.CurrentDashForce = DashForce;

        this.canDash = true;
        this.initialFall = true;
        this.isFacingLeft = false;
    }

    public void UpdatePlayerEffects()
    {
        var effects = gameModel.ActivePlayerEffects;
        string activePowersText = "Active Powers : ";

        if (effects.ContainsKey(PlayerEffect.Speed))
        {
            this.MoveVelocity = effects[PlayerEffect.Speed] > 0 ?
                this.startingMoveVelocity * 1.5f :
                this.startingMoveVelocity;

            if (effects[PlayerEffect.Speed] > 0)
            {
                activePowersText += "| Speed |";
            }
        }
        if (effects.ContainsKey(PlayerEffect.Dash))
        {
            if (effects[PlayerEffect.Dash] > 0)
            {
                activePowersText += "| Dash |";
            }
        }
        if (effects.ContainsKey(PlayerEffect.Pogo))
        {
            if (effects[PlayerEffect.Pogo] > 0)
            {
                activePowersText += "| Pogo |";
            }
        }
        if (effects.ContainsKey(PlayerEffect.Melee))
        {
            if (effects[PlayerEffect.Melee] > 0)
            {
                activePowersText += "| Melee |";
            }
        }

        GameObject activePowersObject = GameObject.FindGameObjectsWithTag("ActivePowersUI")[0];
        UnityEngine.UI.Text text = activePowersObject.GetComponent<UnityEngine.UI.Text>();
        text.text = activePowersText;

    }

    public void OnPlayerHitHazard(GameObject hazard, bool wasDeadly)
    {
        bool isLethalDamage = true;
        var effects = gameModel.ActivePlayerEffects;
        if (effects[PlayerEffect.Melee] > 0)
        {
            if (hazard.GetComponent<MeleeObject>())
            {
                Destroy(hazard);

                isLethalDamage = false;
            }
        }

        if (wasDeadly == false && effects[PlayerEffect.Pogo] > 0)
        {
            this.CurrentGravityForce = this.JumpForce;
            this.isJumping = true;

            isLethalDamage = false;
        }

        if (isLethalDamage)
        {
            GameModel.GetInstance().ReloadScene = true;
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
        if (gameModel.ReloadScene)
        {
            SceneHandler.RestartCurrentScene();
            return;
        }

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

        //Hacky
        int validPlayers = 0;
        foreach (var player in cachedControllablePlayers)
        {
            if (gameModel.ActivePanels[player.PlayerIndex] == false && !this.initialFall)
            {
                continue;
            }

            validPlayers++;
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

        if (validPlayers == 0 && !this.initialFall)
        {
            return;
        }

        this.DesiredVelocity = new Vector2(0f, 0f);
        ComputeGravityForce();
        if (!this.initialFall)
        {
            ComputeDashForce();
            ComputeInputMovement();
        }

        this.DesiredVelocity = GetDesiredVelocityFromValidDirections();

        //Vector2 localSyncPosition = cachedControllablePlayers[0].transform.localPosition;
        foreach (var player in cachedControllablePlayers)
        {
            if (gameModel.ActivePanels[player.PlayerIndex] == false && !this.initialFall)
            {
                continue;
            }

            SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
            if (this.DesiredVelocity.x > Mathf.Epsilon)
            {
                spriteRenderer.flipX = false;
            }
            else if (this.DesiredVelocity.x < -Mathf.Epsilon)
            {
                spriteRenderer.flipX = true;
            }

            Animator animator = player.GetComponent<Animator>();
            if (animator)
            {
                animator.SetBool("IsJumping", this.isJumping);
                animator.SetBool("IsRunning", Mathf.Abs(this.DesiredVelocity.x) > Mathf.Epsilon);
                animator.SetBool("IsDashing", isDashing);
                animator.SetBool("IsFalling", this.DesiredVelocity.y < -0.1f);
            }

            //player.gameObject.transform.localPosition = localSyncPosition;
            float desiredX = this.DesiredVelocity.x;
            float desiredY = this.DesiredVelocity.y;
            float currentX = player.GetComponent<Rigidbody2D>().velocity.x;

            Vector2 adjustedDesiredVelocity = new Vector2(Mathf.Lerp(currentX, desiredX, MoveAccelMult), desiredY);
            player.GetComponent<Rigidbody2D>().velocity = adjustedDesiredVelocity;
        }
    }

    public bool CanUpdatePanel()
    {
        return Mathf.Abs(this.DesiredVelocity.magnitude) < Mathf.Epsilon;
    }

    void ComputeGravityForce()
    {
        if (!this.validDirections[(int)EDirections.Down])
        {
            if (Input.GetKeyDown(KeyCode.Space) && !this.isJumping)
            {
                this.CurrentGravityForce = this.JumpForce;
                this.isJumping = true;
            }
            if (this.CurrentGravityForce < 0f)
            {
                this.initialFall = false;
                this.isJumping = false;
            }
        }
        else
        {
            float gravityForce = this.GravityForce;
            if (this.isJumping)
            {
                gravityForce *= Mathf.Lerp(JumpGravityMultiplierMax, JumpGravityMultiplierMin, this.CurrentGravityForce / JumpForce);
            }

            this.CurrentGravityForce += gravityForce * Time.deltaTime;
        }

    }

    void ComputeDashForce()
    {
        if (this.CurrentDashForce <= 0f)
        {
            if (this.canDash && Input.GetKeyDown(KeyCode.LeftShift))
            {
                var effects = gameModel.ActivePlayerEffects;
                if (effects[PlayerEffect.Dash] > 0)
                {
                    this.dashDirection = new Vector2(0f, 0f);
                    if (GetInputFromDirection(EDirections.Up))
                    {
                        this.dashDirection += Vector2.up;
                    }
                    if (GetInputFromDirection(EDirections.Down))
                    {
                        this.dashDirection += Vector2.down;
                    }
                    if (GetInputFromDirection(EDirections.Right))
                    {
                        this.dashDirection += Vector2.right;
                    }
                    if (GetInputFromDirection(EDirections.Left))
                    {
                        this.dashDirection += Vector2.left;
                    }
                    this.dashDirection.Normalize();
                    this.CurrentGravityForce = 0f;
                    this.CurrentDashForce = this.DashForce;
                    this.isDashing = true;
                }
            }
            else
            {
                if (!this.validDirections[(int)EDirections.Down])
                {
                    this.canDash = true;
                }
                this.isDashing = false;
            }
        }
        else
        {
            if (this.isDashing && this.CurrentDashForce > 0f)
            {
                this.CurrentGravityForce = 0f;
            }
            this.CurrentDashForce = Mathf.Max(0f, this.CurrentDashForce - DashForce * DashDecelerationMultiplier * Time.deltaTime);
        }
    }

    bool GetInputFromDirection(EDirections direction)
    {
        switch (direction)
        {
            case EDirections.Up:
                return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
            case EDirections.Left:
                return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            case EDirections.Right:
                return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
            case EDirections.Down:
                return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
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

    void ComputeInputMovement()
    {
        if (GetInputFromDirection(EDirections.Right))
        {
            this.DesiredVelocity += Vector2.right * this.MoveVelocity;
        }
        if (GetInputFromDirection(EDirections.Left))
        {
            this.DesiredVelocity += Vector2.left * this.MoveVelocity;
        }
    }

    Vector2 GetDesiredVelocityFromValidDirections()
    {
        Vector2 desiredVelocity = this.DesiredVelocity;

        //Apply dash
        desiredVelocity += this.dashDirection * this.CurrentDashForce;

        //Apply gravity
        desiredVelocity += Vector2.up * this.CurrentGravityForce;

        if (!this.validDirections[(int)EDirections.Right])
        {
            desiredVelocity = new Vector2(Mathf.Min(desiredVelocity.x, 0f), desiredVelocity.y);
        }
        if (!this.validDirections[(int)EDirections.Left])
        {
            desiredVelocity = new Vector2(Mathf.Max(desiredVelocity.x, 0f), desiredVelocity.y);
        }
        if (!this.validDirections[(int)EDirections.Up])
        {
            desiredVelocity = new Vector2(desiredVelocity.x, Mathf.Min(desiredVelocity.y, 0f));
        }
        if (!this.validDirections[(int)EDirections.Down])
        {
            desiredVelocity = new Vector2(desiredVelocity.x, Mathf.Max(desiredVelocity.y, 0f));
        }

        return desiredVelocity;
    }
}

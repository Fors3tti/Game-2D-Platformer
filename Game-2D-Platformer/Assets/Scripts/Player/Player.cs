using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject fruitDrop;
    [SerializeField] private DifficultyType gameDifficulty;
    private GameManager gameManager;

    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D cd;

    public PlayerInputSet playerInput {  get; private set; }
    private Vector2 moveInput;

    private bool canBeControlled = false;

    [Header("Movement Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private float defaultGravityScale;
    private bool canDoubleJump;

    [Header("Buffer & Coyote Jump")]
    [SerializeField] private float bufferJumpWindow = .25f;
    private float bufferJumpActivated = -1;
    [SerializeField] private float coyoteJumpWindow = .25f;
    private float coyoteJumpActivated = -1;

    [Header("Wall Info")]
    [SerializeField] private float wallJumpDuration = .6f;
    [SerializeField] private Vector2 wallJumpForce;
    private bool isWallJumping;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = .5f;
    [SerializeField] private Vector2 knockbackPower;
    private bool isKnocked;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [Space]
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;
    [SerializeField] private LayerMask whatIsEnemy;
    private bool isGrounded;
    private bool isAirbone;
    private bool isWallDetected;

    private bool facingRight = true;
    private int facingDir = 1;

    [Header("Player Visuals")]
    [SerializeField] private AnimatorOverrideController[] animators;
    [SerializeField] private GameObject deathVfx;
    [SerializeField] private ParticleSystem dustFx;
    [SerializeField] private int skinId;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CapsuleCollider2D>();

        playerInput = new PlayerInputSet();
    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Player.Jump.performed += ctx => JumpButton();
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        playerInput.Disable();

        playerInput.Player.Jump.performed -= ctx => JumpButton();
        playerInput.Player.Movement.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled -= ctx => moveInput = Vector2.zero;
    }

    private void Start()
    {
        defaultGravityScale = rb.gravityScale;
        gameManager = GameManager.instance;

        UpdateGameDifficulty();
        RespawnFinished(false);
        UpdateSkin();
    }

    private void Update()
    {
        UpdateAirboneStatus();

        if (canBeControlled == false)
        {
            HandleCollision();
            HandleAnimations();
            return;
        }

        if (isKnocked)
            return;

        HandleEnemyDetection();
        //HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleCollision();
        HandleAnimations();
    }

    public void Damage()
    {
        if (gameDifficulty == DifficultyType.Normal)
        {
            if (gameManager.FruitsCollected() <= 0)
            {
                Die();
                gameManager.RestartLevel();
            }
            else
            {
                ObjectCreator.instance.CreateObject(fruitDrop, transform, true);
                gameManager.RemoveFruit();
            }

            return;
        }

        if (gameDifficulty == DifficultyType.Hard)
        {
            Die();
            gameManager.RestartLevel();
        }
    }

    private void UpdateGameDifficulty()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;

        if (difficultyManager != null)
            gameDifficulty = difficultyManager.difficulty;
    }

    public void UpdateSkin()
    {
        SkinManager skinManager = SkinManager.instance;

        if (skinManager == null)
            return;

        anim.runtimeAnimatorController = animators[skinManager.choosenSkinId];
    }

    private void HandleEnemyDetection()
    {
        if (rb.linearVelocity.y >= 0)
            return;

        Collider2D[] colliders = Physics2D.
            OverlapCircleAll(enemyCheck.position, enemyCheckRadius, whatIsEnemy);

        foreach (var enemy in colliders)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
            if(newEnemy != null)
            {
                AudioManager.instance.PlaySFX(1);
                newEnemy.Die();
                Jump();
            }
        }
    }

    public void RespawnFinished(bool finished)
    {

        if (finished)
        {
            rb.gravityScale = defaultGravityScale;
            canBeControlled = true;
            cd.enabled = true;

            AudioManager.instance.PlaySFX(11);
        }
        else
        {
            rb.gravityScale = 0;
            canBeControlled = false;
            cd.enabled = false;
        }
    }

    public void Knockback(float sourceDamageXPosition)
    {
        float knockbackDir = 1;

        if (transform.position.x < sourceDamageXPosition)
            knockbackDir = -1;

        if (isKnocked)
            return;

        AudioManager.instance.PlaySFX(9);
        CameraManager.instance.ScreenShake(knockbackDir);
        StartCoroutine(KnockbackRoutine());

        rb.linearVelocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnocked = true;
        anim.SetBool("isKnocked", true);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
        anim.SetBool("isKnocked", false);
    }

    public void Die()
    {
        AudioManager.instance.PlaySFX(0);

        GameObject newDeathVfx = Instantiate(deathVfx, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void Push(Vector2 direction, float duration = 0)
    {
        StartCoroutine(PushCoroutine(direction, duration));
    }

    private IEnumerator PushCoroutine(Vector2 direction, float duration)
    {
        canBeControlled = false;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        canBeControlled = true;
    }

    private void UpdateAirboneStatus()
    {
        if (isGrounded && isAirbone)
            HandleLanding();

        if (!isGrounded && !isAirbone)
            BecomeAirborne();
    }

    private void BecomeAirborne()
    {
        isAirbone = true;

        if(rb.linearVelocity.y < 0)
        {
            ActivateCoyoteJump();
        }
    }

    private void HandleLanding()
    {
        dustFx.Play();

        isAirbone = false;
        canDoubleJump = true;

        AttemptBufferJump();
    }

    private void HandleInput()
    {
        //xInput = Input.GetAxisRaw("Horizontal");
        //yInput = Input.GetAxisRaw("Vertical");

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    JumpButton();
        //    RequestBufferJump();
        //}
    }

    private void RequestBufferJump()
    {
        if (isAirbone)
            bufferJumpActivated = Time.time;
    }

    private void AttemptBufferJump()
    {
        if(Time.time < bufferJumpActivated + bufferJumpWindow)
        {
            bufferJumpActivated = Time.time - 1;
            Jump();
        }
    }

    private void ActivateCoyoteJump() => coyoteJumpActivated = Time.time;

    private void CancelCoyoteJump() => coyoteJumpActivated = Time.time - 1;

    private void JumpButton()
    {
        bool coyoteJumpAvailable = Time.time < coyoteJumpActivated + coyoteJumpWindow;

        if (isGrounded || coyoteJumpAvailable)
        {
            Jump();
        }
        else if (isWallDetected && !isGrounded)
        {
            WallJump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }
        CancelCoyoteJump();
    }

    private void Jump()
    {
        dustFx.Play();
        AudioManager.instance.PlaySFX(3);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void DoubleJump()
    {
        dustFx.Play();
        AudioManager.instance.PlaySFX(3);

        isWallJumping = false;
        canDoubleJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
    }

    private void WallJump()
    {
        dustFx.Play();
        AudioManager.instance.PlaySFX(12);

        canDoubleJump = true;
        rb.linearVelocity = new Vector2(wallJumpForce.x * -facingDir, wallJumpForce.y);

        Flip();

        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
    }

    private IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;

        yield return new WaitForSeconds(wallJumpDuration);

        isWallJumping = false;
    }

    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.linearVelocity.y < 0;
        float yModifier = moveInput.y < 0 ? 1 : .05f;

        if (canWallSlide == false)
            return;

        dustFx.Play();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * yModifier);
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, 
            Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, 
            Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    private void HandleMovement()
    {
        if (isWallDetected)
            return;

        if (isWallJumping)
            return;

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleFlip()
    {
        if(moveInput.x < 0 && facingRight || moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
    }
}

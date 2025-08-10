using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Header("Bat Details")]
    [SerializeField] private float agroRadius;
    [SerializeField] private float chaseDuration;
    [SerializeField] private float attackSpeed;

    private float defaultSpeed;
    private float chaseTimer;

    private Vector3 originPosition;
    private Vector3 destination;

    private bool canDetectPlayer;
    private Collider2D target;

    protected override void Awake()
    {
        base.Awake();

        defaultSpeed = moveSpeed;
        originPosition = transform.position;
        canMove = false;
    }

    protected override void Update()
    {
        base.Update();

        chaseTimer -= Time.deltaTime;

        if (idleTimer <  0 )
            canDetectPlayer = true;

        HandleMovement();
        HandlePlayerDetection();
    }

    private void HandleMovement()
    {
        if (canMove == false)
        {
            return;
        }

        HandleFlip(destination.x);
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (chaseTimer > 0 & target != null)
            destination = target.transform.position;
        else
            moveSpeed = attackSpeed;

        if (Vector2.Distance(transform.position, destination) < .1f)
        {
            if (destination == originPosition)
            {
                idleTimer = idleDuration;
                canDetectPlayer = false;
                canMove = false;
                anim.SetBool("isMoving", false);
                target = null;
                moveSpeed = defaultSpeed;
            }
            else
            {
                destination = originPosition;
            }
        }
    }

    private void HandlePlayerDetection()
    {
        if (target == null && canDetectPlayer)
        {
            target = Physics2D.OverlapCircle(transform.position, agroRadius, whatIsPlayer);

            if (target != null)
            {
                chaseTimer = chaseDuration;
                destination = target.transform.position;
                canDetectPlayer = false;
                anim.SetBool("isMoving", true);
            }
        }
    }

    private void AllowMovement() => canMove = true;

    protected override void HandleAnimator()
    {
        
    }

    public override void Die()
    {
        base.Die();

        canMove = false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, agroRadius);
    }
}

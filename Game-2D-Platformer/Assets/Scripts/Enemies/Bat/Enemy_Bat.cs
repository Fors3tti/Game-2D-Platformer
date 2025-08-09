using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Header("Bat Details")]
    [SerializeField] private float agroRadius;

    private Vector3 originPosition;
    private Vector3 destination;

    public bool canDetectPlayer;
    public Collider2D target;

    protected override void Awake()
    {
        base.Awake();

        originPosition = transform.position;
        canMove = false;
    }

    protected override void Update()
    {
        base.Update();

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

        if (Vector2.Distance(transform.position, destination) < .1f)
        {
            if (destination == originPosition)
            {
                idleTimer = idleDuration;
                canDetectPlayer = false;
                canMove = false;
                target = null;
                anim.SetBool("isMoving", false);
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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, agroRadius);
    }
}

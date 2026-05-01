using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float stopDistance = 10f;
    [SerializeField] private Transform target;

    bool canMove = true;
    bool canFlip = true;
    bool isHandlingAttack = false;

    void Awake()
    {
        findTarget();
    }

    void Update()
    {
        if (target == null) return;
        updateMovementState();
        move();
        flip();
    }

    void OnEnable()
    {
        BossAnimationController.isHandlingAttack += setIsHandlingAttack;
        BossHealth.OnDied += HandleDead;
    }

    void OnDisable()
    {
        BossAnimationController.isHandlingAttack -= setIsHandlingAttack;
        BossHealth.OnDied -= HandleDead;
    }

    void HandleDead(bool isDead)
    {
        canMove = false;
        canFlip = false;
    }

    void setIsHandlingAttack(bool value)
    {
        if (value && !isHandlingAttack)
            flip();

        isHandlingAttack = value;
    }

    void move()
    {
        if (!canMove) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );
    }

    void findTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            target = player.transform;
    }

    void updateMovementState()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        canMove = distanceToTarget > stopDistance;
    }

    void flip()
    {
        if (!canFlip || isHandlingAttack) return;

        float directionX = target.position.x > transform.position.x ? -1 : 1;
        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * directionX,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}
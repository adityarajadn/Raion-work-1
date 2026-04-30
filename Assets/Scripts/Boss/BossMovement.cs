using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Transform target;

    bool canMove = true;
    bool canFlip = true;
    float distanceToTarget;

    public float stopDistance = 10f;

    bool isHandlingAttack = false;

    void Awake()
    {
        findTarget();
    }

    void Update()
    {
        countDistance();   // update jarak terus
        move();
        flip(isHandlingAttack);
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
        // hanya saat mulai attack (false -> true)
        if (value && !isHandlingAttack)
        {
            flip(isHandlingAttack);
        }

        isHandlingAttack = value;
    }

    void move()
    {
        if (target == null || !canMove) return;
        checkToTarget();
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

    void countDistance()
    {
        if (target == null) return;

        distanceToTarget = Vector2.Distance(
            transform.position,
            target.position
        );
    }

    void checkToTarget()
    {
        if (target == null) return;

        if (distanceToTarget <= stopDistance)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    void flip(bool isHandlingAttack)
    {
        if (!canFlip) return;
        if (target == null) return;
        if (isHandlingAttack) return;

        Vector3 scale = transform.localScale; // ambil scale sekarang

        if (transform.position.x < target.position.x)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}
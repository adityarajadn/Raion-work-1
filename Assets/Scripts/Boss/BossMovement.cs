using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Transform target;

    bool canMove = true;
    float distanceToTarget;

    public float stopDistance = 10f;

    bool isHandlingAttack = false;

    BossAnimationController bossAnimationController;

    void Awake()
    {
        if (bossAnimationController == null)
            bossAnimationController = GetComponent<BossAnimationController>();
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
        bossAnimationController.isHandlingAttack += setIsHandlingAttack;
    }

    void OnDisable()
    {
        bossAnimationController.isHandlingAttack -= setIsHandlingAttack;
    }

    void setIsHandlingAttack(bool value)
    {
        // hanya saat mulai attack (false -> true)
        if (value && !isHandlingAttack)
        {
            FaceTargetOnce();
        }

        isHandlingAttack = value;
    }

    void FaceTargetOnce()
    {
        if (target == null) return;

        Vector3 scale = transform.localScale;

        if (transform.position.x < target.position.x)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    void move()
    {
        checkToTarget();

        if (target == null || !canMove) return;

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
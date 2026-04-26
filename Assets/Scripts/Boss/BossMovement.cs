using System;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Rigidbody2D rb;
    public Transform target;

    bool canMove = true;
    float distanceToTarget;

    public float stopDistance = 10f;

    public event Action<bool> OnMoving;
    public event Action<bool> CanAttack;

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        findTarget();
    }

    void Update()
    {
        countDistance();   // update jarak terus
        move();
        flip();
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

        OnMoving?.Invoke(true);
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
            OnMoving?.Invoke(false);
            CanAttack?.Invoke(true);
        }
        else
        {
            canMove = true;
            CanAttack?.Invoke(false);
        }
    }

    void flip()
    {
        if (target == null) return;

        Vector3 scale = transform.localScale;

        if (transform.position.x < target.position.x)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}
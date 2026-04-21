using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Collider2D hitBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitBox.enabled = false;
    }

    void Update()
    {
        handleInput();
    }

    void handleInput()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            hitBox.enabled = true;
        } else
        {
            hitBox.enabled = false;
        }
    }
}

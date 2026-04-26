using System;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    public float damage = 20f;
    public event Action<float> OnAttack;

    

}

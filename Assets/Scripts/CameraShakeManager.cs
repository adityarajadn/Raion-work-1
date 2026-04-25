using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeManager : MonoBehaviour, ICameraImpulseShaker
{
    public static CameraShakeManager instance;

    public PlayerHealth playerHealth;
    public float globalShakeForce = 1f;
    public float GlobalShakeForce => globalShakeForce;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (playerHealth == null)
        {
            playerHealth = FindAnyObjectByType<PlayerHealth>();
        }
    }

    void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.Damaged += HandlePlayerDamaged;
        }
    }

    void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.Damaged -= HandlePlayerDamaged;
        }
    }

    void HandlePlayerDamaged(float current, float max)
    {
        if (playerHealth == null || playerHealth.impulseSource == null)
        {
            return;
        }

        CameraShake(playerHealth.impulseSource);
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(GlobalShakeForce);
    }
}

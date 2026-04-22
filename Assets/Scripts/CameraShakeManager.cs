using UnityEngine;
using Unity.Cinemachine;
public class CameraShakeManager: MonoBehaviour
{
    public static CameraShakeManager instance;
    public float globalShakeForce = 1f;
    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }
}

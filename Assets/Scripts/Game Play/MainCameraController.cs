using Unity.Cinemachine;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private CinemachineCamera cam;
    private void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
        Observer.AddObserver(GameEvent.OnGameOver, DisableFollowingTarget);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameOver, DisableFollowingTarget);
    }

    void DisableFollowingTarget(object[] datas)
    {
        cam.Follow = null;
        cam.LookAt = null;
    }

}

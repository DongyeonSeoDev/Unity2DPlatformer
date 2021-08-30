using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;
    private PlayerMove playerMove = null;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        playerMove = FindObjectOfType<PlayerMove>();

        cinemachineVirtualCamera.Follow = playerMove.transform;
    }
}

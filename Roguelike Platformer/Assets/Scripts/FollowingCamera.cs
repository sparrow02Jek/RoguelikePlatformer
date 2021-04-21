using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime;

    private Vector3 _velocity = Vector3.zero;

    private void LateUpdate()
    {
        var playerPosition = player.position; //cached
        var cameraPosition = transform.position; //cached
        Vector3 targetPosition = new Vector3(playerPosition.x, playerPosition.y, cameraPosition.z);
        transform.position = Vector3.SmoothDamp(cameraPosition, targetPosition, ref _velocity, smoothTime);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform followTarget;
    void Update()
    {
        Vector3 newPos = new Vector3(followTarget.position.x, followTarget.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}

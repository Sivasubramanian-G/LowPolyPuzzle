using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    public Vector3 distance;
    public float smooth = 1.0f;

    void Start()
    {
        //distance = this.transform.position - player.transform.position;
        Screen.SetResolution((int)Screen.width, (int)Screen.height, true);
    }

    void LateUpdate()
    {
        //Vector3 targetPosition = new Vector3(player.position.x + distance.x, this.transform.position.y, player.position.z + distance.y);
        //transform.position = Vector3.Lerp(this.transform.position, targetPosition, smooth);
    }
}

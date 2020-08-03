using System;
using System.Linq;
using UnityEngine;

public class MovableObjs : MonoBehaviour
{
    public PlayerMovements playerMov = null;
    public Vector3 relativePosition, distance, targetPosition;
    public bool canMove = false, havePlayer = false;

    void Start()
    {
        targetPosition = this.transform.position;
        relativePosition = this.transform.position;
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(this.transform.position, 4f);
    }

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 2.5f);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.name == "Player")
            {
                canMove = true;
                distance = this.transform.position - playerMov.transform.position;
                relativePosition = playerMov.CalRelPos(this.transform, hitCollider.transform.position);
            }
        }

        if (canMove)
        {
            if (Math.Abs(relativePosition.x) > Math.Abs(relativePosition.z))
            {
                targetPosition.x = playerMov.transform.position.x + distance.x;
            }
            else if (Math.Abs(relativePosition.z) > Math.Abs(relativePosition.x))
            {
                targetPosition.z = playerMov.transform.position.z + distance.z;
            }
            transform.position = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
        }
    }
}

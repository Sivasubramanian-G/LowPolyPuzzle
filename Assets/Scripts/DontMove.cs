using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DontMove : MonoBehaviour
{
    public float distance = 100;
    public PlayerMovements movement;
    void Update()
    {
        Vector3 dirF = this.transform.TransformDirection(Vector3.forward);
        Vector3 dirB = this.transform.TransformDirection(Vector3.back);
        Vector3 dirR = this.transform.TransformDirection(Vector3.right);
        Vector3 dirL = this.transform.TransformDirection(Vector3.left);

        Debug.DrawRay(this.transform.position, dirF * distance, Color.blue);
        Debug.DrawRay(this.transform.position, dirB * distance, Color.yellow);
        Debug.DrawRay(this.transform.position, dirR * distance, Color.green);
        Debug.DrawRay(this.transform.position, dirL * distance, Color.red);
        RaycastHit hit;

        try
        {
            if (Physics.Raycast(transform.position, dirF, out hit, distance))
            {
                if (hit.collider.transform.parent.name == "NonTileParent")
                {
                    Debug.Log("It's a hit!");
                    movement.canMove = false;
                }
            }
            if (Physics.Raycast(transform.position, dirB, out hit, distance))
            {
                if (hit.collider.transform.parent.name == "NonTileParent")
                {
                    Debug.Log("It's a hit!");
                    movement.canMove = false;
                    movement.canRotateB = false;
                }
            }
            if (Physics.Raycast(transform.position, dirR, out hit, distance))
            {
                if (hit.collider.transform.parent.name == "NonTileParent")
                {
                    Debug.Log("It's a hit!");
                    movement.canMove = false;
                    movement.canRotateR = false;
                }
            }
            if (Physics.Raycast(transform.position, dirL, out hit, distance))
            {
                if (hit.collider.transform.parent.name == "NonTileParent")
                {
                    Debug.Log("It's a hit!");
                    movement.canMove = false;
                    movement.canRotateL = false;
                }
            }
        }
        catch (Exception)
        {

        }
    }
}

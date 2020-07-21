using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public Animator anim;
    private Quaternion targetRotation;
    public float smooth;
    public Camera cam = null;
    public float speed = 12f;
    public Rigidbody rb;
    private Vector3 targetPosition;
    private Vector3 relativePosition;
    public bool canMove = true;
    public bool canRotateR = true;
    public bool start;
    public bool runAnim = false;
    public float distance = 100;

    private void Start()
    {
        start = true;
        targetRotation = this.transform.rotation;
        targetPosition = this.transform.position;
        rb = GetComponent<Rigidbody>();
        anim.speed = 5f;
    }

    private void Update()
    {
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("TurnRight") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("TurnLeft") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("TurnAround"))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        Vector3 dir = this.transform.TransformDirection(Vector3.right);

        Debug.DrawRay(this.transform.position, dir * distance, Color.red);
        RaycastHit hitR;

        if (Physics.Raycast(transform.position, dir, out hitR, distance))
        {
            try
            {
                if (hitR.collider.transform.parent.name == "NonTileParent")
                {
                    canRotateR = false;
                }
            }
            catch (Exception)
            {
                canMove = true;
                canRotateR = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            start = false;
            canMove = false;
            runAnim = true;
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    try
                    {
                        if (hit.collider.transform.parent.name == "TileParent")
                        {
                            targetPosition = hit.collider.transform.position;

                            Vector3 distance = targetPosition - this.transform.position;
                            relativePosition = Vector3.zero;
                            relativePosition.x = Vector3.Dot(distance, this.transform.right.normalized);
                            relativePosition.y = Vector3.Dot(distance, this.transform.up.normalized);
                            relativePosition.z = Vector3.Dot(distance, this.transform.forward.normalized);

                            targetPosition.y = this.transform.position.y;

                            if (Math.Abs(distance.x) > Math.Abs(distance.z))
                            {
                                targetPosition.z = this.transform.position.z;
                            }
                            else
                            {
                                targetPosition.x = this.transform.position.x;
                            }

                            if (Math.Abs(relativePosition.x) > Math.Abs(relativePosition.y))
                            {
                                if (relativePosition.x > 0.0)
                                {
                                    anim.Play("TurnRight");
                                    smooth = 0.35f;
                                    targetRotation *= Quaternion.AngleAxis(90, Vector3.forward);

                                }
                                else
                                {
                                    anim.Play("TurnLeft");
                                    smooth = 0.35f;
                                    targetRotation *= Quaternion.AngleAxis(-90, Vector3.forward);
                                }
                            }
                            else if (Math.Abs(relativePosition.y) > Math.Abs(relativePosition.x))
                            {
                                if (relativePosition.y > 0.0)
                                {
                                    anim.Play("TurnAround");
                                    smooth = 0.5f;
                                    targetRotation *= Quaternion.AngleAxis(180, Vector3.forward);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        targetPosition = this.transform.position;
                    }
                }
            }
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 20 * smooth * Time.deltaTime);

        if (canMove && !start)
        {
            if (this.transform.position != targetPosition && runAnim)
            {
                anim.Play("RunStart");
                anim.SetBool("RunLoopStop", false);
                runAnim = false;
            }
            if (this.transform.position == targetPosition && !runAnim)
            {
                anim.SetBool("RunLoopStop", true);
            }
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public Quaternion targetRotation;

    public Animator anim;

    public Camera cam = null;
    
    public Rigidbody rb;

    public Vector3 targetPosition;
    public Vector3 relativePosition;
    public Vector3 finalTargetPosition;

    public Vector3 Pos;
    public Vector3 dirF, dirB, dirL, dirR;

    public bool canMove = true;
    public bool canRotateR = true;
    public bool canRotateL = true;
    public bool start;
    public bool runAnim = false;
    public bool canClick = true;
    public bool canInstance = true;

    public float dist = 100;
    public float speed = 12f;
    public float smooth;

    private void Start()
    {
        start = true;
        targetRotation = this.transform.rotation;
        targetPosition = this.transform.position;
        finalTargetPosition = this.transform.position;
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
            canRotateL = true;
            canRotateR = true;

            if (anim.GetBool("RunLoopStop") == true && canInstance)
            {
                Pos = new Vector3(this.transform.position.x, this.transform.position.y - (this.GetComponent<Collider>().bounds.size.y) / 1.5f, this.transform.position.z + (this.GetComponent<Collider>().bounds.size.z) / 1.5f);
                dirF = this.transform.TransformDirection(-Vector3.up);
                Debug.DrawRay(Pos, dirF * dist, Color.blue);
                RaycastHit[] hitF;

                hitF = Physics.RaycastAll(Pos, dirF, dist);

                for (int i = 0; i < hitF.Length; i++)
                {
                    RaycastHit hit = hitF[i];
                    Debug.Log("Hit: " + hit.collider.name);
                }
                canInstance = false;
            }
        }

        if (Input.GetMouseButtonDown(0) && canClick)
        {
            start = false;
            canMove = false;
            runAnim = true;
            canInstance = true;

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
                                    if (canRotateR)
                                    {
                                        anim.Play("TurnRight");
                                        smooth = 0.35f;
                                        targetRotation *= Quaternion.AngleAxis(90, Vector3.forward);
                                    }
                                    else
                                    {
                                        runAnim = false;
                                        targetPosition = this.transform.position;
                                    }
                                }
                                if (relativePosition.x < 0.0)
                                {
                                    if (canRotateL)
                                    {
                                        anim.Play("TurnLeft");
                                        smooth = 0.35f;
                                        targetRotation *= Quaternion.AngleAxis(-90, Vector3.forward);
                                    }
                                    else
                                    {
                                        runAnim = false;
                                        targetPosition = this.transform.position;
                                    }
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
                        anim.SetBool("RunLoopStop", false);
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
                //anim.SetBool("RunLoopStop", false);
                canClick = false;
                runAnim = false;
            }
            if (this.transform.position == targetPosition && !runAnim)
            {
                anim.SetBool("RunLoopStop", true);
                canClick = true;
            }
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));
        }
    }
}
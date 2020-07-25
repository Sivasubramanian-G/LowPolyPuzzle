using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NavMovement : MonoBehaviour
{

    public NavMeshAgent agent;
    public Camera cam;
    public Vector3 targetPosition;
    public Animator anim;
    public GameObject player;
    public bool tap = false;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        targetPosition = this.transform.position;
        anim = player.GetComponent<Animator>();
        anim.speed = 2f;
    }

    void Update()
    {
        /*Debug.Log(agent.velocity);

        if (Math.Abs(agent.velocity.x) > 1)
        {
            if (agent.velocity.x < 0)
            {
                anim.Play("TurnLeft");
            }
            else
            {
                anim.Play("TurnRight");
            }
        }

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("TurnRight") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("TurnLeft"))
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }*/

        if (Input.GetMouseButtonDown(0))
        {
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
                            tap = true;
                        }
                    }
                    catch (Exception)
                    {
                        targetPosition = this.transform.position;
                        tap = false;
                    }
                }   
            }
            agent.SetDestination(targetPosition);

            /*if (agent.nextPosition.x > 0 && agent.nextPosition.x < 1)
            {
                agent.nextPosition.Set(0, targetPosition.y, targetPosition.z);
            }*/

        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            if (tap)
            {
                anim.Play("RunStart");
                anim.SetBool("RunLoopStop", false);
                tap = false;
            }
            /*Vector3 s = agent.transform.InverseTransformDirection(agent.velocity).normalized;
            float turn = s.x;
            if (turn > 0)
            {
                anim.Play("TurnRight");
            }
            else if (turn < 0)
            {
                anim.Play("TurnLeft");
            }*/

        }
        else
        {
            anim.SetBool("RunLoopStop", true);
        }


    }

    private float normalize(Vector3 velocity)
    {
        throw new NotImplementedException();
    }
}

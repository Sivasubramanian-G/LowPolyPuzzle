using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMovement : MonoBehaviour
{

    public NavMeshAgent agent;
    public Camera cam;
    public Vector3 targetPosition;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
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
                        }
                    }
                    catch (Exception)
                    {
                        targetPosition = this.transform.position;
                    }
                }   
            }
            agent.SetDestination(targetPosition);
        }
    }
}

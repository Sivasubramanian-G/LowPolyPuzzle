﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMechanism : MonoBehaviour
{

    public PlayerMovements playerMov = null;

    public GameObject leverChange;
    public GameObject leverChangeNonObj;

    public float speed = 8;

    public bool canMove = false;

    public Camera cam = null;

    [HideInInspector]
    public Vector3 targetPosition;

    [HideInInspector]
    public Collider[] hitColliders;
    public Ray ray;
    public RaycastHit hit;

    void Start()
    {
        targetPosition = new Vector3(leverChange.transform.position.x, leverChange.GetComponent<DragObject>().initialPos.y, leverChange.transform.position.z);
    }

    void Update()
    {
        hitColliders = Physics.OverlapSphere(this.transform.position, 2.5f);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.name == "Player" && playerMov.anim.GetBool("RunLoopStop"))
            {
                if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !PauseMenu.gamePaused)
                {
                    ray = cam.ScreenPointToRay(Input.touches[0].position);
                    if (Physics.Raycast(ray, out hit, 100f))
                    {
                        if (hit.collider != null)
                        {
                            if (hit.collider.transform.parent.name == "Levers")
                            {
                                canMove = true;
                                if (targetPosition.y == leverChange.GetComponent<DragObject>().initialPos.y)
                                {
                                    targetPosition.y = leverChange.GetComponent<DragObject>().maxHeight;
                                }
                                else if (targetPosition.y == leverChange.GetComponent<DragObject>().maxHeight)
                                {
                                    targetPosition.y = leverChange.GetComponent<DragObject>().initialPos.y;
                                }
                            }
                            else
                            {
                                canMove = false;
                            }
                        }
                    }
                }
            }
        }

        if (canMove)
        {
            leverChange.transform.position = Vector3.Lerp(leverChange.transform.position, new Vector3(leverChange.transform.position.x, targetPosition.y, leverChange.transform.position.z), speed * Time.deltaTime);
            leverChangeNonObj.transform.position = Vector3.Lerp(leverChangeNonObj.transform.position, new Vector3(leverChange.transform.position.x, targetPosition.y - leverChange.GetComponent<Collider>().bounds.size.y * 1.5f, leverChange.transform.position.z), speed * Time.deltaTime);
        }
    }
}

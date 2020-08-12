using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMechanism : MonoBehaviour
{

    public PlayerMovements playerMov = null;

    public GameObject leverChange;
    public GameObject leverChangeNonObj;

    public float speed = 8;

    public bool start = true, havePlayer = false;

    public Camera cam = null;

    [HideInInspector]
    public Vector3 targetPosition;

    [HideInInspector]
    public Collider[] hitColliders;
    public Ray ray;
    public RaycastHit hit;

    void Update()
    {
        hitColliders = Physics.OverlapSphere(this.transform.position, 2.5f);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.name == "Player" && playerMov.anim.GetBool("RunLoopStop"))
            {
                if (start)
                {
                    targetPosition = new Vector3(leverChange.transform.position.x, leverChange.GetComponent<DragObject>().initialPos.y, leverChange.transform.position.z);
                    start = false;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100f))
                    {
                        if (hit.collider != null)
                        {
                            if (hit.collider.transform.parent.name == "Levers")
                            {
                                havePlayer = true;
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
                                havePlayer = false;
                            }
                        }
                    }
                }
            }
        }

        if (!start && havePlayer)
        {
            leverChange.transform.position = Vector3.Lerp(leverChange.transform.position, new Vector3(leverChange.transform.position.x, targetPosition.y, leverChange.transform.position.z), speed * Time.deltaTime);
            leverChangeNonObj.transform.position = Vector3.Lerp(leverChangeNonObj.transform.position, new Vector3(leverChange.transform.position.x, targetPosition.y - leverChange.GetComponent<Collider>().bounds.size.y * 1.5f, leverChange.transform.position.z), speed * Time.deltaTime);
        }
    }
}

using System;
using System.Linq;
using UnityEngine;

public class MovableObjs : MonoBehaviour
{
    public PlayerMovements playerMov = null;
    public Vector3 relativePosition, distance, targetPosition;
    public bool canMove = false, havePlayer = false;
    public Vector3 dir, dir1, pos;
    public float dist = 100f;
    public RaycastHit[] hit, hit1, hits, hits1;
    public RaycastHit hitM;
    public Ray ray;
    public Camera cam = null;
    public Collider[] hitColliders;

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
        hitColliders = Physics.OverlapSphere(this.transform.position, 2.5f);

        pos = new Vector3(this.transform.position.x, this.transform.position.y - (this.GetComponent<Collider>().bounds.size.y) / 1.5f, this.transform.position.z + (this.GetComponent<Collider>().bounds.size.z) / 1.5f);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.name == "Player")
            {
                canMove = true;
                distance = this.transform.position - playerMov.transform.position;
                relativePosition = playerMov.CalRelPos(this.transform, hitCollider.transform.position);
                if (Math.Abs(relativePosition.x) > Math.Abs(relativePosition.z))
                {
                    dir = this.transform.TransformDirection(Vector3.right);
                    dir1 = this.transform.TransformDirection(Vector3.left);
                }
                else if (Math.Abs(relativePosition.z) > Math.Abs(relativePosition.x))
                {
                    dir = this.transform.TransformDirection(Vector3.forward);
                    dir1 = this.transform.TransformDirection(Vector3.back);
                }
            }
        }

        Debug.DrawRay(pos, dir * dist, Color.blue);
        Debug.DrawRay(pos, dir1 * dist, Color.red);

        hit = Physics.RaycastAll(pos, dir, dist).OrderBy(h => h.distance).ToArray();
        hit1 = Physics.RaycastAll(pos, dir1, dist).OrderBy(h => h.distance).ToArray();

        for (int i = 0; i < hit.Length; i++)
        {
            RaycastHit hitr = hit[i];
            if (hitr.collider.transform.parent.name == "NonTileParent")
            {
                break;
            }
            //hits[i] = hitr;
        }

        for (int i = 0; i < hit1.Length; i++)
        {
            RaycastHit hit = hit1[i];
            if (hit.collider.transform.parent.name == "NonTileParent")
            {
                break;
            }
            //hits1[i] = hit;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && playerMov.canClick)
        {
            if (Physics.Raycast(ray, out hitM, dist))
            {
                if (hitM.collider != null)
                {
                    try
                    {
                        if (hitM.collider.transform.parent.name == "TileParent")
                        {
                            foreach (var hit in hits)
                            {
                                if (hit.collider.name == hitM.collider.name)
                                {
                                    targetPosition = hitM.collider.transform.position;
                                    targetPosition.y = this.transform.position.y;
                                }
                            }
                            foreach (var hit in hits1)
                            {
                                if (hit.collider.name == hitM.collider.name)
                                {
                                    targetPosition = hitM.collider.transform.position;
                                    targetPosition.y = this.transform.position.y;
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

        if (canMove)
        {
            /*if (Math.Abs(relativePosition.x) > Math.Abs(relativePosition.z))
            {
                targetPosition.x = playerMov.transform.position.x + distance.x;
            }
            else if (Math.Abs(relativePosition.z) > Math.Abs(relativePosition.x))
            {
                targetPosition.z = playerMov.transform.position.z + distance.z;
            }*/
            transform.position = targetPosition;
        }

        /*if (canMove)
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
        }*/
    }
}

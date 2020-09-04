using System;
using System.Linq;
using UnityEngine;

public class MovableObjs : MonoBehaviour
{
    public PlayerMovements playerMov = null;

    public Camera cam = null;

    public GameObject nonTileDragObj = null;

    public float speed = 1f;

    public Animator anim = null;

    [HideInInspector]
    public Vector3 relativePosition, distance, targetPosition, screenPoint, offset, dir, dir1, dirD;

    [HideInInspector]
    public RaycastHit[] hit, hit1, hits;

    [HideInInspector]
    public Collider[] hitColliders;

    [HideInInspector]
    public bool canDrag = false, lefR = false, forB = false, canMove = false, isMoveObj = false;

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

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.name == "Player" && playerMov.anim.GetBool("RunLoopStop"))
            {
                canDrag = true;
                relativePosition = playerMov.CalRelPos(this.transform, hitCollider.transform.position);
                if (Math.Abs(relativePosition.x) > Math.Abs(relativePosition.z))
                {
                    lefR = true;
                    forB = false;

                    if (relativePosition.x > 0.0)
                    {
                        dir = this.transform.TransformDirection(Vector3.right);
                        dir1 = this.transform.TransformDirection(Vector3.left);
                    }
                    else if (relativePosition.x < 0.0)
                    {
                        dir = this.transform.TransformDirection(Vector3.left);
                        dir1 = this.transform.TransformDirection(Vector3.right);
                    }

                }
                else if (Math.Abs(relativePosition.z) > Math.Abs(relativePosition.x))
                {
                    lefR = false;
                    forB = true;

                    if (relativePosition.z > 0.0)
                    {
                        dir = this.transform.TransformDirection(Vector3.forward);
                        dir1 = this.transform.TransformDirection(Vector3.back);
                    }
                    else if (relativePosition.z < 0.0)
                    {
                        dir = this.transform.TransformDirection(Vector3.back);
                        dir1 = this.transform.TransformDirection(Vector3.forward);
                    }

                }
            }
        }

        if (Input.touchCount > 0 && !PauseMenu.gamePaused)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    if (hit.collider != null && hit.collider.transform.parent.name == "MovableObjs")
                    {
                        isMoveObj = true;
                        playerMov.canClick = false;
                        distance = this.transform.position - playerMov.transform.position;
                        playerMov.canMove = false;
                        playerMov.DestroyInsts();
                        screenPoint = cam.WorldToScreenPoint(gameObject.transform.position);
                        offset = gameObject.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                        if (!canDrag)
                        {
                            playerMov.InstObjs();
                        }
                        else
                        {
                            anim.Play("RunStart");
                            anim.SetBool("RunLoopStop", false);
                        }
                        playerMov.GetComponent<PlayerMovements>().enabled = false;
                    }
                    else
                    {
                        isMoveObj = false;
                    }
                }
            }
            if (Input.touches[0].phase == TouchPhase.Moved && isMoveObj)
            {
                playerMov.canClick = false;
                Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 cursorPosition = cam.ScreenToWorldPoint(cursorPoint) + offset;

                Debug.DrawRay(this.transform.position, dir * 5f, Color.red);
                Debug.DrawRay(this.transform.position, dir1 * 2.5f, Color.green);

                if (canDrag)
                {
                    dirD = this.transform.TransformDirection(Vector3.down);
                    hits = Physics.RaycastAll(this.transform.position, dirD, 2.5f).OrderBy(h => h.distance).ToArray();

                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider.transform.parent.name == "TileParent")
                        {
                            float dist = hits[i].collider.bounds.size.x;
                            hit = Physics.RaycastAll(hits[i].collider.transform.position, dir, dist * 2f).OrderBy(h => h.distance).ToArray();
                            hit1 = Physics.RaycastAll(hits[i].collider.transform.position, dir1, dist).OrderBy(h => h.distance).ToArray();

                            for (int j = 0; j < hit.Length; j++)
                            {
                                if (hit[j].collider.transform.parent.name == "TileParent")
                                {
                                    continue;
                                }
                                else if (hit[j].collider.transform.parent.name == "OuterRegion")
                                {
                                    if (lefR)
                                    {
                                        if (dir == this.transform.TransformDirection(Vector3.right))
                                        {
                                            if (cursorPosition.x > hits[i].collider.transform.position.x)
                                            {
                                                cursorPosition.x = hits[i].collider.transform.position.x;
                                            }
                                        }
                                        else
                                        {
                                            if (cursorPosition.x < hits[i].collider.transform.position.x)
                                            {
                                                cursorPosition.x = hits[i].collider.transform.position.x;
                                            }
                                        }

                                    }
                                    else if (forB)
                                    {
                                        if (dir == this.transform.TransformDirection(Vector3.forward))
                                        {
                                            if (cursorPosition.z > hits[i].collider.transform.position.z)
                                            {
                                                cursorPosition.z = hits[i].collider.transform.position.z;
                                            }
                                        }
                                        else
                                        {
                                            if (cursorPosition.z < hits[i].collider.transform.position.z)
                                            {
                                                cursorPosition.z = hits[i].collider.transform.position.z;
                                            }
                                        }
                                    }
                                }
                            }

                            for (int j = 0; j < hit1.Length; j++)
                            {
                                if (hit1[j].collider.transform.parent.name == "TileParent")
                                {
                                    continue;
                                }
                                else if (hit1[j].collider.transform.parent.name == "OuterRegion")
                                {
                                    if (lefR)
                                    {
                                        if (dir1 == this.transform.TransformDirection(Vector3.right))
                                        {
                                            if (cursorPosition.x > hits[i].collider.transform.position.x)
                                            {
                                                cursorPosition.x = hits[i].collider.transform.position.x;
                                            }
                                        }
                                        else
                                        {
                                            if (cursorPosition.x < hits[i].collider.transform.position.x)
                                            {
                                                cursorPosition.x = hits[i].collider.transform.position.x;
                                            }
                                        }
                                    }
                                    else if (forB)
                                    {
                                        if (dir1 == this.transform.TransformDirection(Vector3.forward))
                                        {
                                            if (cursorPosition.z > hits[i].collider.transform.position.z)
                                            {
                                                cursorPosition.z = hits[i].collider.transform.position.z;
                                            }
                                        }
                                        else
                                        {
                                            if (cursorPosition.z < hits[i].collider.transform.position.z)
                                            {
                                                cursorPosition.z = hits[i].collider.transform.position.z;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    if (lefR)
                    {
                        transform.position = new Vector3(cursorPosition.x, transform.position.y, transform.position.z);
                        playerMov.transform.position = new Vector3(gameObject.transform.position.x - distance.x, playerMov.transform.position.y, playerMov.transform.position.z);
                        nonTileDragObj.transform.position = new Vector3(cursorPosition.x, transform.position.y - this.GetComponent<Collider>().bounds.size.y * 1.5f, transform.position.z);
                    }
                    else if (forB)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, cursorPosition.z);
                        playerMov.transform.position = new Vector3(playerMov.transform.position.x, playerMov.transform.position.y, gameObject.transform.position.z - distance.z);
                        nonTileDragObj.transform.position = new Vector3(transform.position.x, transform.position.y - this.GetComponent<Collider>().bounds.size.y * 1.5f, cursorPosition.z);
                    }
                }
            }
            if (Input.touches[0].phase == TouchPhase.Ended && isMoveObj)
            {
                playerMov.GetComponent<PlayerMovements>().enabled = true;
                playerMov.canMove = true;
                playerMov.canClick = true;
                if (canDrag)
                {
                    playerMov.InstObjs();
                    anim.SetBool("RunLoopStop", true);
                }
                canDrag = false;
                targetPosition = this.transform.position;
                Vector3 dir = this.transform.TransformDirection(Vector3.down);
                Debug.DrawRay(this.transform.position, dir * 2.5f, Color.magenta);
                RaycastHit[] hit = Physics.RaycastAll(transform.position, dir, 2.5f);

                for (int i = 0; i < hit.Length; i++)
                {
                    RaycastHit hit1 = hit[i];
                    if (hit1.collider.transform.parent.name == "TileParent")
                    {
                        targetPosition = hit1.collider.transform.position;
                        targetPosition.y = this.transform.position.y;
                        canMove = true;
                    }
                }
            }
        }

        if (targetPosition == this.transform.position)
        {
            canMove = false;
        }
        if (canMove && !PauseMenu.gamePaused)
        {
            transform.position = Vector3.Lerp(this.transform.position, targetPosition, speed * Time.deltaTime);
            nonTileDragObj.transform.position = new Vector3(targetPosition.x, targetPosition.y - this.GetComponent<Collider>().bounds.size.y * 1.5f, targetPosition.z);
        }
    }
}
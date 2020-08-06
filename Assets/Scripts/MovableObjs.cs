using System;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    public Rigidbody rb;
    public float speed = 8f;


    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject nonTileDragObj = null;
    public bool canDrag = false, lefR = false, forB = false;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        targetPosition = this.transform.position;
        relativePosition = this.transform.position;
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(this.transform.position, 4f);
    }

    void Update()
    {
        Vector3 dir = this.transform.TransformDirection(Vector3.up);
        hitColliders = Physics.OverlapSphere(this.transform.position, 2.5f);

        //canDrag = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.name == "Player" && playerMov.anim.GetBool("RunLoopStop"))
            {
                canDrag = true;
                //distance = this.transform.position - playerMov.transform.position;
                relativePosition = playerMov.CalRelPos(this.transform, hitCollider.transform.position);
                if (Math.Abs(relativePosition.x) > Math.Abs(relativePosition.z))
                {
                    lefR = true;
                    forB = false;
                }
                else if (Math.Abs(relativePosition.z) > Math.Abs(relativePosition.x))
                {
                    lefR = false;
                    forB = true;
                }
            }
        }

        if (targetPosition == this.transform.position)
        {
            canMove = false;
        }
    }

    void OnMouseUp()
    {
        playerMov.GetComponent<PlayerMovements>().enabled = true;
        playerMov.canMove = true;
        playerMov.canClick = true;
        playerMov.InstObjs();
        canDrag = false;
        targetPosition = this.transform.position;
        Vector3 dir = this.transform.TransformDirection(Vector3.down);
        Debug.DrawRay(this.transform.position, dir * 2.5f, Color.magenta);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, 2.5f))
        {
            if (hit.collider.transform.parent.name == "TileParent")
            {
                targetPosition = hit.collider.transform.position;
                targetPosition.y = this.transform.position.y;
                canMove = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));
        }
    }

    void OnMouseDown()
    {
        distance = this.transform.position - playerMov.transform.position;
        playerMov.canMove = false;
        playerMov.canClick = false;
        playerMov.DestroyInsts();
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        playerMov.GetComponent<PlayerMovements>().enabled = false;
    }

    void OnMouseDrag()
    {
        playerMov.canClick = false;
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        
        if (canDrag)
        {
            if (lefR)
            {
                transform.position = new Vector3(cursorPosition.x, transform.position.y, transform.position.z);
                playerMov.transform.position = new Vector3(gameObject.transform.position.x - distance.x, playerMov.transform.position.y, playerMov.transform.position.z);
            }
            else if (forB)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, cursorPosition.z);
                playerMov.transform.position = new Vector3(playerMov.transform.position.x, playerMov.transform.position.y, gameObject.transform.position.z - distance.z);
            }
            //nonTileDragObj.transform.position = new Vector3(transform.position.x, cursorPosition.y - this.GetComponent<Collider>().bounds.size.y * 1.5f, transform.position.z);
        }
    }

    /*void Update()
    {
        hitColliders = Physics.OverlapSphere(this.transform.position, 2.5f);

        pos = new Vector3(this.transform.position.x, this.transform.position.y - (this.GetComponent<Collider>().bounds.size.y) / 1.5f, this.transform.position.z);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.name == "Player" && playerMov.anim.GetBool("RunLoopStop"))
            {
                havePlayer = true;
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

                hit = Physics.RaycastAll(pos, dir, dist).OrderBy(h => h.distance).ToArray();
                hit1 = Physics.RaycastAll(pos, dir1, dist).OrderBy(h => h.distance).ToArray();

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
                                    for (int i = 0; i < hit.Length; i++)
                                    {
                                        if (hit[i].collider.transform.parent.name == "NonTileParent")
                                        {
                                            break;
                                        }
                                        if (hit[i].collider.name == hitM.collider.name)
                                        {
                                            Debug.Log("Voila!");
                                            targetPosition = hit[i - 1].collider.transform.position;
                                            targetPosition.y = this.transform.position.y;
                                        }
                                    }
                                    for (int i = 0; i < hit1.Length; i++)
                                    {
                                        if (hit1[i].collider.transform.parent.name == "NonTileParent")
                                        {
                                            break;
                                        }
                                        if (hit1[i].collider.name == hitM.collider.name)
                                        {
                                            Debug.Log("Voila!");
                                            targetPosition = hit1[i - 1].collider.transform.position;
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
            }
        }

        Debug.DrawRay(pos, dir * dist, Color.blue);
        Debug.DrawRay(pos, dir1 * dist, Color.red);

        if (!havePlayer)
        {
            canMove = false;
        }

        

        if (canMove && this.transform.position != targetPosition)
        {
            if (Math.Abs(relativePosition.x) > Math.Abs(relativePosition.z))
            {
                targetPosition.x = playerMov.transform.position.x + distance.x;
            }
            else if (Math.Abs(relativePosition.z) > Math.Abs(relativePosition.x))
            {
                targetPosition.z = playerMov.transform.position.z + distance.z;
            }
            //transform.position = targetPosition + distance;
            //transform.position = new Vector3(playerMov.transform.position.x + distance.x, this.transform.position.y, playerMov.transform.position.z + distance.z);
            transform.position = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
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
        }
    }*/
}

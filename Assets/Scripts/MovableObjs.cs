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
    public float speed = 1f;


    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject nonTileDragObj = null;
    public bool canDrag = false, lefR = false, forB = false;

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
        Vector3 dir = this.transform.TransformDirection(Vector3.up);
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
        if (canMove)
        {
            transform.position = Vector3.Lerp(this.transform.position, targetPosition, speed * Time.deltaTime);
            nonTileDragObj.transform.position = new Vector3(targetPosition.x, targetPosition.y - this.GetComponent<Collider>().bounds.size.y * 1.5f, targetPosition.z);
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

    void FixedUpdate()
    {
        
    }

    void OnMouseDown()
    {
        playerMov.canClick = false;
        distance = this.transform.position - playerMov.transform.position;
        playerMov.canMove = false;
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
}

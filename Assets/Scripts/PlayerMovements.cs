using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public Animator anim;

    public Camera cam = null;
    
    public Rigidbody rb;

    public float dist = 100;
    public float speed = 8;

    public GameObject pathHighlight = null;

    public Puzzle puzzle = null;

    [HideInInspector]
    public RaycastHit[] hitF, hitB, hitL, hitR;
    [HideInInspector]
    public Ray ray;
    [HideInInspector]
    public RaycastHit hitM;

    [HideInInspector]
    public Vector3 targetPosition, relativePosition, pos, guidePos, dirF, dirB, dirL, dirR;

    [HideInInspector]
    public Quaternion targetRotation;

    [HideInInspector]
    public bool canMove = true, start = true, runAnim = false, canClick = true, canInstance = true, haveKey = false, hitTP = false;

    [HideInInspector]
    public float smooth;

    [HideInInspector]
    public string keyTag = null;

    private void Start()
    {
        start = true;
        targetRotation = this.transform.rotation;
        rb = GetComponent<Rigidbody>();
        anim.speed = 5f;
    }

    public void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 20 * smooth * Time.deltaTime);

        if (canMove && !start && !PauseMenu.gamePaused)
        {
            if (this.transform.position != targetPosition && runAnim)
            {
                anim.Play("RunStart");
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


    void OnEnable()
    {
        if (puzzle != null)
        {
            puzzle.enabled = false;
            puzzle.cam.enabled = false;
        }
        targetPosition = this.transform.position;
        ray = new Ray(this.transform.position, this.transform.TransformDirection(Vector3.back));
        Vector3 dir = this.transform.TransformDirection(Vector3.back);
        Debug.DrawRay(this.transform.position, dir * 2.5f, Color.magenta);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, 2.5f))
        {
            if (hit.collider.transform.parent.name == "TileParent")
            {
                targetPosition = hit.collider.transform.position;
                targetPosition.y = this.transform.position.y;
                runAnim = true;
                canMove = true;
            }
        }
    }

    IEnumerator WaitSecs()
    {
        yield return new WaitForSeconds(0.05f);

        if (Physics.Raycast(ray, out hitM, dist))
        {
            if (hitM.collider != null)
            {
                try
                {
                    if (hitM.collider.transform.parent.name == "TileParent" && hitM.transform.Find("PathHighlight(Clone)") != null)
                    {
                        canInstance = true;
                        start = false;
                        canMove = false;
                        runAnim = true;

                        DestroyInsts();

                        targetPosition = hitM.collider.transform.position;

                        relativePosition = CalRelPos(this.transform, targetPosition);

                        targetPosition.y = this.transform.position.y;

                        if (Math.Abs(relativePosition.x) > Math.Abs(relativePosition.y))
                        {
                            if (relativePosition.x > 0.0)
                            {
                                anim.Play("TurnRight");
                                smooth = 0.35f;
                                targetRotation *= Quaternion.AngleAxis(90, Vector3.forward);
                            }
                            if (relativePosition.x < 0.0)
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
                        anim.SetBool("RunLoopStop", false);
                    }
                }
                catch (Exception)
                {
                    targetPosition = this.transform.position;
                }
            }
        }
    }

    private void Update()
    {
        Vector3 dir = this.transform.TransformDirection(Vector3.back);
        Debug.DrawRay(this.transform.position, dir * 2.5f, Color.magenta);

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("TurnRight") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("TurnLeft") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("TurnAround"))
        {
            canMove = false;
        }
        else
        {
            canMove = true;

            if (canInstance)
            {
                InstObjs();
            }
        }


        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !PauseMenu.gamePaused)
        {
            ray = cam.ScreenPointToRay(Input.touches[0].position);
        }

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended && !PauseMenu.gamePaused)
        {
            StartCoroutine(WaitSecs());
        }
    }

    public Vector3 CalRelPos(Transform transformObj, Vector3 targetPosition)
    {
        Vector3 distance = targetPosition - transformObj.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, transformObj.right.normalized);
        relativePosition.y = Vector3.Dot(distance, transformObj.up.normalized);
        relativePosition.z = Vector3.Dot(distance, transformObj.forward.normalized);
        return relativePosition;
    }

    public void InstObjs()
    {
        pos = new Vector3(this.transform.position.x, this.transform.position.y - (this.GetComponent<Collider>().bounds.size.y) / 1.5f, this.transform.position.z);
        dirF = this.transform.TransformDirection(-Vector3.up);
        dirR = this.transform.TransformDirection(Vector3.right);
        dirL = this.transform.TransformDirection(Vector3.left);
        dirB = this.transform.TransformDirection(Vector3.up);

        Debug.DrawRay(pos, dirF * dist, Color.blue);
        Debug.DrawRay(pos, dirR * dist, Color.red);
        Debug.DrawRay(pos, dirL * dist, Color.green);
        Debug.DrawRay(pos, dirB * dist, Color.yellow);

        if (anim.GetBool("RunLoopStop") == true)
        {
            hitF = Physics.RaycastAll(pos, dirF, dist).OrderBy(h => h.distance).ToArray();
            hitB = Physics.RaycastAll(pos, dirB, dist).OrderBy(h => h.distance).ToArray();
            hitL = Physics.RaycastAll(pos, dirL, dist).OrderBy(h => h.distance).ToArray();
            hitR = Physics.RaycastAll(pos, dirR, dist).OrderBy(h => h.distance).ToArray();

            
            for (int i = 0; i < hitF.Length; i++)
            {
                RaycastHit hit = hitF[i];
                if (hit.collider.transform.parent.name == "NonTileParent")
                {
                    break;
                }
                else if (hit.collider.transform.parent.name == "TileParent")
                {
                    guidePos = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + hit.collider.bounds.size.y / 2, hit.collider.transform.position.z);
                    Instantiate(pathHighlight, guidePos, Quaternion.identity).transform.SetParent(hit.collider.transform);
                }
            }

            for (int i = 0; i < hitB.Length; i++)
            {
                RaycastHit hit = hitB[i];
                if (hit.collider.transform.parent.name == "NonTileParent")
                {
                    break;
                }
                else if (hit.collider.transform.parent.name == "TileParent")
                {
                    guidePos = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + hit.collider.bounds.size.y / 2, hit.collider.transform.position.z);
                    Instantiate(pathHighlight, guidePos, Quaternion.identity).transform.SetParent(hit.collider.transform);
                }
            }

            for (int i = 0; i < hitL.Length; i++)
            {
                RaycastHit hit = hitL[i];
                if (hit.collider.transform.parent.name == "NonTileParent")
                {
                    break;
                }
                else if (hit.collider.transform.parent.name == "TileParent")
                {
                    guidePos = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + hit.collider.bounds.size.y / 2, hit.collider.transform.position.z);
                    Instantiate(pathHighlight, guidePos, Quaternion.identity).transform.SetParent(hit.collider.transform);
                }
            }

            for (int i = 0; i < hitR.Length; i++)
            {
                RaycastHit hit = hitR[i];
                if (hit.collider.transform.parent.name == "NonTileParent")
                {
                    break;
                }
                else if (hit.collider.transform.parent.name == "TileParent")
                {
                    guidePos = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + hit.collider.bounds.size.y / 2, hit.collider.transform.position.z);
                    Instantiate(pathHighlight, guidePos, Quaternion.identity).transform.SetParent(hit.collider.transform);
                }
            }
            canInstance = false;
        }
    }

    public void DestroyInsts()
    {
        for (int i = 0; i < hitF.Length; i++)
        {
            if (hitF[i].transform.Find("PathHighlight(Clone)") != null)
            {
                Destroy(hitF[i].transform.Find("PathHighlight(Clone)").gameObject);
            }
        }
        for (int i = 0; i < hitB.Length; i++)
        {
            if (hitB[i].transform.Find("PathHighlight(Clone)") != null)
            {
                Destroy(hitB[i].transform.Find("PathHighlight(Clone)").gameObject);
            }
        }
        for (int i = 0; i < hitL.Length; i++)
        {
            if (hitL[i].transform.Find("PathHighlight(Clone)") != null)
            {
                Destroy(hitL[i].transform.Find("PathHighlight(Clone)").gameObject);
            }
        }
        for (int i = 0; i < hitR.Length; i++)
        {
            if (hitR[i].transform.Find("PathHighlight(Clone)") != null)
            {
                Destroy(hitR[i].transform.Find("PathHighlight(Clone)").gameObject);
            }
        }
    }
}
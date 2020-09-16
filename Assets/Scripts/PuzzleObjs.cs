using System;
using System.Collections;
using UnityEngine;

public class PuzzleObjs : MonoBehaviour
{

    public PlayerMovements playerMov = null;

    public GameObject nonTileDoorObj = null;

    public Puzzle puzzle = null;

    public float speed = 0.04f;

    public Camera cam = null;
    public Camera puzzleCam = null;

    [HideInInspector]
    public Collider[] hitColliders;
    public Ray ray;
    public RaycastHit hit;

    [HideInInspector]
    public Vector3 targetPosition;

    public static bool doorOpen = false;

    void Start()
    {
        targetPosition = this.transform.position;
    }

    void Update()
    {
        hitColliders = Physics.OverlapSphere(this.transform.position, 2.5f);

        foreach (var hitCollider in hitColliders)
        {
            try
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
                                if (hit.collider.tag == "SlidePuzzle")
                                {
                                    playerMov.DestroyInsts();
                                    playerMov.enabled = false;
                                    cam.enabled = false;
                                    puzzleCam.enabled = true;
                                    puzzle.enabled = true;
                                    puzzle.state = Puzzle.PuzzleState.Start;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        if (doorOpen)
        {
            targetPosition = new Vector3(transform.position.x, transform.position.y - gameObject.GetComponent<Collider>().bounds.size.y * 2, transform.position.z);
            nonTileDoorObj.transform.position = new Vector3(nonTileDoorObj.transform.position.x, nonTileDoorObj.transform.position.y - gameObject.GetComponent<Collider>().bounds.size.y * 2, nonTileDoorObj.transform.position.z);
            StartCoroutine(WaitSecs());
            doorOpen = false;
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
    }
    IEnumerator WaitSecs()
    {
        yield return new WaitForSeconds(0.5f);
        playerMov.canInstance = true;
    }
}

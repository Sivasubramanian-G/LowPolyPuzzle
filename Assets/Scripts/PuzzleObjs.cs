using System;
using UnityEngine;

public class PuzzleObjs : MonoBehaviour
{

    public PlayerMovements playerMov = null;

    public LeverMechanism lever = null;

    public Puzzle puzzle = null;

    public Camera cam = null;
    public Camera puzzleCam = null;

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
                if (Input.GetMouseButtonDown(0))
                {
                    ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100f))
                    {
                        if (hit.collider != null)
                        {
                            try
                            {
                                if (hit.collider.transform.parent.name == "SlidePuzzle")
                                {
                                    playerMov.enabled = false;
                                    cam.enabled = false;
                                    puzzleCam.enabled = true;
                                    puzzle.enabled = true;
                                    puzzle.state = Puzzle.PuzzleState.Start;
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
        }
    }
}

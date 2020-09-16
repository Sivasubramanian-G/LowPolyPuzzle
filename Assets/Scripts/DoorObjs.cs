using System.Collections;
using UnityEngine;

public class DoorObjs : MonoBehaviour
{

    public PlayerMovements playerMov = null;

    public GameObject nonTileDoorObj = null;

    public float speed = 0.04f;

    [HideInInspector]
    public Collider[] hitColliders;
    public Ray ray;
    public RaycastHit hit;

    [HideInInspector]
    public Vector3 targetPosition;

    void Start()
    {
        targetPosition = this.transform.position;
    }

    void Update()
    {
        hitColliders = Physics.OverlapSphere(this.transform.position, 2.5f);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.name == "Player" && playerMov.anim.GetBool("RunLoopStop") && playerMov.haveKey)
            {
                if (this.GetComponent<Collider>().tag == playerMov.keyTag)
                {
                    targetPosition = new Vector3(transform.position.x, transform.position.y - gameObject.GetComponent<Collider>().bounds.size.y * 2, transform.position.z);
                    nonTileDoorObj.transform.position = new Vector3(nonTileDoorObj.transform.position.x, nonTileDoorObj.transform.position.y - gameObject.GetComponent<Collider>().bounds.size.y * 2, nonTileDoorObj.transform.position.z);
                    StartCoroutine(WaitSecs());
                    playerMov.haveKey = false;
                }
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);

    }

    IEnumerator WaitSecs()
    {
        yield return new WaitForSeconds(0.05f);
        playerMov.DestroyInsts();
        playerMov.canInstance = true;
    }

}

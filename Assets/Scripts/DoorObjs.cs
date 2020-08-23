using UnityEngine;

public class DoorObjs : MonoBehaviour
{

    public PlayerMovements playerMov = null;

    public GameObject nonTileDoorObj = null;

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
                if (playerMov.haveKey)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - gameObject.GetComponent<Collider>().bounds.size.y * 2, transform.position.z);
                    nonTileDoorObj.transform.position = new Vector3(nonTileDoorObj.transform.position.x, nonTileDoorObj.transform.position.y - gameObject.GetComponent<Collider>().bounds.size.y * 2, nonTileDoorObj.transform.position.z);
                    playerMov.DestroyInsts();
                    playerMov.canInstance = true;
                }
            }
        }
    }
}

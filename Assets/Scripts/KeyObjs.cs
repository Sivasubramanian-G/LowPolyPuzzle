using UnityEngine;

public class KeyObjs : MonoBehaviour
{
    public GameObject keyTrigger;

    public PlayerMovements playerMov;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerMov.haveKey = true;
            Destroy(gameObject);
        }
    }
}

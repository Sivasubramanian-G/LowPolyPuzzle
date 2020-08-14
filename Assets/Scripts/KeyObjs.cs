using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyObjs : MonoBehaviour
{
    public GameObject keyTrigger;
    public float speed = 8f;
    public bool move;
    void Start()
    {
        move = false;
    }

    void Update()
    {
        if (move)
        {
            keyTrigger.transform.position = Vector3.Lerp(keyTrigger.transform.position, new Vector3(keyTrigger.transform.position.x, keyTrigger.GetComponent<DragObject>().maxHeight, keyTrigger.transform.position.z), speed * Time.deltaTime);
        }
        if (keyTrigger.transform.position.y == keyTrigger.GetComponent<DragObject>().maxHeight)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            move = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}

﻿using UnityEngine;

public class KeyObjs : MonoBehaviour
{
    public PlayerMovements playerMov;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerMov.haveKey = true;
            playerMov.keyTag = this.GetComponent<Collider>().tag;
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int sceneNum;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            //SceneManager.LoadScene("SampleScene1");
            SceneManager.LoadScene(sceneNum);
        }
    }
}

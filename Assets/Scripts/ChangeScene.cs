using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    //public int sceneNum;
    [HideInInspector]
    public int isLevelCompleted;
    [HideInInspector]
    public string levelPrefs;
    private void OnTriggerEnter(Collider other)
    {
        int sceneNum = SceneManager.GetActiveScene().buildIndex + 1;
        if (other.name == "Player")
        {
            levelPrefs = SceneManager.GetActiveScene().buildIndex.ToString() + "completed";
            isLevelCompleted = PlayerPrefs.GetInt(levelPrefs);
            if (isLevelCompleted == 0)
            {
                PlayerPrefs.SetInt(levelPrefs, 1);
            }
            SceneManager.LoadScene(sceneNum);
        }
    }
}

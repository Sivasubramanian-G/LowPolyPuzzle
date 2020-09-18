using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckFirstTime : MonoBehaviour
{
    public GameObject infoPanel;
    public int levelIsFirst;
    public string levelIndex;

    void Start()
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex.ToString();
        levelIsFirst = PlayerPrefs.GetInt(levelIndex);
        if (levelIsFirst == 0)
        {
            Activate();
            PlayerPrefs.SetInt(levelIndex, 1);
        }
        else
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        infoPanel.SetActive(true);
    }

    public void Deactivate()
    {
        infoPanel.SetActive(false);
    }

}

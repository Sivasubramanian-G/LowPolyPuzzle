using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    public VideoControl videoControl;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString() + "completed") == 0)
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString() + "completed", 1);
                Play();
            }
        }
    }

    public void Play()
    {
        videoControl.SaveFrame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Settings()
    {
        Debug.Log("Settings");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Levels()
    {
        SceneManager.LoadScene(1);
    }

}

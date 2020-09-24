using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{

    void Start()
    {
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString() + "completed") == 0)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString() + "completed", 1);
            Play();
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Settings()
    {
        Debug.Log("Settings");
    }

    public void Quit()
    {
        if (UnityEditor.EditorApplication.isPlaying == true)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public void Levels()
    {
        SceneManager.LoadScene(1);
    }

}

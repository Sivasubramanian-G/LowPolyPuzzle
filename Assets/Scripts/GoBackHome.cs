using UnityEngine;
using UnityEngine.SceneManagement;
public class GoBackHome : MonoBehaviour
{
    public void BackHome()
    {
        SceneManager.LoadScene(0);
    }
}

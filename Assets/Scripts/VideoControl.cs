using UnityEngine;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    public VideoPlayer player;
    void Start()
    {
        player = gameObject.GetComponent<VideoPlayer>();
        player.Prepare();
        player.frame = PlayerPrefs.GetInt("BGVideo");
    }

    public void SaveFrame()
    {
        PlayerPrefs.SetInt("BGVideo", (int)player.frame);
    }
}

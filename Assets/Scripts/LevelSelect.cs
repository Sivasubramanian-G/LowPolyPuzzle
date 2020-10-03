using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public int levelLimit;
    public Button button, disabledButton;
    public RectTransform parent;
    public VideoControl videoControl;

    void Start()
    {
        CheckFirstTime();
        SetupButtons();
    }

    void CheckFirstTime()
    {
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString() + "completed") == 0)
        {
            videoControl.SaveFrame();
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString() + "completed", 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void SetupButtons()
    {
        Vector3[] coords = new Vector3[4];
        parent.GetLocalCorners(coords);

        float startX = coords[1].x + ((parent.GetComponent<RectTransform>().rect.width - (button.GetComponent<RectTransform>().rect.width * 7)) / 2);

        float x = startX;
        float y = coords[1].y + ((parent.GetComponent<RectTransform>().rect.height - (button.GetComponent<RectTransform>().rect.height * 3)) / 2);

        for (int i = 1; i <= levelLimit; i++)
        {
            int I = SceneManager.GetActiveScene().buildIndex + i;
            Button btn;
            if (PlayerPrefs.GetInt((SceneManager.GetActiveScene().buildIndex + i).ToString()) == 1)
            {
                btn = Instantiate(button);
            }
            else
            {
                if (SceneManager.GetActiveScene().buildIndex + i == SceneManager.GetActiveScene().buildIndex + 1)
                {
                    btn = Instantiate(button);
                }
                else
                {
                    btn = Instantiate(disabledButton);
                }
            }
            var btnRect = btn.GetComponent<RectTransform>();
            btn.GetComponentInChildren<Text>().text = i.ToString();

            float width = btnRect.rect.width * 2;
            float height = btnRect.rect.height * 2;

            btn.transform.SetParent(parent, false);
            btnRect.anchoredPosition = new Vector2(x + width / 4, y - height);
            btn.onClick.AddListener(() => Temp(btn, I));

            if (i % 4 == 0)
            {
                y -= height;
                x = startX;
            }
            else
            {
                x += width;
            }
        }
    }

    void Temp(Button btn, int i)
    {
        videoControl.SaveFrame();
        SceneManager.LoadScene(i);
    }
}

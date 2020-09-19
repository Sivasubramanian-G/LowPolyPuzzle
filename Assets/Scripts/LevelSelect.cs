using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public Button button, disabledButton;
    public RectTransform parent;

    void Start()
    {
        CheckFirstTime();
        SetupButtons();
    }

    void CheckFirstTime()
    {
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString()) == 0)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), 1);
            SceneManager.LoadScene(0);
        }
    }

    void SetupButtons()
    {
        Vector3[] coords = new Vector3[4];
        parent.GetLocalCorners(coords);

        float startX = coords[1].x + ((parent.GetComponent<RectTransform>().rect.width - (button.GetComponent<RectTransform>().rect.width * 7)) / 2);

        float x = startX;
        float y = coords[1].y + ((parent.GetComponent<RectTransform>().rect.height - (button.GetComponent<RectTransform>().rect.height * 3)) / 2);

        for (int i = 1; i <= 8; i++)
        {
            int I = i;
            Button btn;
            if (PlayerPrefs.GetInt((i - 1).ToString() + "completed") == 1)
            {
                btn = Instantiate(button);
            }
            else
            {
                if ((i-1) == 0)
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
            btn.onClick.AddListener(() => Temp(btn, I - 1));

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
        Debug.Log("Scene Number : " + i);
        SceneManager.LoadScene(i);
    }
}

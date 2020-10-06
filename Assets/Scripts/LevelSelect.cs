using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public int levelLimit;
    public Button button, disabledButton;
    public GameObject parent, uiPanel2;
    public VideoControl videoControl;

    [HideInInspector]
    public float startX, startY;
    [HideInInspector]
    public Vector3[] coords;

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

    void SetStartXY()
    {
        coords = new Vector3[4];
        parent.GetComponent<RectTransform>().GetLocalCorners(coords);
        startX = coords[1].x + ((parent.GetComponent<RectTransform>().rect.width - (button.GetComponent<RectTransform>().rect.width * 7)) / 2);
        startY = coords[1].y + ((parent.GetComponent<RectTransform>().rect.height - (button.GetComponent<RectTransform>().rect.height * 5)) / 2);
    }

    void SetupButtons()
    {

        SetStartXY();

        uiPanel2.transform.localPosition = new Vector2(coords[2].x + uiPanel2.GetComponent<RectTransform>().rect.width / 2, parent.transform.localPosition.y);

        float x = startX;
        float y = startY;

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

            btn.transform.SetParent(parent.transform, false);
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

            if (i % 8 == 0)
            {
                parent = uiPanel2;
                SetStartXY();
                x = startX;
                y = startY;
            }

        }
    }

    void Temp(Button btn, int i)
    {
        videoControl.SaveFrame();
        SceneManager.LoadScene(i);
    }
}

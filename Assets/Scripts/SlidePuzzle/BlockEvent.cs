using System.Collections;
using UnityEngine;

public class BlockEvent : MonoBehaviour
{
    public event System.Action<BlockEvent> OnBlockPressed;
    public event System.Action OnFinishedMoving;
    public Vector2Int coord, startingCoord;

    public void Init(Vector2Int startingCoord, Texture2D image)
    {
        this.startingCoord = startingCoord;
        coord = startingCoord;
        GetComponent<MeshRenderer>().material = Resources.Load<Material>("Block");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    public void MoveToPosition(Vector3 target, float duration)
    {
        StartCoroutine(AnimateMove(target, duration));
    }

    void OnMouseDown()
    {
        if (OnBlockPressed != null && !PauseMenu.gamePaused)
        {
            OnBlockPressed(this);
        }
    }

    IEnumerator AnimateMove(Vector3 target, float duration)
    {
        Vector3 initialPos = transform.position;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(initialPos, target, percent);
            yield return null;
        }

        if (OnFinishedMoving != null)
        {
            OnFinishedMoving();
        }
    }

    public bool IsAtStartingCoord()
    {
        return coord == startingCoord;
    }

}

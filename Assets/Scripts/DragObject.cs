using System.Linq;
using UnityEngine;

public class DragObject : MonoBehaviour
{
	public PlayerMovements playerMov = null;

	public GameObject nonTileDragObj = null;

	public Camera cam = null;

	public float maxHeight = 3f;

	[HideInInspector]
	public Vector3 screenPoint, offset, initialPos;

	[HideInInspector]
	public bool canDrag = true, canTouchDrag = false;

	void Start()
    {
		initialPos = transform.position;
    }

	void Update()
    {
		Vector3 dir = this.transform.TransformDirection(Vector3.up);

		Debug.DrawRay(this.transform.position, dir * 10, Color.red);

		RaycastHit[] hits = Physics.RaycastAll(this.transform.position, dir, 5).OrderBy(h => h.distance).ToArray(); ;

		canDrag = true;
		for (int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			if (hit.collider.name == "Player")
			{
				canDrag = false;
			}
		}

		if (Input.touchCount > 0 && !PauseMenu.gamePaused && canDrag)
        {
			if (Input.touches[0].phase == TouchPhase.Began && canTouchDrag)
            {
				Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
				if (Physics.Raycast(ray, out RaycastHit hit, 100))
				{
					if (hit.collider != null && hit.collider.transform.name == "Cube (8)")
                    {
						Debug.Log("DragObj ray");
						canTouchDrag = true;
                    }
				}
				playerMov.canMove = false;
				playerMov.canClick = false;
				playerMov.DestroyInsts();
				screenPoint = cam.WorldToScreenPoint(gameObject.transform.position);
				offset = gameObject.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, screenPoint.z));
			}
			if (Input.touches[0].phase == TouchPhase.Moved && canTouchDrag)
            {
				
				playerMov.canClick = false;
				Vector3 cursorPoint = new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, screenPoint.z);
				Vector3 cursorPosition = cam.ScreenToWorldPoint(cursorPoint) + offset;
				if (cursorPosition.y > maxHeight)
				{
					cursorPosition.y = maxHeight;
					playerMov.canClick = true;
				}
				else if (cursorPosition.y < initialPos.y)
				{
					cursorPosition.y = initialPos.y;
					playerMov.canClick = true;
				}
				if (canDrag)
				{
					transform.position = new Vector3(transform.position.x, cursorPosition.y, transform.position.z);
					nonTileDragObj.transform.position = new Vector3(transform.position.x, cursorPosition.y - this.GetComponent<Collider>().bounds.size.y * 1.5f, transform.position.z);
				}
				
			}
			if (Input.touches[0].phase == TouchPhase.Ended && canTouchDrag)
            {
				playerMov.canMove = true;
				playerMov.canClick = true;
				//playerMov.InstObjs();
				playerMov.canInstance = true;
			}

		}

	}

	/*void OnMouseUp()
    {
		if (!PauseMenu.gamePaused)
        {
			playerMov.canMove = true;
			playerMov.canClick = true;
			//playerMov.InstObjs();
			playerMov.canInstance = true;
		}
	}

	void OnMouseDown()
	{
		if (!PauseMenu.gamePaused)
        {
			playerMov.canMove = false;
			playerMov.canClick = false;
			playerMov.DestroyInsts();
			screenPoint = cam.WorldToScreenPoint(gameObject.transform.position);
			offset = gameObject.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}
	}

	void OnMouseDrag()
	{
		if (!PauseMenu.gamePaused)
        {
			playerMov.canClick = false;
			Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 cursorPosition = cam.ScreenToWorldPoint(cursorPoint) + offset;
			if (cursorPosition.y > maxHeight)
			{
				cursorPosition.y = maxHeight;
				playerMov.canClick = true;
			}
			else if (cursorPosition.y < initialPos.y)
			{
				cursorPosition.y = initialPos.y;
				playerMov.canClick = true;
			}
			if (canDrag)
			{
				transform.position = new Vector3(transform.position.x, cursorPosition.y, transform.position.z);
				nonTileDragObj.transform.position = new Vector3(transform.position.x, cursorPosition.y - this.GetComponent<Collider>().bounds.size.y * 1.5f, transform.position.z);
			}
		}
	}*/
}

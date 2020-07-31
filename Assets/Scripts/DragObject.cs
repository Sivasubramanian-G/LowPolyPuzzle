using UnityEngine;

public class DragObject : MonoBehaviour
{
	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 initialPos;
	public PlayerMovements playerMov = null;
	public bool canInst = true;

	void Start()
    {
		//playerMov = GetComponent<PlayerMovements>();
		initialPos = transform.position;
    }

	/*void Update()
    {
		if (Input.GetMouseButtonUp(0))
		{
			//playerMov.canInstance = true;
			playerMov.InstObjs();
		}
	}*/

	void OnMouseUp()
    {
		playerMov.canMove = true;
		playerMov.InstObjs();
	}

	void OnMouseDown()
	{
		playerMov.canMove = false;
		playerMov.DestroyInsts();
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag()
	{
		canInst = false;
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		if (cursorPosition.y > 3)
        {
			cursorPosition.y = 3;
        }
		else if (cursorPosition.y < initialPos.y)
        {
			cursorPosition.y = initialPos.y;
			canInst = true;
		}
		transform.position = new Vector3(transform.position.x, cursorPosition.y, transform.position.z);
	}
}

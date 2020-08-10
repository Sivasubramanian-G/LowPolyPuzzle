using System.Linq;
using UnityEngine;

public class DragObject : MonoBehaviour
{
	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 initialPos;
	public PlayerMovements playerMov = null;
	public GameObject nonTileDragObj = null;
	public bool canDrag = true;

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
	}

	void OnMouseUp()
    {
		playerMov.canMove = true;
		playerMov.canClick = true;
		playerMov.InstObjs();
	}

	void OnMouseDown()
	{
		playerMov.canMove = false;
		playerMov.canClick = false;
		playerMov.DestroyInsts();
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag()
	{
		playerMov.canClick = false;
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		if (cursorPosition.y > 3)
        {
			cursorPosition.y = 3;
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
			nonTileDragObj.transform.position = new Vector3(transform.position.x, cursorPosition.y - this.GetComponent<Collider>().bounds.size.y*1.5f, transform.position.z);
		}
	}
}

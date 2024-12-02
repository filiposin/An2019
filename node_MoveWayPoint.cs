using UnityEngine;

public class node_MoveWayPoint : MonoBehaviour
{
	[Tooltip("The AI agent you want to follow this wayPoint.")]
	public GameObject myAI;

	private Vector3 newPosition;

	private float tempMoveSpeed;

	private node_AIMovement myAIM;

	private void Start()
	{
		newPosition = base.transform.position;
		myAIM = myAI.GetComponent<node_AIMovement>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "floor")
			{
				newPosition = new Vector3(hitInfo.point.x, hitInfo.point.y + 1f, hitInfo.point.z);
				base.transform.position = newPosition;
				myAIM.StartCoroutine(myAIM.wayPointHit(0f, true));
			}
		}
	}
}

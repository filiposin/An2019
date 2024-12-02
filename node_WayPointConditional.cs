using UnityEngine;

public class node_WayPointConditional : MonoBehaviour
{
	[Tooltip("Set true if this node is used in a conditional setting. Check out the conditionalWayPointDemo scene for an example")]
	public bool conditionalWayPoint;

	[Tooltip("use this to id custom wayPoints for unique behaviours. Typically only used on conditinal wayPoints")]
	public string wayPointName;

	[HideInInspector]
	public bool atThisWayPoint;

	[HideInInspector]
	public Transform[] thingsWayPoints;

	[HideInInspector]
	public GameObject thingAtThisWayPoint;

	[HideInInspector]
	public Transform nextWayPointConditional;

	[HideInInspector]
	public bool moveToNextWayPoint;

	private void Update()
	{
		if (!atThisWayPoint)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Transform[] array = thingsWayPoints;
			foreach (Transform transform in array)
			{
				if (transform.GetComponent<node_WayPointConditional>().wayPointName == "red" && transform != base.transform)
				{
					nextWayPointConditional = transform;
					moveToNextWayPoint = true;
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Transform[] array2 = thingsWayPoints;
			foreach (Transform transform2 in array2)
			{
				if (transform2.GetComponent<node_WayPointConditional>().wayPointName == "blue" && transform2 != base.transform)
				{
					nextWayPointConditional = transform2;
					moveToNextWayPoint = true;
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Transform[] array3 = thingsWayPoints;
			foreach (Transform transform3 in array3)
			{
				if (transform3.GetComponent<node_WayPointConditional>().wayPointName == "orange" && transform3 != base.transform)
				{
					nextWayPointConditional = transform3;
					moveToNextWayPoint = true;
				}
			}
		}
		if (!Input.GetKeyDown(KeyCode.Alpha4))
		{
			return;
		}
		Transform[] array4 = thingsWayPoints;
		foreach (Transform transform4 in array4)
		{
			if (transform4.GetComponent<node_WayPointConditional>().wayPointName == "green" && transform4 != base.transform)
			{
				nextWayPointConditional = transform4;
				moveToNextWayPoint = true;
			}
		}
	}
}

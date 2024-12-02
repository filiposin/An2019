using UnityEngine;

public class node_ConditionalWayPointHandler : MonoBehaviour
{
	[Tooltip("this is public to assign an initial target wayPoint.")]
	public Transform myTargetWayPoint;

	[Tooltip("set this to true if you want to patrol the list of wayPoints from the aiMovement script.")]
	public bool usePatrol;

	[HideInInspector]
	public int wayPointIndex;

	[HideInInspector]
	public Transform[] myWayPoints;

	[HideInInspector]
	public bool atWayPoint;

	[HideInInspector]
	public bool atConditionalWayPoint;

	private node_AIMovement myaiMovement;

	private float wayPointDistance;

	private void Awake()
	{
		myaiMovement = GetComponent<node_AIMovement>();
		myWayPoints = myaiMovement.wayPointSettings.wayPoints;
		if (myTargetWayPoint == null || usePatrol)
		{
			myTargetWayPoint = myWayPoints[0];
		}
	}

	private void Update()
	{
		wayPointDistance = Vector3.Distance(base.transform.position, myTargetWayPoint.position);
		if (wayPointDistance <= myaiMovement.advancedSettings.stopDistanceAdjust + myaiMovement.agent.stoppingDistance && myaiMovement.agent.speed != 0f)
		{
			wayPointHit();
		}
		if (atConditionalWayPoint && myTargetWayPoint.GetComponent<node_WayPointConditional>().moveToNextWayPoint)
		{
			myTargetWayPoint.GetComponent<node_WayPointConditional>().atThisWayPoint = false;
			myTargetWayPoint.GetComponent<node_WayPointConditional>().moveToNextWayPoint = false;
			myTargetWayPoint = myTargetWayPoint.GetComponent<node_WayPointConditional>().nextWayPointConditional;
			myaiMovement.targetWayPoint = myTargetWayPoint;
			myaiMovement.agent.speed = myaiMovement.speedSettings.walkSpeed;
			myaiMovement.agent.SetDestination(myTargetWayPoint.position);
			atConditionalWayPoint = false;
		}
	}

	private void wayPointHit()
	{
		myaiMovement.agent.speed = 0f;
		myTargetWayPoint.GetComponent<node_WayPointConditional>().atThisWayPoint = true;
		myTargetWayPoint.GetComponent<node_WayPointConditional>().thingAtThisWayPoint = base.gameObject;
		myTargetWayPoint.GetComponent<node_WayPointConditional>().thingsWayPoints = myWayPoints;
		if (myTargetWayPoint.GetComponent<node_WayPointConditional>().conditionalWayPoint)
		{
			atConditionalWayPoint = true;
		}
		if (usePatrol)
		{
			wayPointIndex++;
			if (wayPointIndex >= myWayPoints.Length)
			{
				wayPointIndex = 0;
			}
			if (!myTargetWayPoint.GetComponent<node_WayPointConditional>().conditionalWayPoint)
			{
				myTargetWayPoint.GetComponent<node_WayPointConditional>().atThisWayPoint = false;
				myTargetWayPoint = myWayPoints[wayPointIndex];
				myaiMovement.targetWayPoint = myTargetWayPoint;
				myaiMovement.agent.speed = myaiMovement.speedSettings.walkSpeed;
			}
		}
	}
}

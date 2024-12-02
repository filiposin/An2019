using UnityEngine;

public class WheelAlignment : MonoBehaviour
{
	public WheelCollider CorrespondingCollider;

	public GameObject SlipPrefab;

	private float RotationValue;

	private void Update()
	{
		Vector3 vector = CorrespondingCollider.transform.TransformPoint(CorrespondingCollider.center);
		RaycastHit hitInfo;
		if (Physics.Raycast(vector, -CorrespondingCollider.transform.up, out hitInfo, CorrespondingCollider.suspensionDistance + CorrespondingCollider.radius))
		{
			base.transform.position = hitInfo.point + CorrespondingCollider.transform.up * CorrespondingCollider.radius;
		}
		else
		{
			base.transform.position = vector - CorrespondingCollider.transform.up * CorrespondingCollider.suspensionDistance;
		}
		base.transform.rotation = CorrespondingCollider.transform.rotation * Quaternion.Euler(RotationValue, CorrespondingCollider.steerAngle, 0f);
		RotationValue += CorrespondingCollider.rpm * 6f * Time.deltaTime;
		WheelHit hit;
		CorrespondingCollider.GetGroundHit(out hit);
		if (Mathf.Abs(hit.sidewaysSlip) > 10f && (bool)SlipPrefab)
		{
			Object.Instantiate(SlipPrefab, hit.point, Quaternion.identity);
		}
	}
}

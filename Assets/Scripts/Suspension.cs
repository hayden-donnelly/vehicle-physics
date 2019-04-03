using UnityEngine;

public class Suspension : MonoBehaviour 
{
	[SerializeField] private Rigidbody vehicleBody;
	[SerializeField] private float wheelRadius = 0.5f;
	[SerializeField] private float suspensionDistanceConstant = 1f;
	[SerializeField] private float springConstant = 30000f;
	[SerializeField] private float damperConstant = 4000f;

	private float previousSuspensionDistance;
	private float currentSuspensionDistance;
	private float springVelocity;
	private float springForce;
	private float damperForce;

	private void FixedUpdate()
	{
		// Vehicle uses a raycast suspension system
		RaycastHit hit;
		Ray ray = new Ray(transform.position, -transform.up);
		if(Physics.Raycast(ray, out hit, suspensionDistanceConstant + wheelRadius))
		{
			// Hooke's Law
			previousSuspensionDistance = currentSuspensionDistance;
			currentSuspensionDistance = suspensionDistanceConstant - (hit.distance - wheelRadius);
			springVelocity = (currentSuspensionDistance - previousSuspensionDistance) / Time.fixedDeltaTime;
			springForce = springConstant * currentSuspensionDistance;
			damperForce = damperConstant * springVelocity;

			// Apply force to car body
			vehicleBody.AddForceAtPosition(transform.up * (springForce + damperForce), transform.position, ForceMode.Force);
		}

		Debug.DrawRay(transform.position, -transform.up * (suspensionDistanceConstant), Color.blue, 0f);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour 
{
	[Header("Rigidbody")]
	[SerializeField] private Vector3 centerOfMass;
	[Header("Acceleration")]
	[SerializeField] private float accelSpeed;
	[SerializeField] private ForceMode accelMode;
	[SerializeField] private Vector3 accelOrigin;
	[Header("Turning")]
	[SerializeField] private float turnSpeed;
	[SerializeField] private ForceMode turnMode;
	[Header("Traction")]
	[SerializeField] private float normalTractionConstant;
	[SerializeField] private float driftTractionConstant;
	private float tractionConstant;
	[SerializeField] private ForceMode tractionMode;
	[Header("Debug")]
	[SerializeField] private float velocityDebugRayLength;
	[SerializeField] private float currentSpeed;

	private Rigidbody rb;
	private float axisH;
	private float axisV;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = centerOfMass;
		tractionConstant = normalTractionConstant;
	} 

	private void Update()
	{
		// Get Input
		axisH = Input.GetAxis("Horizontal");
		axisV = Input.GetAxis("Vertical");
	}

	private void FixedUpdate()
	{
		currentSpeed = rb.velocity.magnitude;

		// Acceleration
		Vector3 desiredAccel = transform.forward * accelSpeed * axisV * Time.fixedDeltaTime;
		rb.AddForceAtPosition(desiredAccel, transform.position + accelOrigin, accelMode);

		// Turn
		if(axisV != 0)
		{
			Vector3 desiredTurn = Vector3.up * turnSpeed * axisH * Time.fixedDeltaTime;
			rb.AddTorque(desiredTurn, turnMode);
		}

		// Traction
		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			tractionConstant = driftTractionConstant;
		}
		if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			tractionConstant = normalTractionConstant;
		}
		Vector3 desiredTraction = transform.InverseTransformDirection(rb.velocity);
		rb.AddForce(transform.right * -desiredTraction.x * tractionConstant);
	}
}

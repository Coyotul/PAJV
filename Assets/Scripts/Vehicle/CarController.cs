using UnityEngine;

public class CarController : MonoBehaviour
{
	[SerializeField]
	private float acceleration = 15f;

	[SerializeField]
	private float maxSpeed = 20f;

	[SerializeField]
	private float turnSpeed = 100f;

	[SerializeField]
	private float brakeDeceleration = 25f;

	[SerializeField]
	private float drag = 0.1f; // Very small drag for stability (prevents jitter at high speeds)

	private Rigidbody rb;
	private float accelerationInput;
	private float brakeInput;
	private float turnInput;
	private float currentSpeed;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody>();
		}
		
		// Always set constraints to prevent tipping
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		
		// Set small drag for stability (prevents jitter at high speeds)
		rb.linearDamping = drag;
		rb.angularDamping = 0.1f; // Small angular damping for stability
		
		// Lower center of mass to prevent tipping
		rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
	}

	private void FixedUpdate()
	{
		// Get current velocity
		Vector3 currentVelocity = rb.linearVelocity;
		currentSpeed = currentVelocity.magnitude;
		
		// Calculate forward direction (only horizontal movement)
		Vector3 forward = transform.forward;
		forward.y = 0f;
		forward.Normalize();
		
		// Handle turning (only when moving)
		if (Mathf.Abs(turnInput) > 0.01f && currentSpeed > 0.1f)
		{
			float turnAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
			Quaternion turn = Quaternion.Euler(0f, turnAmount, 0f);
			rb.MoveRotation(rb.rotation * turn);
		}
		
		// Calculate velocity in forward direction (horizontal only)
		Vector3 horizontalVelocity = currentVelocity;
		horizontalVelocity.y = 0f;
		float horizontalSpeed = horizontalVelocity.magnitude;
		
		// Handle acceleration using forces for better stability
		if (accelerationInput > 0f)
		{
			// Use AddForce for smoother, more stable acceleration
			if (horizontalSpeed < maxSpeed)
			{
				Vector3 force = forward * acceleration * rb.mass;
				rb.AddForce(force, ForceMode.Force);
			}
			
			// Clamp speed to maxSpeed
			if (horizontalSpeed > maxSpeed)
			{
				Vector3 clampedVelocity = horizontalVelocity.normalized * maxSpeed;
				clampedVelocity.y = currentVelocity.y; // Preserve vertical velocity
				rb.linearVelocity = clampedVelocity;
			}
		}
		// Handle braking
		else if (brakeInput > 0f)
		{
			// Apply brake force opposite to velocity direction
			if (horizontalSpeed > 0.1f)
			{
				Vector3 brakeDirection = -horizontalVelocity.normalized;
				Vector3 brakeForce = brakeDirection * brakeDeceleration * rb.mass;
				rb.AddForce(brakeForce, ForceMode.Force);
			}
			else
			{
				// Stop completely
				Vector3 stopVelocity = currentVelocity;
				stopVelocity.x = 0f;
				stopVelocity.z = 0f;
				rb.linearVelocity = stopVelocity;
			}
		}
		// No input - natural deceleration handled by drag
		// Drag will naturally slow down the car
	}

	public void Accelerate()
	{
		// Set input flag instead of applying force directly
		accelerationInput = 1f;
	}

	public void StopAccelerate()
	{
		accelerationInput = 0f;
	}

	public void Brake()
	{
		// Set input flag instead of applying force directly
		brakeInput = 1f;
	}

	public void StopBrake()
	{
		brakeInput = 0f;
	}

	public void Turn(float direction)
	{
		// Store turn input (-1 to 1)
		turnInput = Mathf.Clamp(direction, -1f, 1f);
	}

	public void StopTurn()
	{
		turnInput = 0f;
	}

	/// <summary>
	/// Get the current speed of the car
	/// </summary>
	public float GetSpeed()
	{
		return currentSpeed;
	}
}





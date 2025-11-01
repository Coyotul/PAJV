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
	private float drag = 0f; // Set to 0 to eliminate friction

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
		
		// Set drag to 0 to eliminate friction
		rb.linearDamping = drag;
		rb.angularDamping = 0f; // Also set angular damping to 0
		
		// Lower center of mass to prevent tipping
		rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
		
		// Set up physics material for zero friction
		SetupZeroFrictionMaterial();
	}
	
	private void SetupZeroFrictionMaterial()
	{
		// Get all colliders on this GameObject
		Collider[] colliders = GetComponents<Collider>();
		
		// Create or get a zero-friction physics material
		PhysicsMaterial zeroFrictionMaterial = new PhysicsMaterial("ZeroFriction")
		{
			dynamicFriction = 0f,
			staticFriction = 0f,
			frictionCombine = PhysicsMaterialCombine.Minimum,
			bounciness = 0f,
			bounceCombine = PhysicsMaterialCombine.Minimum
		};
		
		// Apply the material to all colliders
		foreach (Collider col in colliders)
		{
			if (col != null)
			{
				col.material = zeroFrictionMaterial;
			}
		}
	}

	private void FixedUpdate()
	{
		// Get current velocity
		Vector3 currentVelocity = rb.linearVelocity;
		currentSpeed = currentVelocity.magnitude;
		
		// Handle turning (only when moving)
		if (Mathf.Abs(turnInput) > 0.01f && currentSpeed > 0.1f)
		{
			float turnAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
			Quaternion turn = Quaternion.Euler(0f, turnAmount, 0f);
			rb.MoveRotation(rb.rotation * turn);
		}
		
		// Calculate forward direction (only horizontal movement)
		Vector3 forward = transform.forward;
		forward.y = 0f;
		forward.Normalize();
		
		// Handle acceleration
		if (accelerationInput > 0f)
		{
			// Accelerate by directly modifying velocity
			currentSpeed += acceleration * Time.fixedDeltaTime;
			currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
			
			// Set velocity in forward direction
			Vector3 newVelocity = forward * currentSpeed;
			newVelocity.y = currentVelocity.y; // Preserve vertical velocity (gravity)
			rb.linearVelocity = newVelocity;
		}
		// Handle braking
		else if (brakeInput > 0f)
		{
			// Decelerate by reducing speed
			currentSpeed -= brakeDeceleration * Time.fixedDeltaTime;
			currentSpeed = Mathf.Max(0f, currentSpeed);
			
			// Set velocity in current direction (or forward if stopped)
			Vector3 direction = currentVelocity.magnitude > 0.1f ? currentVelocity.normalized : forward;
			direction.y = 0f;
			direction.Normalize();
			
			Vector3 newVelocity = direction * currentSpeed;
			newVelocity.y = currentVelocity.y; // Preserve vertical velocity
			rb.linearVelocity = newVelocity;
		}
		// No input - apply natural deceleration
		else
		{
			// Gradually slow down
			if (currentSpeed > 0.1f)
			{
				currentSpeed -= drag * Time.fixedDeltaTime;
				currentSpeed = Mathf.Max(0f, currentSpeed);
				
				Vector3 direction = currentVelocity.magnitude > 0.1f ? currentVelocity.normalized : forward;
				direction.y = 0f;
				direction.Normalize();
				
				Vector3 newVelocity = direction * currentSpeed;
				newVelocity.y = currentVelocity.y;
				rb.linearVelocity = newVelocity;
			}
			else
			{
				// Stop completely
				Vector3 newVelocity = currentVelocity;
				newVelocity.x = 0f;
				newVelocity.z = 0f;
				rb.linearVelocity = newVelocity;
			}
		}
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





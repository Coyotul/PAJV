using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("Target")]
	[SerializeField]
	private Transform target; // The car to follow

	[Header("Follow Settings")]
	[SerializeField]
	private Vector3 offset = new Vector3(0f, 6f, -8f); // Camera offset from target

	[SerializeField]
	private float followSpeed = 8f; // How fast camera follows position

	[SerializeField]
	private float lookSpeed = 12f; // How fast camera rotates to look at target

	[Header("Look At Settings")]
	[SerializeField]
	private bool lookAtTarget = true; // Should camera rotate to look at target?

	[SerializeField]
	private Vector3 lookAtOffset = Vector3.zero; // Offset for look at point

	[Header("Rotation Settings")]
	[SerializeField]
	private bool rotateWithTarget = true; // Should camera rotate with the car?

	[SerializeField]
	private float rotationFollowSpeed = 8f; // How fast camera follows rotation

	[Header("Constraints")]
	[SerializeField]
	private bool lockX = false;

	[SerializeField]
	private bool lockY = false;

	[SerializeField]
	private bool lockZ = false;

	private Vector3 currentVelocity;

	private void LateUpdate()
	{
		if (target == null)
			return;

		// Calculate desired position
		Vector3 desiredPosition;
		
		if (rotateWithTarget)
		{
			// Apply offset in target's local space (rotates with target)
			desiredPosition = target.position + target.rotation * offset;
		}
		else
		{
			// Apply offset in world space (doesn't rotate with target)
			desiredPosition = target.position + offset;
		}

		// Apply position constraints
		Vector3 currentPosition = transform.position;
		if (lockX) desiredPosition.x = currentPosition.x;
		if (lockY) desiredPosition.y = currentPosition.y;
		if (lockZ) desiredPosition.z = currentPosition.z;

		// Smoothly move camera to desired position
		transform.position = Vector3.SmoothDamp(
			transform.position,
			desiredPosition,
			ref currentVelocity,
			1f / followSpeed
		);

		// Handle rotation
		if (rotateWithTarget)
		{
			// Rotate camera with target, maintaining relative offset
			Quaternion targetRotation;
			
			if (lookAtTarget)
			{
				// Look at target while rotating with it
				Vector3 lookAtPoint = target.position + target.rotation * lookAtOffset;
				Vector3 lookDirection = lookAtPoint - transform.position;
				
				if (lookDirection != Vector3.zero)
				{
					targetRotation = Quaternion.LookRotation(lookDirection);
				}
				else
				{
					targetRotation = target.rotation;
				}
			}
			else
			{
				// Just rotate with target
				targetRotation = target.rotation;
			}
			
			// Smoothly rotate camera
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				targetRotation,
				rotationFollowSpeed * Time.deltaTime
			);
		}
		else if (lookAtTarget)
		{
			// Only look at target without rotating with it
			Vector3 lookAtPoint = target.position + lookAtOffset;
			Vector3 lookDirection = lookAtPoint - transform.position;

			if (lookDirection != Vector3.zero)
			{
				Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
				transform.rotation = Quaternion.Slerp(
					transform.rotation,
					targetRotation,
					lookSpeed * Time.deltaTime
				);
			}
		}
	}

	/// <summary>
	/// Set the target to follow (useful for runtime assignment)
	/// </summary>
	public void SetTarget(Transform newTarget)
	{
		target = newTarget;
	}

	/// <summary>
	/// Set the target to follow by GameObject (useful for runtime assignment)
	/// </summary>
	public void SetTarget(GameObject newTarget)
	{
		if (newTarget != null)
			target = newTarget.transform;
		else
			target = null;
	}
}


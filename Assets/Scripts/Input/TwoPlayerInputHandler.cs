using UnityEngine;
using UnityEngine.InputSystem;

public class TwoPlayerInputHandler : MonoBehaviour
{
	[SerializeField]
	private CarController player1;

	[SerializeField]
	private CarController player2;

	[Header("Player 1 Actions")]
	[SerializeField]
	private InputActionReference p1Accelerate;

	[SerializeField]
	private InputActionReference p1Brake;

	[SerializeField]
	private InputActionReference p1Steer; // expected float -1..1

	[Header("Player 2 Actions")]
	[SerializeField]
	private InputActionReference p2Accelerate;

	[SerializeField]
	private InputActionReference p2Brake;

	[SerializeField]
	private InputActionReference p2Steer; // expected float -1..1

	private void OnEnable()
	{
		EnableIfPresent(p1Accelerate);
		EnableIfPresent(p1Brake);
		EnableIfPresent(p1Steer);
		EnableIfPresent(p2Accelerate);
		EnableIfPresent(p2Brake);
		EnableIfPresent(p2Steer);
	}

	private void OnDisable()
	{
		DisableIfPresent(p1Accelerate);
		DisableIfPresent(p1Brake);
		DisableIfPresent(p1Steer);
		DisableIfPresent(p2Accelerate);
		DisableIfPresent(p2Brake);
		DisableIfPresent(p2Steer);
	}

	private void Update()
	{
		// Player 1
		if (player1 != null)
		{
			// Check button actions properly
			bool acc1 = ReadButton(p1Accelerate);
			bool brk1 = ReadButton(p1Brake);
			float steer1 = ReadFloat(p1Steer);
			
			if (acc1) 
				player1.Accelerate();
			else 
				player1.StopAccelerate();
				
			if (brk1) 
				player1.Brake();
			else 
				player1.StopBrake();
				
			if (Mathf.Abs(steer1) > 0.01f) 
				player1.Turn(Mathf.Clamp(steer1, -1f, 1f));
			else 
				player1.StopTurn();
		}

		// Player 2
		if (player2 != null)
		{
			bool acc2 = ReadButton(p2Accelerate);
			bool brk2 = ReadButton(p2Brake);
			float steer2 = ReadFloat(p2Steer);
			
			if (acc2) 
				player2.Accelerate();
			else 
				player2.StopAccelerate();
				
			if (brk2) 
				player2.Brake();
			else 
				player2.StopBrake();
				
			if (Mathf.Abs(steer2) > 0.01f) 
				player2.Turn(Mathf.Clamp(steer2, -1f, 1f));
			else 
				player2.StopTurn();
		}
	}

	private static void EnableIfPresent(InputActionReference aref)
	{
		if (aref != null && aref.action != null) aref.action.Enable();
	}

	private static void DisableIfPresent(InputActionReference aref)
	{
		if (aref != null && aref.action != null) aref.action.Disable();
	}

	private static float ReadFloat(InputActionReference aref)
	{
		return (aref != null && aref.action != null) ? aref.action.ReadValue<float>() : 0f;
	}

	private static bool ReadButton(InputActionReference aref)
	{
		return (aref != null && aref.action != null) && aref.action.IsPressed();
	}
}







using UnityEngine;
using TMPro;

public class CarUI : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField]
	private TextMeshProUGUI speedText;

	[SerializeField]
	private TextMeshProUGUI timerText;

	[Header("Settings")]
	[SerializeField]
	private bool showInKMH = true; // Display speed in km/h instead of m/s

	[SerializeField]
	private int decimalPlaces = 1; // Number of decimal places for speed

	private CarController carController;
	private float timer = 0f;
	private bool isTimerRunning = false;

	private void Awake()
	{
		carController = GetComponent<CarController>();
		if (carController == null)
		{
			Debug.LogWarning($"CarUI on {gameObject.name} requires a CarController component!");
		}
	}

	private void Start()
	{
		// Start timer when game starts
		StartTimer();
	}

	private void Update()
	{
		UpdateSpeedDisplay();
		UpdateTimerDisplay();
	}

	private void UpdateSpeedDisplay()
	{
		if (speedText == null || carController == null)
			return;

		float speed = carController.GetSpeed();
		
		if (showInKMH)
		{
			// Convert m/s to km/h (multiply by 3.6)
			speed = speed * 3.6f;
			speedText.text = $"Speed: {speed.ToString($"F{decimalPlaces}")} km/h";
		}
		else
		{
			speedText.text = $"Speed: {speed.ToString($"F{decimalPlaces}")} m/s";
		}
	}

	private void UpdateTimerDisplay()
	{
		if (timerText == null)
			return;

		if (isTimerRunning)
		{
			timer += Time.deltaTime;
		}

		// Format time as MM:SS.mmm
		int minutes = Mathf.FloorToInt(timer / 60f);
		int seconds = Mathf.FloorToInt(timer % 60f);
		int milliseconds = Mathf.FloorToInt((timer % 1f) * 1000f);

		timerText.text = $"Time: {minutes:00}:{seconds:00}.{milliseconds:000}";
	}

	/// <summary>
	/// Start the timer
	/// </summary>
	public void StartTimer()
	{
		isTimerRunning = true;
	}

	/// <summary>
	/// Stop the timer
	/// </summary>
	public void StopTimer()
	{
		isTimerRunning = false;
	}

	/// <summary>
	/// Reset the timer to 0
	/// </summary>
	public void ResetTimer()
	{
		timer = 0f;
		isTimerRunning = false;
	}

	/// <summary>
	/// Get the current timer value
	/// </summary>
	public float GetTimer()
	{
		return timer;
	}
}


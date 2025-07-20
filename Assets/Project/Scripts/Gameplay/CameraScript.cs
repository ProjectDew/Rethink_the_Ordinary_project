using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : ManagedBehaviour
{
	private const float SCREEN_TO_WORLD_MULTIPLIER = 0.01f;
	private const float MIN_MENU_DISTANCE = 0.1f;

	[SerializeField]
	private Vector3 menuPosition;

	[SerializeField]
	private Vector3 initialPosition;

	[SerializeField]
	private float menuSpeed;

	[SerializeField]
	private float minZoom;

	[SerializeField]
	private float maxZoom;

	[SerializeField]
	private float zoomSpeed;

	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float cameraLimit;
	
	private CameraStates states;

	private Transform tr;

	private Gameplay gameplay;

	private float zoomValue;

	private float zoomChange;

	private bool moving;

	public override void ManagedAwake ()
	{
		states = GetComponent<CameraStates> ();

		tr = transform;

		zoomValue = initialPosition.y;

		gameplay = new ();

		states.SetMenu ();
	}

	public override void ManagedOnEnable ()
	{
		gameplay.Enable ();

		InputAction zoomAction = gameplay.FindAction ("Zoom");
		InputAction moveAction = gameplay.FindAction ("Move");

		zoomAction.performed += StartZoom;
		zoomAction.canceled += StopZoom;

		moveAction.started += StartMove;
		moveAction.canceled += StopMove;
	}

	public override void ManagedUpdate ()
	{
		if (states.IsMenu)
		{
			SetMenuPosition ();
		}
		else if (states.IsRuntime)
		{
			ApplyZoom ();
			Move ();
		}
	}

	public override void ManagedOnDisable ()
	{
		InputAction zoomAction = gameplay.FindAction ("Zoom");
		InputAction moveAction = gameplay.FindAction ("Move");

		zoomAction.performed -= StartZoom;
		zoomAction.canceled -= StopZoom;

		moveAction.started -= StartMove;
		moveAction.canceled -= StopMove;
		
		gameplay.Disable ();
	}

	public void SetFixed () => states.SetMenu ();

	public void SetRuntime () => states.SetRuntime ();

	private void ApplyZoom ()
	{
		zoomValue = Mathf.Clamp (zoomValue - zoomChange, minZoom, maxZoom);

		float positionY = Mathf.Lerp (tr.position.y, zoomValue, zoomSpeed * Time.deltaTime);
		
		tr.position = new (tr.position.x, positionY, tr.position.z);
	}

	private void Move ()
	{
		if (!moving)
			return;

		Vector2 mousePosition = moveSpeed * SCREEN_TO_WORLD_MULTIPLIER * Mouse.current.delta.ReadValue ();
		Vector3 goalPosition = new (tr.position.x - mousePosition.x, tr.position.y, tr.position.z - mousePosition.y);

		goalPosition.x = Mathf.Clamp (goalPosition.x, initialPosition.x - cameraLimit, initialPosition.x + cameraLimit);
		goalPosition.z = Mathf.Clamp (goalPosition.z, initialPosition.z - cameraLimit, initialPosition.z + cameraLimit);

		tr.position = goalPosition;
	}

	private void SetMenuPosition ()
	{
		tr.position = Vector3.Lerp (tr.position, menuPosition, menuSpeed * Time.deltaTime);

		if (Vector3.Distance (tr.position, menuPosition) > MIN_MENU_DISTANCE)
			return;

		tr.position = menuPosition;

		states.SetIdle ();
	}

	private void StartZoom (InputAction.CallbackContext context)
	{
		zoomChange = context.ReadValue<Vector2> ().y;
	}

	private void StopZoom (InputAction.CallbackContext context)
	{
		zoomChange = 0;
	}

	private void StartMove (InputAction.CallbackContext context)
	{
		moving = true;
	}

	private void StopMove (InputAction.CallbackContext context)
	{
		moving = false;
	}
}

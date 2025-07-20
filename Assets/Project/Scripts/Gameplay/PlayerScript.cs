using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : ManagedBehaviour
{
	[SerializeField]
	private Property[] properties;

	private Dictionary<Property, int> propertyValues;

	private Gameplay gameplay;

	private Camera cam;
	
	public int UpdatePropertyValue (Property property, int valueDifference)
	{
		if (!propertyValues.ContainsKey (property))
			return 0;

		propertyValues[property] += valueDifference;

		return propertyValues[property];
	}

	public override void ManagedAwake ()
	{
		cam = Camera.main;
		gameplay = new ();
	}

	public override void ManagedStart ()
	{
		propertyValues = new ();

		for (int i = 0; i < properties.Length; i++)
			if (!propertyValues.ContainsKey (properties[i]))
				propertyValues.Add (properties[i], GameManager.GameData.InitialPropertyValue);
	}

	public override void ManagedOnEnable ()
	{
		gameplay.Enable ();
		gameplay.FindAction ("Click").started += SelectObject;
	}

	public override void ManagedOnDisable ()
	{
		gameplay.FindAction ("Click").started -= SelectObject;
		gameplay.Disable ();
	}

	private void SelectObject (InputAction.CallbackContext context)
	{
		Vector2 mousePosition = Mouse.current.position.ReadValue ();
		Vector3 cameraPosition = cam.transform.position;

		Vector3 worldPosition = cam.ScreenToWorldPoint (new Vector3 (mousePosition.x, mousePosition.y, cameraPosition.y));
		Vector3 originPosition = new (worldPosition.x, cameraPosition.y, worldPosition.z);

		Ray ray = new (originPosition, Vector3.down);

		if (Physics.Raycast (ray, out RaycastHit hit, 50))
		{
			if (!hit.collider.TryGetComponent (out InteractableObject selectedObject))
				selectedObject = hit.collider.GetComponentInParent<InteractableObject> ();

			if (selectedObject != null)
				GameManager.Instance.SelectObject (selectedObject);
		}
	}
}

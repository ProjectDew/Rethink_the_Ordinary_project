using UnityEngine;

public class InteractableObject : MonoBehaviour
{
	[SerializeField]
	private string objectName;

	[SerializeField]
	private Property[] properties;

	[SerializeField]
	private Property[] deadlyProperties;

	private Transform tr;

	public string Name => objectName;

	public Vector3 Position => tr.position;

	public bool HasProperty (Property property)
	{
		for (int i = 0; i < properties.Length; i++)
			if (property == properties[i])
				return true;

		return false;
	}

	public bool PropertyIsDeadly (Property property)
	{
		for (int i = 0; i < deadlyProperties.Length; i++)
			if (property == deadlyProperties[i])
				return true;

		return false;
	}

	private void Awake ()
	{
		tr = transform;
	}
}

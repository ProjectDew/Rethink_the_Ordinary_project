using UnityEngine;

[CreateAssetMenu(fileName = "Property", menuName = "Scriptable Objects/Property")]
public class Property : ScriptableObject
{
	[SerializeField]
	private string id;

	public string ID => id;
}

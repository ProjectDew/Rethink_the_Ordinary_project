using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Task")]
public class Task : ScriptableObject
{
	[SerializeField]
	private string interactionName;

	[SerializeField]
	private float duration;

	[SerializeField]
	private Property[] properties;

	public string Name => interactionName;

	public float Duration => duration;

	public Property[] GetProperties () => properties;
}

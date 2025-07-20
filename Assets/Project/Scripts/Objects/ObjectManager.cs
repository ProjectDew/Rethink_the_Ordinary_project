/*using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
	private List<InteractableObject> unselectableObjects;

	public InteractableObject SelectedObject { get; set; }

	public void AddObjectToUnselectableList (InteractableObject interactableObject)
	{
		unselectableObjects.Add (interactableObject);
	}

	public bool ObjectIsSelectable (InteractableObject interactableObject) => !unselectableObjects.Contains (interactableObject);

	public ObjectPropertyRelationship GetObjectTaskRelationship (InteractableObject interactableObject, Task task)
	{
		Property[] taskProperties = task.GetProperties ();

		for (int i = 0; i < taskProperties.Length; i++)
			if (interactableObject.PropertyIsDeadly (taskProperties[i]))
				return ObjectPropertyRelationship.Opposition;

		for (int i = 0; i < taskProperties.Length; i++)
			if (interactableObject.HasProperty (taskProperties[i]))
				return ObjectPropertyRelationship.Equivalence;

		return ObjectPropertyRelationship.Neutrality;
	}

	//public string GetDialogueFromSelectedObject (Task task)
	//{
	//	if (task == null)
	//		return GameManager.GameData.DialogueNotFoundError;

	//	Property[] taskProperties = task.GetProperties ();

	//	for (int i = 0; i < taskProperties.Length; i++)
	//		if (SelectedObject.PropertyIsDeadly (taskProperties[i]))
	//			return "";//deadlyDialogues.GetRandomText ();

	//	for (int i = 0; i < taskProperties.Length; i++)
	//		if (!SelectedObject.HasProperty (taskProperties[i]))
	//			return "";//neutralDialogues.GetRandomText ();
		
	//	return GameManager.GameData.DialogueNotFoundError;
	//}

 //   public void PerformTask (InteractableObject interactableObject, Property[] taskProperties)
	//{
	//	for (int i = 0; i < taskProperties.Length; i++)
	//	{
	//		if (interactableObject.HasProperty (taskProperties[i]))
	//		{
	//			GameManager.Instance.IncreasePlayerProperty (taskProperties[i]);
	//			continue;
	//		}

	//		if (interactableObject.PropertyIsDeadly (taskProperties[i]))
	//		{
	//			GameManager.Instance.FinishByDeath ();
	//			continue;
	//		}

	//		GameManager.Instance.DecreasePlayerProperty (taskProperties[i]);
	//	}
	//}

	public void ResetUnselectableList ()
	{
		unselectableObjects.Clear ();
	}

	private void Awake ()
	{
		unselectableObjects = new ();
	}
}*/

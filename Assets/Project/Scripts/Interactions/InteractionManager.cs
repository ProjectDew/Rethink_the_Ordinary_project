using System;
using System.Collections.Generic;

public class InteractionManager
{
	private readonly List<InteractableObject> unselectableObjects;

	private readonly Queue<Interaction> activeInteractions;
	
	public InteractableObject SelectedObject { get; private set; }

	public Interaction CurrentInteraction => activeInteractions.Peek ();

	public int TotalInteractions => activeInteractions.Count;

	public InteractionManager ()
	{
		unselectableObjects = new ();
		activeInteractions = new ();
	}

	public bool SelectObject (InteractableObject interactableObject)
	{
		if (interactableObject == null)
			throw new ArgumentNullException ("The interactable object is null.");

		if (unselectableObjects.Contains (interactableObject))
			return false;

		SelectedObject = interactableObject;

		return true;
	}

	public void AssignTask (Task task)
	{
		if (task == null)
			throw new ArgumentNullException ("The task is null.");

		if (SelectedObject == null)
			throw new ArgumentNullException ("The task cannot be assigned because no object was selected.");

		Interaction interaction = new (SelectedObject, task);

		activeInteractions.Enqueue (interaction);
			
		unselectableObjects.Add (SelectedObject);
	}
	
	public void PerformTask ()
	{
		activeInteractions.Dequeue ();
	}

	public void ResetInteractions ()
	{
		unselectableObjects.Clear ();
		activeInteractions.Clear ();
	}
}

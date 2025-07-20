using System;
using UnityEngine;

public class UIManager : ManagedBehaviour
{
	[SerializeField]
	private UIDialogue dialogueUI;

	[SerializeField]
	private UIRuntime runtimeUI;

	[SerializeField]
	private UITasks tasksUI;

	[SerializeField]
	private UIPause pauseUI;

	[SerializeField]
	private UIEnding endingUI;

	public UIDialogue DialogueUI => dialogueUI;

	public UIRuntime RuntimeUI => runtimeUI;

	public UITasks TasksUI => tasksUI;

	public UIPause PauseUI => pauseUI;

	public UIEnding EndingUI => endingUI;

	public override void ManagedOnEnable ()
	{
		runtimeUI.PauseGameEventHandler += PauseGame;
	}

	public override void ManagedStart ()
	{
		dialogueUI.Show ();
		runtimeUI.Hide ();
		tasksUI.Hide ();
		pauseUI.Hide ();
		endingUI.Hide ();
	}

	public override void ManagedOnDisable ()
	{
		runtimeUI.PauseGameEventHandler -= PauseGame;
	}

	private void PauseGame (object sender, EventArgs args)
	{
		pauseUI.Show ();
	}
}

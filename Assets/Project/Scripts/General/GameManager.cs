using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameData gameData;

	[SerializeField]
	private RoutineManager routine;

	[SerializeField]
	private DialogueManager dialogues;

	[SerializeField]
	private UIManager ui;

	[SerializeField]
	private ManagedBehaviour[] runtimeObjects;

	[SerializeField]
	private PlayerScript player;

	[SerializeField]
	private MainCharacter mainCharacter;

	[SerializeField]
	private CameraScript cameraScript;
	
	[SerializeField]
	private float timeLeftUpdateFrequency;

	[SerializeField]
	private int  runtimeTutorialIndex;

	private InteractionManager interactions;

	private GameManagerStates states;

	private float timeLeftUpdateCounter;

	public static GameManager Instance { get; private set; }

	public static GameData GameData { get; set; }

	public void StartTutorial ()
	{
		string dialogue = dialogues.GetFirstDialogue ("TUTORIAL_1");

		ui.DialogueUI.UpdateDialogue (dialogue, true);
		ui.DialogueUI.Show ();
		
		cameraScript.SetFixed ();

		states.SetTutorial ();
	}

	public void ContinueTutorial ()
	{
		string dialogueKey = "TUTORIAL_1";

		if (!dialogues.TryGetNextDialogue (dialogueKey, out string dialogue))
			return;

		ui.DialogueUI.UpdateDialogue (dialogue, true);

		if (dialogues.GetDialogueIndex (dialogueKey) < runtimeTutorialIndex)
			return;

		cameraScript.SetRuntime ();

		states.SetRuntime ();

		ui.RuntimeUI.Show ();
	}

	public void SelectObject (InteractableObject selectedObject)
	{
		if (!states.IsRuntime || ui.TasksUI.IsVisible)
			return;

		if (!interactions.SelectObject (selectedObject))
		{
			ui.DialogueUI.UpdateDialogue (dialogues.GetDialogue ("SELECTED_OBJECT"), true);
			return;
		}

		ui.TasksUI.UpdateTaskListLabel (selectedObject.Name);
		ui.TasksUI.UpdateTaskList (selectedObject, routine.GetCurrentTasks ());

		ui.TasksUI.Show ();
	}

	public void AssignTaskToSelectedObject (int taskIndex)
	{
		Task task = routine.GetTaskAt (taskIndex);

		if (task == null)
			return;

		interactions.AssignTask (task);

		if (interactions.TotalInteractions == 0)
			throw new Exception ("The interaction was not assigned properly");

		Interaction interaction = interactions.CurrentInteraction;

		bool objectHasDeadlyProperty = false;
		
		Property[] taskProperties = task.GetProperties ();

		for (int i = 0; i < taskProperties.Length; i++)
		{
			if (interaction.Object.PropertyIsDeadly (taskProperties[i]))
			{
				objectHasDeadlyProperty = true;
				break;
			}
		}

		string dialogueKey = objectHasDeadlyProperty ? "OBJECT_DEADLY_1" : "OBJECT_NEUTRAL_1";

		string dialogue = dialogues.GetDialogue (dialogueKey);

		ui.DialogueUI.UpdateDialogue (dialogue, true);

		ui.TasksUI.Hide ();
	}
	
	public void PerformTask (InteractableObject interactableObject)
	{
		if (interactions.TotalInteractions == 0)
			return;

		Interaction interaction = interactions.CurrentInteraction;

		if (interactableObject != interaction.Object)
			return;

		RoutineInfo routineInfo = routine.PerformTask (interaction.Task);

		Property[] taskProperties = interaction.Task.GetProperties ();

		for (int i = 0; i < taskProperties.Length; i++)
		{
			if (interactableObject.HasProperty (taskProperties[i]))
			{
				UpdatePlayerProperty (taskProperties[i], GameData.PropertyValueChange);
				continue;
			}

			if (interactableObject.PropertyIsDeadly (taskProperties[i]))
			{
				FinishByDeath ();
				continue;
			}

			UpdatePlayerProperty (taskProperties[i], -GameData.PropertyValueChange);
		}

		interactions.PerformTask ();

		if (routineInfo.WeekHasFinished)
			FinishSuccessfully ();
		else if (routineInfo.DayHasFinished)
			StartNewDay ();
	}

	public bool TryGetNextObjectPosition (out Vector3 position)
	{
		if (interactions.TotalInteractions == 0)
		{
			position = Vector3.zero;
			return false;
		}

		position = interactions.CurrentInteraction.Object.Position;

		return true;
	}

	public void FinishSuccessfully () => FinishGame (dialogues.GetDialogue ("SUCCESS"));

	public void FinishByTimeout () => FinishGame (dialogues.GetDialogue ("TIMEOUT"));

	public void FinishByDeath ()
	{
		if (interactions.TotalInteractions == 0)
		{
			FinishGame (dialogues.GetDialogue ("DEATH_GENERIC"));
			return;
		}

		Interaction interaction = interactions.CurrentInteraction;

		FinishGame (dialogues.GetDialogue ("DEATH", interaction.Task.Name.ToLower (), interaction.Object.Name.ToLower ()));
	}
	
	private void StartNewDay ()
	{
		routine.SetTimer (GameData.InitialTime);

		interactions.ResetInteractions ();

		// Show new day menu (currentDayIndex + 1)

		mainCharacter.ResetState ();
	}

	private void UpdateTimer ()
	{
		float timeLeft = routine.UpdateTimer (Time.deltaTime);

		timeLeftUpdateCounter += Time.deltaTime;

		if (timeLeftUpdateCounter > timeLeftUpdateFrequency)
		{
			ui.RuntimeUI.UpdateTimeLeft (timeLeft.ToString ("F0"));
			timeLeftUpdateCounter = 0;
		}
	}

	private void UpdatePlayerProperty (Property property, int valueDifference)
	{
		int newValue = player.UpdatePropertyValue (property, valueDifference);

		ui.RuntimeUI.UpdateProgressBar (property, newValue);

		if (newValue < 0 || newValue > GameData.PropertyDangerThreshold)
			FinishByDeath ();
	}

	private void FinishGame (string endingDialogue)
	{
		ui.DialogueUI.UpdateDialogue (endingDialogue, false);
		ui.DialogueUI.Show ();

		ui.EndingUI.Show ();

		states.SetGameOver ();
	}

	private void AcceptDialogue (object sender, EventArgs args)
	{
		if (states.IsTutorial)
			ContinueTutorial ();
		else if (states.IsRuntime)
			ui.DialogueUI.Hide ();
	}

	private void PauseGame (object sender, EventArgs args)
	{
		//cameraScript.SetFixed ();
		//states.SetPause ();
	}

	private void ResumeGame (object sender, EventArgs args)
	{
		cameraScript.SetRuntime ();
		states.SetRuntime ();
	}

	private void Awake ()
	{
		Instance = this;

		GameData = gameData;

		interactions = new ();

		states = GetComponent<GameManagerStates> ();

		for (int i = 0; i < runtimeObjects.Length; i++)
			runtimeObjects[i].ManagedAwake ();
	}

	private void OnEnable ()
	{
		for (int i = 0; i < runtimeObjects.Length; i++)
			runtimeObjects[i].ManagedOnEnable ();
		
		ui.RuntimeUI.PauseGameEventHandler += PauseGame;
		ui.DialogueUI.PressAcceptEventHandler += AcceptDialogue;
	}

	private void Start ()
	{
		for (int i = 0; i < runtimeObjects.Length; i++)
			runtimeObjects[i].ManagedStart ();

		ui.RuntimeUI.UpdateTimeLeft (GameData.InitialTime.ToString ("F0"));

		StartNewDay ();

		StartTutorial ();
	}

	private void Update ()
	{
		if (!states.IsRuntime)
			return;

		UpdateTimer ();

		for (int i = 0; i < runtimeObjects.Length; i++)
			runtimeObjects[i].ManagedUpdate ();
	}

	private void OnDisable ()
	{
		for (int i = 0; i < runtimeObjects.Length; i++)
			runtimeObjects[i].ManagedOnDisable ();
		
		ui.RuntimeUI.PauseGameEventHandler -= PauseGame;
		ui.DialogueUI.PressAcceptEventHandler -= AcceptDialogue;
	}
}

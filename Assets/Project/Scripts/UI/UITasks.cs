using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UITasks : UIBase
{
	private const string TASK_LIST_DESCRIPTION = "Asignar tarea";

	[SerializeField]
	private int totalTasksOnList;

	[SerializeField]
	private Sprite signMinusSprite;

	[SerializeField]
	private Sprite signPlusSprite;

	private Dictionary<int, VisualElement> taskContainers;

	private Dictionary<int, VisualElement> elementsContainers;
	private Dictionary<int, Label> taskNameLabels;

	private Dictionary<int, Button> acceptTaskButtons;

	private Label taskListLabel;
	private Button closeButton;

	public override void ManagedAwake ()
	{
		base.ManagedAwake ();

		taskListLabel = Root.Q<Label> ("TaskListLabel");
		closeButton = Root.Q<Button> ("CloseButton");

		UpdateTaskListLabel ("unknown_object");

		closeButton.clicked += Hide;

		taskContainers = new ();

		elementsContainers = new ();
		taskNameLabels = new ();

		acceptTaskButtons = new ();

		for (int i = 1; i <= totalTasksOnList; i++)
		{
			taskContainers.Add (i, Root.Q<VisualElement> ($"TaskContainer_{i}"));

			elementsContainers.Add (i, Root.Q<VisualElement> ($"ElementsContainer_{i}"));
			taskNameLabels.Add (i, Root.Q<Label> ($"TaskNameLabel_{i}"));

			acceptTaskButtons.Add (i, Root.Q<Button> ($"AcceptTaskButton_{i}"));
		}

		acceptTaskButtons[1].clicked += AssignTask_1;
		acceptTaskButtons[2].clicked += AssignTask_2;
		acceptTaskButtons[3].clicked += AssignTask_3;
		acceptTaskButtons[4].clicked += AssignTask_4;
		acceptTaskButtons[5].clicked += AssignTask_5;
		acceptTaskButtons[6].clicked += AssignTask_6;
		acceptTaskButtons[7].clicked += AssignTask_7;
	}

	public void UpdateTaskListLabel (string interactableObjectName) => taskListLabel.text = $"{interactableObjectName.ToUpper ()} - {TASK_LIST_DESCRIPTION}";

	public void UpdateTaskList (InteractableObject selectedObject, Task[] currentTasks)
	{
		for (int i = 1; i <= totalTasksOnList; i++)
		{
			if (i <= currentTasks.Length)
			{
				int taskIndex = i - 1;

				Task task = currentTasks[taskIndex];

				if (task != null)
				{
					taskNameLabels[i].text = task.Name;
					
					elementsContainers[i].Query<VisualElement> ()
						.ForEach (element => element.style.display = DisplayStyle.None);

					elementsContainers[i].style.display = DisplayStyle.Flex;

					DisplayElementIcons (elementsContainers[i], selectedObject, task);

					taskContainers[i].style.display = DisplayStyle.Flex;
				}
				else
				{
					taskContainers[i].style.display = DisplayStyle.None;
				}
			}
			else
			{
				taskContainers[i].style.display = DisplayStyle.None;
			}
		}
	}

	private void DisplayElementIcons (VisualElement elementsContainer, InteractableObject interactableObject, Task task)
	{
		Property[] taskProperties = task.GetProperties ();

		for (int i = 0; i < taskProperties.Length; i++)
		{
			bool objectHasProperty = interactableObject.HasProperty (taskProperties[i]);

			string propertyID = taskProperties[i].ID;
			
			VisualElement elementIcon = elementsContainer.Q<VisualElement> ($"{propertyID}Icon");
			VisualElement operatorIcon = elementsContainer.Q<VisualElement> ($"Operator{propertyID}Icon");
			
			elementIcon.style.display = DisplayStyle.Flex;
			operatorIcon.style.display = DisplayStyle.Flex;
			
			operatorIcon.style.backgroundImage = objectHasProperty ? new (signPlusSprite) : new (signMinusSprite);
		}
	}

	private void AssignTask_1 () => GameManager.Instance.AssignTaskToSelectedObject (0);

	private void AssignTask_2 () => GameManager.Instance.AssignTaskToSelectedObject (1);

	private void AssignTask_3 () => GameManager.Instance.AssignTaskToSelectedObject (2);

	private void AssignTask_4 () => GameManager.Instance.AssignTaskToSelectedObject (3);

	private void AssignTask_5 () => GameManager.Instance.AssignTaskToSelectedObject (4);

	private void AssignTask_6 () => GameManager.Instance.AssignTaskToSelectedObject (5);

	private void AssignTask_7 () => GameManager.Instance.AssignTaskToSelectedObject (6);
}

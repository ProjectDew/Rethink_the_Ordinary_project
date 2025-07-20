using UnityEngine;

public class Day : MonoBehaviour
{
	[SerializeField]
	private Task[] tasks;

	public int TotalTasks => tasks.Length;

	public Task[] GetTasks () => tasks;

	public RoutineInfo PerformTask (Task task)
	{
		int tasksLeft = 0;

		for (int i = 0; i < tasks.Length; i++)
		{
			if (tasks[i] == task)
				tasks[i] = null;
			
			if (tasks[i] != null)
				tasksLeft++;
		}

		return new RoutineInfo (tasksLeft == 0);
	}
}

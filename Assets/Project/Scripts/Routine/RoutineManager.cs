using System;
using UnityEngine;

public class RoutineManager : MonoBehaviour
{
	[SerializeField]
	private Week[] weeks;
	
	private float timeLeft;

	private int currentWeekIndex = 0;

	public Week CurrentWeek
	{
		get
		{
			if (currentWeekIndex >= weeks.Length)
				throw new IndexOutOfRangeException ("There are no more weeks left.");

			return (weeks[currentWeekIndex] != null) ? weeks[currentWeekIndex] : null;
		}
	}

	public int TotalWeeks => weeks.Length;

	public Task[] GetCurrentTasks () => CurrentWeek.CurrentDay.GetTasks ();

	public Task GetTaskAt (int taskIndex)
	{
		Task[] tasks = GetCurrentTasks ();

		if (taskIndex < 0 || taskIndex >= tasks.Length)
			throw new IndexOutOfRangeException ("The index is out of the bounds of the tasks list.");

		return (tasks[taskIndex] != null) ? tasks[taskIndex] : null;
	}

	public RoutineInfo PerformTask (Task task)
	{
		if (task == null)
			throw new ArgumentNullException ("The task provided is null.");
		
		if (CurrentWeek == null)
			throw new Exception ($"The current week is null. Please remove it or add one in the inspector. Week index: {currentWeekIndex}");

		UpdateTimer (task.Duration);

		RoutineInfo routineInfo = CurrentWeek.PerformTask (task);

		if (routineInfo.WeekHasFinished)
			currentWeekIndex++;

		bool routineHasFinished = currentWeekIndex >= weeks.Length;

		return !routineHasFinished ? routineInfo : new RoutineInfo (routineInfo.DayHasFinished, routineInfo.WeekHasFinished, routineHasFinished);
	}

	public void SetTimer (float timeLeft) => this.timeLeft = timeLeft;

	public float UpdateTimer (float decreaseValue)
	{
		timeLeft -= decreaseValue;

		if (timeLeft < 0)
		{
			GameManager.Instance.FinishByTimeout ();
			timeLeft = 1000;
		}

		return timeLeft;
	}
}

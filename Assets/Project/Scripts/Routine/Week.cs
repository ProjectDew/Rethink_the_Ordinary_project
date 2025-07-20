using System;
using UnityEngine;

public class Week : MonoBehaviour
{
	[SerializeField]
	private Day[] days;

	private int currentDayIndex = 0;

	public Day CurrentDay
	{
		get
		{
			if (currentDayIndex >= days.Length)
				throw new IndexOutOfRangeException ("The week has already finished.");

			return (days[currentDayIndex] != null) ? days[currentDayIndex] : null;
		}
	}

	public int TotalDays => days.Length;

	public RoutineInfo PerformTask (Task task)
	{
		if (CurrentDay == null)
			throw new Exception ($"The current day is null. Please remove it or add one in the inspector. Day index: {currentDayIndex}");

		RoutineInfo routineInfo = CurrentDay.PerformTask (task);

		if (routineInfo.DayHasFinished)
			currentDayIndex++;

		bool weekHasFinished = currentDayIndex >= days.Length;

		return !weekHasFinished ? routineInfo : new RoutineInfo (routineInfo.DayHasFinished, weekHasFinished);
	}
}

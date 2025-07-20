using UnityEngine;

public class CharacterStates : StateManager
{
	[SerializeField]
	private State movingToNextSpot;

	[SerializeField]
	private State performingTask;

	[SerializeField]
	private State waiting;

	public bool IsMovingToNextSpot => CurrentState == movingToNextSpot;

	public bool IsPerformingTask => CurrentState == performingTask;

	public bool IsWaiting => CurrentState == waiting;

	public void SetMovingToNextSpot () => SetState (movingToNextSpot);

	public void SetPerformingTask () => SetState (performingTask);

	public void SetWaiting () => SetState (waiting);
}

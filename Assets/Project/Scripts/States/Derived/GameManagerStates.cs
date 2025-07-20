using UnityEngine;

public class GameManagerStates : StateManager
{
	[SerializeField]
	private State tutorial;
	
	[SerializeField]
	private State runtime;

	[SerializeField]
	private State pause;

	[SerializeField]
	private State gameOver;

	public bool IsTutorial => CurrentState == tutorial;

	public bool IsRuntime => CurrentState == runtime;

	public bool IsPause => CurrentState == pause;

	public bool IsGameOver => CurrentState == gameOver;

	public void SetTutorial () => SetState (tutorial);

	public void SetRuntime () => SetState (runtime);

	public void SetPause () => SetState (pause);

	public void SetGameOver () => SetState (gameOver);
}

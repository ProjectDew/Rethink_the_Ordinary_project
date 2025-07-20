using UnityEngine;

public class CameraStates : StateManager
{
	[SerializeField]
	private State idle;

	[SerializeField]
	private State runtime;

	[SerializeField]
	private State menu;

	public bool IsIdle => CurrentState == idle;

	public bool IsRuntime => CurrentState == runtime;

	public bool IsMenu => CurrentState == menu;

	public void SetIdle () => SetState (idle);

	public void SetRuntime () => SetState (runtime);

	public void SetMenu () => SetState (menu);
}

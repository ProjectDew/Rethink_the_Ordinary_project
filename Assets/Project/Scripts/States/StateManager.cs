using UnityEngine;

public class StateManager : MonoBehaviour
{
	private State currentState;

	public State CurrentState
	{
		get
		{
			if (currentState == null)
				return null;

			return currentState;
		}

		set => currentState = value;
	}

	public void SetState (State state) => CurrentState = state;
}

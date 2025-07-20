using UnityEngine;
using UnityEngine.AI;

public class MainCharacter : ManagedBehaviour
{
	private CharacterStates states;

	private NavMeshAgent navMeshAgent;
	private Animator animator;

	private Transform tr;

	private Vector3 initialPosition;

	public void ResetState ()
	{
		tr.position = initialPosition;

		UpdateNavMeshAgent (tr.position, 0);

		states.SetWaiting ();
	}

	public override void ManagedAwake ()
	{
		states = GetComponent<CharacterStates> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();

		tr = transform;

		initialPosition = tr.position;

		states.SetWaiting ();
	}

	public override void ManagedUpdate ()
	{
		if (states.IsWaiting)
			WaitForNextDestination ();
	}

	private void WaitForNextDestination ()
	{
		if (!GameManager.Instance.TryGetNextObjectPosition (out Vector3 destination))
			return;

		UpdateNavMeshAgent (destination, GameManager.GameData.CharacterSpeed);

		states.SetMovingToNextSpot ();
	}

	private void UpdateNavMeshAgent (Vector3 destination, float speed)
	{
		navMeshAgent.destination = destination;

		navMeshAgent.speed = speed;

		animator.SetFloat ("Speed", speed);
	}

	private void OnCollisionEnter (Collision obj)
	{
		InteractableObject interactableObject = obj.collider.GetComponentInParent<InteractableObject> ();

		GameManager.Instance.PerformTask (interactableObject);
		
		UpdateNavMeshAgent (tr.position, 0);
		
		states.SetWaiting ();
	}
}

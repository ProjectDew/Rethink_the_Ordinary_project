using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIEnding : UIBase
{
	public override void ManagedAwake ()
	{
		base.ManagedAwake ();
		//
	}

	private void RestartGame ()
	{
		SceneManager.LoadScene ("Game");
	}
}

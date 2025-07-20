using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIPause : UIBase
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

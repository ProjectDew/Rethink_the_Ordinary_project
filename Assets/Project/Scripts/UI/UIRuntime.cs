using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIRuntime : UIBase
{
	[SerializeField]
	private Property[] properties;

	private Label timeLeftLabel;
	private Button pauseButton;

	private Dictionary<Property, ProgressBar> propertyBars;

	public event EventHandler PauseGameEventHandler;

	public override void ManagedAwake ()
	{
		base.ManagedAwake ();

		timeLeftLabel = Root.Q<Label> ("TimeLeftLabel");
		pauseButton = Root.Q<Button> ("PauseButton");

		pauseButton.clicked += OnPauseGame;

		propertyBars = new ();

		for (int i = 0; i < properties.Length; i++)
		{
			ProgressBar progressBar = Root.Q<ProgressBar> ($"{properties[i].ID}Bar");
			propertyBars.Add (properties[i], progressBar);
		}
	}

	public void UpdateProgressBar (Property property, int newValue)
	{
		if (!propertyBars.ContainsKey (property))
			return;

		propertyBars[property].value = newValue;
	}

	public void UpdateTimeLeft (string timeLeft) => timeLeftLabel.text = timeLeft;

	private void OnPauseGame ()
	{
		PauseGameEventHandler?.Invoke (this, EventArgs.Empty);
	}
}

using System;
using UnityEngine.UIElements;

public class UIDialogue : UIBase
{
	private Label dialogueLabel;
	private Button dialogueAcceptButton;

	public event EventHandler PressAcceptEventHandler;

	public override void ManagedAwake ()
	{
		base.ManagedAwake ();
		
		dialogueLabel = Root.Q<Label> ("DialogueLabel");
		dialogueAcceptButton = Root.Q<Button> ("DialogueAcceptButton");

		dialogueAcceptButton.clicked += OnPressAccept;
	}

	public void UpdateDialogue (string dialogue, bool displayAcceptButton)
	{
		dialogueLabel.text = dialogue;
		dialogueAcceptButton.style.display = displayAcceptButton ? DisplayStyle.Flex : DisplayStyle.None;

		Show ();
	}

	private void OnPressAccept ()
	{
		PressAcceptEventHandler?.Invoke (this, EventArgs.Empty);
	}
}

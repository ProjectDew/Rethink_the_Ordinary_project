using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	private delegate string GetDialogueDelegate (DialogueGroup group);

	[SerializeField]
	private TextAsset dialoguesDocument;
	
	[SerializeField]
	private string dialogueNotFound;

	private Dictionary<string, Dialogue> dialogues;

	private List<DialogueGroup> groupedDialogues;

	public int GetDialogueIndex (string key) => TryGetDialogueGroup (key, out DialogueGroup group) ? group.DialogueIndex : -1;

	public string GetDialogue (string key, params string[] formatValues)
	{
		if (dialogues.TryGetValue (key, out Dialogue dialogue))
			return dialogue.GetContent (formatValues);
		
		throw new KeyNotFoundException ($"The key was not found. Key: {key}");
	}

	public string GetDialogueAt (string key, int index, params string[] formatValues) => GetDialogueInGroup (key, group => group.GetDialogueAt (index, formatValues));

	public string GetFirstDialogue (string key, params string[] formatValues) => GetDialogueInGroup (key, group => group.GetFirstDialogue (formatValues));

	public string GetNextDialogue (string key, params string[] formatValues) => GetDialogueInGroup (key, group => group.GetNextDialogue (formatValues));

	public string GetLastDialogue (string key, params string[] formatValues) => GetDialogueInGroup (key, group => group.GetLastDialogue (formatValues));

	public string GetRandomDialogue (string key, params string[] formatValues) => GetDialogueInGroup (key, group => group.GetRandomDialogue (formatValues));

	public bool TryGetDialogue (string key, out string dialogue, params string[] formatValues)
	{
		bool keyExists = dialogues.TryGetValue (key, out Dialogue dialogueObject);
		
		dialogue = keyExists ? dialogueObject.GetContent (formatValues) : dialogueNotFound;

		return keyExists;
	}

	public bool TryGetDialogueAt (string key, int index, out string dialogue, params string[] formatValues)
	{
		if (TryGetDialogueGroup (key, out DialogueGroup group))
			return group.TryGetDialogueAt (index, out dialogue, formatValues);
		
		return TryGetDialogue (key, out dialogue, formatValues);
	}

	public bool TryGetFirstDialogue (string key, out string dialogue, params string[] formatValues)
	{
		if (TryGetDialogueGroup (key, out DialogueGroup group))
			return group.TryGetFirstDialogue (out dialogue, formatValues);
		
		return TryGetDialogue (key, out dialogue, formatValues);
	}

	public bool TryGetNextDialogue (string key, out string dialogue, params string[] formatValues)
	{
		if (TryGetDialogueGroup (key, out DialogueGroup group))
			return group.TryGetNextDialogue (out dialogue, formatValues);
		
		return TryGetDialogue (key, out dialogue, formatValues);
	}

	public bool TryGetLastDialogue (string key, out string dialogue, params string[] formatValues)
	{
		if (TryGetDialogueGroup (key, out DialogueGroup group))
			return group.TryGetLastDialogue (out dialogue, formatValues);
		
		return TryGetDialogue (key, out dialogue, formatValues);
	}

	public bool TryGetRandomDialogue (string key, out string dialogue, params string[] formatValues)
	{
		if (TryGetDialogueGroup (key, out DialogueGroup group))
			return group.TryGetRandomDialogue (out dialogue, formatValues);
		
		return TryGetDialogue (key, out dialogue, formatValues);
	}

	private void GroupDialogues ()
	{
		groupedDialogues = new ();

		foreach (KeyValuePair<string, Dialogue> dialogue in dialogues)
		{
			string key = dialogue.Key;

			int numberIndex = -1;

			for (int i = key.Length - 1; i >= 0; i--)
			{
				if (!char.IsDigit (key[i]))
					break;

				numberIndex = i;
			}

			if (numberIndex >= 0)
				AddDialogueToGroup (key, numberIndex, dialogue.Value);
		}
	}

	private void AddDialogueToGroup (string key, int numberIndex, Dialogue dialogue)
	{
		string keyBase = key[..numberIndex];

		int keyNumber = int.Parse (key[numberIndex..]);

		foreach (DialogueGroup group in groupedDialogues)
		{
			if (group.ContainsKey (keyBase))
			{
				group.Add (keyNumber, dialogue);
				return;
			}
		}

		DialogueGroup dialogueGroup = new (keyBase, keyNumber, dialogue);

		groupedDialogues.Add (dialogueGroup);
	}

	private bool TryGetDialogueGroup (string key, out DialogueGroup group)
	{
		for (int i = 0; i < groupedDialogues.Count; i++)
		{
			if (groupedDialogues[i].ContainsKey (key))
			{
				group = groupedDialogues[i];
				return true;
			}
		}

		group = null;

		return false;
	}

	private string GetDialogueInGroup (string key, GetDialogueDelegate getDialogue, params string[] formatValues)
	{
		if (TryGetDialogueGroup (key, out DialogueGroup group))
			return getDialogue (group);
		
		return GetDialogue (key, formatValues);
	}

	private void Awake ()
	{
		dialogues = DialogueReader.Read (dialoguesDocument);
		GroupDialogues ();
	}
}

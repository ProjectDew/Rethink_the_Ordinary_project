using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueGroup : IEnumerable<Dialogue>
{
	private struct KeyDialoguePair
	{
		public string Key { get; private set; }

		public Dialogue Dialogue { get; private set; }

		public KeyDialoguePair (string key, Dialogue dialogue)
		{
			Key = key;
			Dialogue = dialogue;
		}
	}

	private const string numberNotFoundMessage = "The number provided was not found in the group. Number: {0}.";

	private readonly Dictionary<int, KeyDialoguePair> dialogues = new ();

	private readonly string keyBase;

	public int DialogueIndex { get; private set; }

	public int TotalDialogues => dialogues.Count;

	private int FirstIndex { get; set; }

	private int LastIndex { get; set; }

	private int RandomIndex => new Random ().Next (FirstIndex, LastIndex + 1);

	public Dialogue this[int index]
	{
		get => dialogues.ContainsKey (index) ? dialogues[index].Dialogue : throw new ArgumentException (string.Format (numberNotFoundMessage, index));

		set
		{
			if (!dialogues.ContainsKey (index))
				throw new ArgumentException (string.Format (numberNotFoundMessage, index));

			KeyDialoguePair currentValue = dialogues[index];

			dialogues[index] = new (currentValue.Key, value);
		}
	}

	public DialogueGroup (string keyBase, int keyNumber, Dialogue dialogue)
	{
		this.keyBase = keyBase;
		Add (keyNumber, dialogue);
	}

	public DialogueGroup (string keyBase, int keyNumber, string content) : this (keyBase, keyNumber, new Dialogue (content)) { }

	public IEnumerator<Dialogue> GetEnumerator ()
	{
		foreach (KeyValuePair<int, KeyDialoguePair> item in dialogues)
			yield return item.Value.Dialogue;
	}

	IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

	public bool Contains (int dialogueNumber) => dialogues.ContainsKey (dialogueNumber);

	public bool ContainsKey (string key)
	{
		if (key == keyBase)
			return true;
		
		foreach (KeyValuePair<int, KeyDialoguePair> item in dialogues)
			if (item.Value.Key == key)
				return true;

		return false;
	}

	public bool ContainsDialoogue (Dialogue dialogue)
	{
		foreach (KeyValuePair<int, KeyDialoguePair> item in dialogues)
			if (item.Value.Dialogue == dialogue)
				return true;

		return false;
	}

	public string GetDialogueAt (int index, params string[] formatValues) => this[DialogueIndex = index].GetContent (formatValues);

	public string GetFirstDialogue (params string[] formatValues) => this[DialogueIndex = FirstIndex].GetContent (formatValues);

	public string GetNextDialogue (params string[] formatValues) => this[++DialogueIndex].GetContent (formatValues);

	public string GetLastDialogue (params string[] formatValues) => this[DialogueIndex = LastIndex].GetContent (formatValues);

	public string GetRandomDialogue (params string[] formatValues) => this[DialogueIndex = RandomIndex].GetContent (formatValues);

	public bool TryGetDialogueAt (int index, out string dialogue, params string[] formatValues)
	{
		try
		{
			dialogue = this[DialogueIndex = index].GetContent (formatValues);
			return true;
		}
		catch (ArgumentException)
		{
			dialogue = null;
			return false;
		}
	}

	public bool TryGetFirstDialogue (out string dialogue, params string[] formatValues) => TryGetDialogueAt (FirstIndex, out dialogue, formatValues);

	public bool TryGetNextDialogue (out string dialogue, params string[] formatValues) => TryGetDialogueAt (DialogueIndex + 1, out dialogue, formatValues);

	public bool TryGetLastDialogue (out string dialogue, params string[] formatValues) => TryGetDialogueAt (LastIndex, out dialogue, formatValues);

	public bool TryGetRandomDialogue (out string dialogue, params string[] formatValues) => TryGetDialogueAt (RandomIndex, out dialogue, formatValues);

	public void Add (int dialogueNumber, Dialogue dialogue)
	{
		if (dialogues.ContainsKey (dialogueNumber))
			throw new ArgumentException ($"The number provided is already in the group. Number: {dialogueNumber}.");

		KeyDialoguePair keyDialoguePair = new ($"{keyBase}{dialogueNumber}", dialogue);

		dialogues.Add (dialogueNumber, keyDialoguePair);

		ResetIndices ();
	}

	public void Add (int index, string content) => Add (index, new Dialogue (content));

	public void Remove (int index)
	{
		dialogues.Remove (index);
		ResetIndices ();
	}

	public void Clear () => dialogues.Clear ();

	private void ResetIndices ()
	{
		FirstIndex = int.MaxValue;
		LastIndex = 0;

		foreach (KeyValuePair<int, KeyDialoguePair> item in dialogues)
		{
			if (item.Key < FirstIndex)
				FirstIndex = item.Key;

			if (item.Key > LastIndex)
				LastIndex = item.Key;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DialogueReader
{
	public static Dictionary<string, Dialogue> Read (string filePath)
	{
		if (filePath == null || filePath.Length == 0)
			throw new ArgumentException ($"Invalid path: {filePath}");
			
		if (!filePath.EndsWith (".csv"))
			throw new FormatException ($"The file has an invalid extension: {filePath}");

		if (!File.Exists (filePath))
		{
			FileStream fs = File.Create (filePath);
			fs.Dispose ();
		}

		string content = File.ReadAllText (filePath);

		return GetContent (content);
	}

	public static Dictionary<string, Dialogue> Read (TextAsset doc)
	{
		if (doc == null)
			throw new ArgumentNullException ("The document provided is null.");

		string content = doc.text;
		
		return GetContent (content);
	}

	private static Dictionary<string, Dialogue> GetContent (string content)
	{
		Dictionary<string, Dialogue> dialogues = new ();

		if (content == null || content.Length == 0)
			return dialogues;

		int commaIndex = 0;
		int keyIndex = 0;

		int lineBreakIndex;
		int valueIndex;

		while (commaIndex >= 0)
		{
			commaIndex = content.IndexOf (',', keyIndex);

			if (commaIndex < 0)
				return dialogues;

			lineBreakIndex = content.IndexOf ('\n', keyIndex);
			
			if (lineBreakIndex < 0)
				lineBreakIndex = content.Length - 1;

			valueIndex = commaIndex + 1;

			string key = content[keyIndex..commaIndex].Trim ();
			string value = content[valueIndex..lineBreakIndex].Trim ();

			Dialogue dialogue = new (value.Replace (@"\n", "\n"));

			dialogues.Add (key, dialogue);

			keyIndex = lineBreakIndex + 1;
		};

		return dialogues;
	}
}

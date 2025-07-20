using System.Text;

public class Dialogue
{
	private readonly string content;

	private readonly StringBuilder formattedContent;

	public Dialogue (string content)
	{
		this.content = content;
		formattedContent = new StringBuilder (content);
	}

	public string GetContent (params string[] values)
	{
		if (values == null || values.Length == 0)
			return formattedContent.ToString ();

		formattedContent.Clear ();
		formattedContent.Append (content);

		for (int i = 0; i < values.Length; i++)
			formattedContent.Replace ($"{{{i}}}", values[i]);

		return formattedContent.ToString ();
	}
}

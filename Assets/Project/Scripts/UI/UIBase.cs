using UnityEngine.UIElements;

public abstract class UIBase : ManagedBehaviour
{
	private UIDocument uiDocument;

	public virtual bool IsVisible => Root.style.display == DisplayStyle.Flex;

	protected virtual UIDocument UIDocument => uiDocument;

	protected virtual VisualElement Root { get; private set; }

	public override void ManagedAwake ()
	{
		uiDocument = GetComponent<UIDocument> ();
		Root = uiDocument.rootVisualElement;
	}

	public virtual void Show ()
	{
		Root.style.display = DisplayStyle.Flex;
	}

	public virtual void Hide ()
	{
		Root.style.display = DisplayStyle.None;
	}
}

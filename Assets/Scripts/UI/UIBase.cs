using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public abstract class UIBase : MonoBehaviour
{
	private protected VisualElement _root;
	private protected UIDocument _doc;

	public virtual void Awake()
	{
		_doc = GetComponent<UIDocument>();
		_root = _doc.rootVisualElement;
		_root.style.display = DisplayStyle.None;
	}

	public virtual void Show() { _root.style.display = DisplayStyle.Flex; }
	public virtual void Hide() { _root.style.display = DisplayStyle.None; }
}

using UnityEngine;
using System.Collections;

public class GoToScript : MonoBehaviour {
	
	[SerializeField]
	private CameraGUIMutliMenuScript _menuScript;
	[SerializeField]
	public CameraGUIMutliMenuScript.Position _subMenu;

	void OnMouseUp ()
	{
		_menuScript.GoToPosition(_subMenu);
	}
	
	public CameraGUIMutliMenuScript.Position SubMenu {
		get {
			return this._subMenu;
		}
		set {
			_subMenu = value;
		}
	}
}

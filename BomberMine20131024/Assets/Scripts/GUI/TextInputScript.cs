using UnityEngine;
using System.Collections;

public class TextInputScript : MonoBehaviour {
	
	private bool _focus = false;
	private TextAreaScript _tas;
	public TextInputScript[] _autresTextInputScripts;
	
	void Start()
	{
		_tas = GetComponent<TextAreaScript>();
	}
	
	void Update()
	{
		if(Focus)
		{
			string inputString = Input.inputString;
			if(Input.GetKeyUp(KeyCode.Backspace)) _tas.EffacerDerniereLettre();
			else if(inputString != "" && inputString != "\b")
			{
				_tas.Append(inputString);
			}
		}
	}
	
	public bool Focus {
		get {
			return this._focus;
		}
		set {
			_focus = value;
			_tas.SetFocused(_focus);
		}
	}	
	
	
	void OnMouseDown()
	{
		for(int i=0; i<_autresTextInputScripts.Length; i++)
		{
			_autresTextInputScripts[i].Focus = false;
		}
		Focus = true;
	}
}

using UnityEngine;
using System.Collections;

public class MouseHorizontal : MonoBehaviour {


	private float _mouseSensitivity;
	private float _upDownRange;
	private float _rotLeftRight;	
	
	void Start () {
		
		_rotLeftRight = 0;
		_mouseSensitivity = 80.0f;
		_upDownRange = 60.0f;

	}
	
	void Update () {
		
		_rotLeftRight = Input.GetAxis ("Mouse X") * _mouseSensitivity * Time.deltaTime;
		transform.Rotate(0,_rotLeftRight,0);
		
		var forward = (_rotLeftRight * 
        	Vector3.forward).normalized;
		var leftward = (_rotLeftRight * 
        	Vector3.left).normalized;
	
	}
}

using UnityEngine;
using System.Collections;

public class MineDeclenchementScript : MonoBehaviour {
	
	public MineScript _mineScript;
	
	void OnTriggerExit() {
		_mineScript.Explode();
	}
}

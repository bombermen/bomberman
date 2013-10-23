using UnityEngine;
using System.Collections;

public class MineDeclenchementScript : MonoBehaviour {
	
	public MineScript _mineScript;
	
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.layer == 8)
		{
			Debug.Log("explosion");
			_mineScript.Explode();
		}
	}
}

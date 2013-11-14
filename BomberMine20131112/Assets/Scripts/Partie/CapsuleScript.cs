using UnityEngine;
using System.Collections;

public class CapsuleScript : MonoBehaviour {
	
	public NetworkScript _networkScript;
	public static int _nbCapsules = 0;
	private int _idCapsule = 0;
	private bool _enAttenteDeConfirmation = false;
	
	// Use this for initialization
	void Start ()
	{
		_idCapsule = _nbCapsules;
		_nbCapsules++;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(Network.isServer && col.gameObject.layer == 8)
		{
			//int playerIndex = col.transform.parent.GetComponent<JoueurScript>().PlayerIndex;
			_networkScript.EntrerDansCapsule(col.transform, _idCapsule);
		}
	}
}

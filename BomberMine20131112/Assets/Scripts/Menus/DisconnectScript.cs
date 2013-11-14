using UnityEngine;
using System.Collections;

public class DisconnectScript : MonoBehaviour {
	
	void OnMouseUp ()
	{
		Network.Disconnect(250);
	}
}

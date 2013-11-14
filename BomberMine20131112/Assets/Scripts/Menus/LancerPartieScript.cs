using UnityEngine;
using System.Collections;

public class LancerPartieScript : MonoBehaviour {
	
	void OnMouseUp ()
	{
		if(Network.isServer)
		{
			this.networkView.RPC("LancerPartie", RPCMode.Others);
            LancerPartie();
		}
	}
	
	[RPC]
	void LancerPartie()
	{
		Application.LoadLevel(2);
	}
}

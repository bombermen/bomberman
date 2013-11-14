using UnityEngine;
using System.Collections;

public class FoudroimentScript : MonoBehaviour {

    [SerializeField]
    private GameObject _map;
    [SerializeField]
    private NetworkScript _networkScript;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8)
        {
            if (Network.isServer)
            {
                _networkScript.DemanderFoudroiement(col.transform);
            }
            else
            {
                _networkScript.networkView.RPC("DemanderFoudroiement", RPCMode.Server, col.transform);
            }
            this.gameObject.SetActive(false);
        }
    }

}

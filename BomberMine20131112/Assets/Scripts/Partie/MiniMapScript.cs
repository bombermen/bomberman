using UnityEngine;
using System.Collections;

public class MiniMapScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _objetsAFoudroyer;
    private int choixDeLObjet = 0;
    private int clic = 0;
    private float _seconds = 10;
    private bool _foudroiementActif = false;

    public bool FoudroiementActif
    {
        get { return _foudroiementActif; }
        set { _foudroiementActif = value; }
    }
    [SerializeField]
    private NetworkScript _networkScript;

    void Update()
    {
        if (_foudroiementActif)
        {
            if (Input.GetButtonDown("Fire1") && clic < 5)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray))
                {
                    GameObject bomb = Network.Instantiate(_objetsAFoudroyer[choixDeLObjet], ray.GetPoint(10), transform.rotation, 0) as GameObject;
                    bomb.transform.GetChild(0).GetComponent<BombScript>()._networkScript = _networkScript;
                    clic++;
                }

            }
            //StartCoroutine(Declencher(_seconds));
        }
    }
}

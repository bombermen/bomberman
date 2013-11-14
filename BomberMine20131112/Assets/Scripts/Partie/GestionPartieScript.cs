using UnityEngine;
using System.Collections;

public class GestionPartieScript : MonoBehaviour {
	
	public NetworkScript _networkScript;
	
	private float _tempsPhase1 = 10f;
	private float _tempsPhase2 = 2f;
	private int _sequenceActuelle = 1;
	
	// Use this for initialization
	void Start () {
		
		StartCoroutine(Phase1());
	}
	
	private IEnumerator Phase1()
	{
		Debug.Log("Phase1");
		yield return new WaitForSeconds(_tempsPhase1);
		StartCoroutine(Phase2());
	}

	private IEnumerator Phase2()
	{
		Debug.Log("Phase2");
		yield return new WaitForSeconds(_tempsPhase2);
		
		if(_sequenceActuelle < 4)
		{
			_networkScript.ExploserBombeH();
			_networkScript.ReinitialiserCapsules();
			_sequenceActuelle++;
			StartCoroutine(Phase1());
		}
		else
		{
			Application.LoadLevel(0);
		}
	}
}

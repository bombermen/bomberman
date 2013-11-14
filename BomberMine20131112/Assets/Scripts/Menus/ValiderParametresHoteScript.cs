using UnityEngine;
using System.Collections;

public class ValiderParametresHoteScript : MonoBehaviour {
	
	[SerializeField]
	private ManagerScript _manager;
	[SerializeField]
	private TextAreaScript _inputNomJoueur;
	[SerializeField]
	private TextAreaScript _inputNomServeur;
	[SerializeField]
	private RadioButtonScript _equipe;
	[SerializeField]
	private CheckBoxScript _tirAmi;
	[SerializeField]
	private GoToScript _boutonRetourSalleAttente;
	[SerializeField]
	private TextAreaScript _sousTitreSalleAttente;
	[SerializeField]
	private CameraGUIMutliMenuScript _menuScript;
	[SerializeField]
	private TextAreaScript _errorOutput;
	[SerializeField]
	private GameObject _boutonLancerPartie;
	
	void OnMouseUp()
	{
        Debug.Log("initialisation du serveur");
		Network.InitializeServer(4, 6600, false);

		_boutonRetourSalleAttente.SubMenu = CameraGUIMutliMenuScript.Position.PARAMETRAGE_HOTE;
		_sousTitreSalleAttente.SetText("Serveur " + _manager.AdresseHote);

		_boutonLancerPartie.SetActive(true);
        _menuScript.GoToPosition(CameraGUIMutliMenuScript.Position.SALLE_ATTENTE);
		_manager.SetPlayerName(Network.player, _inputNomJoueur.GetText());
	}
}

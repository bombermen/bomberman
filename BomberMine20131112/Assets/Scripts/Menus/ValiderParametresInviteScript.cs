using UnityEngine;
using System.Collections;

public class ValiderParametresInviteScript : MonoBehaviour {
	
	[SerializeField]
	private GlassPaneScript _glassPane;
	[SerializeField]
	private TextAreaScript _textAreaError;
	[SerializeField]
	private ManagerScript _manager;
    [SerializeField]
    private TextAreaScript _ipServeur;
	[SerializeField]
	private TextAreaScript _inputNomJoueur;
	[SerializeField]
	private GoToScript _boutonRetourSalleAttente;
	[SerializeField]
	private TextAreaScript _sousTitreSalleAttente;
	[SerializeField]
	private GameObject _boutonLancerPartie;
	[SerializeField]
	private CameraGUIMutliMenuScript _multiMenu;
	
	private bool _connected = false;
	
	void OnMouseUp ()
	{
        Network.Connect(_ipServeur.GetText(), 6600);
		_glassPane.StartGlassPane();
	}

	void OnFailedToConnect (NetworkConnectionError error)
	{
		_textAreaError.SetText("La connexion au serveur est impossible");
		_glassPane.StopGlassPane();
	}

	void OnConnectedToServer ()
	{
		_textAreaError.SetText("");
		_boutonRetourSalleAttente.SubMenu = CameraGUIMutliMenuScript.Position.PARAMETRAGE_INVITE;
		_sousTitreSalleAttente.SetText("Invité");
		_boutonLancerPartie.SetActive(false);
		_multiMenu.GoToPosition(CameraGUIMutliMenuScript.Position.SALLE_ATTENTE);
		_manager.networkView.RPC ("SetPlayerName", RPCMode.Server, Network.player, _inputNomJoueur.GetText());
		_connected = true;
		_glassPane.StopGlassPane();
	}
}

using UnityEngine;
using System.Collections;

public class ManagerScriptOld : MonoBehaviour {
		
	private string _adresseHote;
	private string _nomServeur;
	private string _nomJoueur;
	
	private bool _equipe;
	private bool _tirAmi;
	
	private PlayerScript[] _players;
	
	[SerializeField]
	private TextAreaScript[] _playerInfo;
	
	void Start ()
	{
		DontDestroyOnLoad(this);
		_players = new PlayerScript[4];
		for(int i=0; i<4; i++)
			_players[i] = new PlayerScript();
	}
	
	public void SetServer(string adresseHote, string nomServeur, string nomJoueur, bool equipe, bool tirAmi)
	{
		AdresseHote = adresseHote;
		NomServeur = nomServeur;
		NomJoueur = nomJoueur;
		Equipe = equipe;
		TirAmi = tirAmi;
	}
	
	public void SetClient(string adresseHote, string nomJoueur)
	{
		AdresseHote = adresseHote;
		NomJoueur = nomJoueur;
	}

	public string AdresseHote
	{
		get { return this._adresseHote; }
		set { _adresseHote = value; }
	}
	
	public string NomJoueur {
		get { return this._nomJoueur; }
		set { _nomJoueur = value; }
	}

	public string NomServeur {
		get { return this._nomServeur; }
		set { _nomServeur = value; }
	}
	
	public bool Equipe {
		get { return this._equipe;
		}
		set { _equipe = value;
		}
	}

	public bool TirAmi {
		get { return this._tirAmi;
		}
		set { _tirAmi = value;
		}
	}
	
	void OnServerInitialized(NetworkPlayer player) 
	{
		this.networkView.RPC("NotifyNewPlayer", RPCMode.AllBuffered, player);
	}

    void OnPlayerConnected(NetworkPlayer player)
    {
		this.networkView.RPC("NotifyNewPlayer", RPCMode.AllBuffered, player);
	}
	
	[RPC]
	void NotifyNewPlayer(NetworkPlayer p)
	{
		if (!_players[0].Initialized)
		{
			_players[0].NetPlayer = p;
			_players[0].Initialized = true;
			_players[0].Team = 0;
		}
		else if (!_players[1].Initialized)
		{
			_players[1].NetPlayer = p;
			_players[1].Initialized = true;
			_players[1].Team = 0;
		}
		else if (!_players[2].Initialized)
		{
			_players[2].NetPlayer = p;
			_players[2].Initialized = true;
			_players[2].Team = 1;
		}
		else if (!_players[3].Initialized)
		{
			_players[3].NetPlayer = p;
			_players[3].Initialized = true;
			_players[3].Team = 1;
		}
	}
	
	public void SetTeam(int team)
	{
		if(Network.isServer)
		{
			ChangeTeam(Network.player, team);
		}
		else
		{
			this.networkView.RPC("ChangeTeam", RPCMode.Server, Network.player, team);
		}
	}
	
	[RPC]
	public void SetPlayerName(NetworkPlayer player, string name)
	{
		if(Network.isServer)
		{
			int playerIndex = GetPlayerIndex(player);
			_players[playerIndex].Nom = name;
			this.networkView.RPC("NotifyPlayerInfo", RPCMode.AllBuffered, playerIndex, _players[playerIndex].Nom, _players[playerIndex].Team);
		}
	}
	
	[RPC]
	void ChangeTeam(NetworkPlayer player, int team)
	{
		if(Network.isServer)
		{
			int playerIndex = GetPlayerIndex(player);
			_players[playerIndex].Team = team;
			this.networkView.RPC("NotifyPlayerInfo", RPCMode.AllBuffered, playerIndex, _players[playerIndex].Nom, _players[playerIndex].Team);
		}
	}
	
	[RPC]
	void NotifyPlayerInfo(int playerIndex, string playerName, int playerTeam)
	{
		_players[playerIndex].Nom = playerName;
		_players[playerIndex].Team = playerTeam;
		Debug.Log (_players[playerIndex].Nom + " \n Groupe " + _players[playerIndex].Team);
		string info = _players[playerIndex].Nom + " \n Groupe " + _players[playerIndex].Team;
		_playerInfo[playerIndex].SetText(info);
	}
	
	private int GetPlayerIndex(NetworkPlayer player)
	{
		for(int i=0; i<_players.Length; i++)
			if(player == _players[i].NetPlayer)
				return i;
		return -1;
	}
}

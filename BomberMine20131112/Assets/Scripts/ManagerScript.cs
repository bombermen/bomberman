using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerScript : Singleton<ManagerScript>
{

    protected ManagerScript() { }

    private string _adresseHote;
    private string _nomServeur;
    private string _nomJoueur;

    private bool _equipe;
    private bool _tirAmi;

    //numero du joueur
    private int playerIndex;
    private PlayerScript[] _players;
    [SerializeField]
    private TextAreaScript[] _playerInfo;
    private NetworkView _networkView;

    void Start()
    {
        _networkView = this.networkView;
        if (_players == null)
        {
            _players = new PlayerScript[4];
            for (int i = 0; i < 4; i++)
                _players[i] = new PlayerScript();
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public PlayerScript[] Players
    {
        get { return this._players; }
        set { _players = value; }
    }

    public int PlayerIndex
    {
        get { return playerIndex; }
        set { playerIndex = value; }
    }

    public bool TirAmi
    {
        get { return _tirAmi; }
        set { _tirAmi = value; }
    }

    public bool Equipe
    {
        get { return _equipe; }
        set { _equipe = value; }
    }

    public string AdresseHote
    {
        get { return this._adresseHote; }
        set { _adresseHote = value; }
    }

    public string NomJoueur
    {
        get { return this._nomJoueur; }
        set { _nomJoueur = value; }
    }

    public string NomServeur
    {
        get { return this._nomServeur; }
        set { _nomServeur = value; }
    }

    void OnServerInitialized(NetworkPlayer player)
    {
        this.networkView.RPC("NotifyNewPlayer", RPCMode.All, player);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        this.networkView.RPC("NotifyNewPlayer", RPCMode.All, player);
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        int playerIndex = GetPlayerIndex(player);
        _players[playerIndex].Connected = false;
    }

    [RPC]
    void NotifyNewPlayer(NetworkPlayer p)
    {
        //Initialisation du nouveau player
        PlayerScript player = new PlayerScript();
        player.NetPlayer = p;
        player.Initialized = true;
        player.Connected = true;

        //Enregistrement de ce player dans le premier player non initialisé du tableau de players
        for (int i = 0; i < _players.Length; i++)
        {
            //Le joueur est initialisé
            //on vérifie que le joueur a déjà été initialisé
            if (_players[i].Initialized)
            {
                if (_players[i].NetPlayer == p)
                {
                    _players[i].Connected = true;
                    player = _players[i];
                }
            }
            //le joueur n'est pas initialisé
            else
            {
                player.PlayerIndex = i;
                _players[i] = player;
                break;
            }
        }

        //Envoi des caractéristique du nouveau player aux autres joueurs
        for (int i = 1; i < player.PlayerIndex; i++)
        {
            if (_players[i].Initialized)
            {
                this.networkView.RPC("NotifyPlayerInfo", p, player.PlayerIndex, player.Nom, player.Team);
            }
        }

        //Envoi de l'indice du joueur qui vient d'être ajouté
        if (Network.player == p)
        {
            Debug.Log("c'est moi qui vient d'etre ajouté");
            if (Network.isServer)
            {
                SetPlayerIndex(playerIndex);
            }
            else
            {
                this.networkView.RPC("ChangeTeam", Network.player, Network.player, 1);
                this.networkView.RPC("SetPlayerIndex", p, playerIndex);
            }
        }
    }

    [RPC]
    private void SetPlayerIndex(int playerIndex)
    {
        Debug.Log("my player index : " + playerIndex);
        PlayerIndex = playerIndex;
    }

    public void SetTeam(int team)
    {
        if (Network.isServer)
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
        if (Network.isServer)
        {
            int playerIndex = GetPlayerIndex(player);
            _players[playerIndex].Nom = name;
            this.networkView.RPC("NotifyPlayerInfo", RPCMode.AllBuffered,
                                    playerIndex,
                                    _players[playerIndex].Nom,
                                    _players[playerIndex].Team);
        }
    }

    [RPC]
    void ChangeTeam(NetworkPlayer player, int team)
    {
        if (Network.isServer)
        {
            int playerIndex = GetPlayerIndex(player);
            _players[playerIndex].Team = team;
            this.networkView.RPC("NotifyPlayerInfo", RPCMode.All, playerIndex, _players[playerIndex].Nom, _players[playerIndex].Team);
        }
    }

    [RPC]
    void NotifyPlayerInfo(int playerIndex, string playerName, int playerTeam)
    {
        PlayerScript player = new PlayerScript();

        player.Nom = playerName;
        player.Team = playerTeam;
        player.PlayerIndex = playerIndex;
        player.Initialized = true;
        _players[playerIndex] = player;

        string info = player.Nom + "\nGroupe " + player.Team;

        _playerInfo[playerIndex].SetText(info);
    }

    public int GetPlayerIndex(NetworkPlayer player)
    {
        for (int i = 0; i < _players.Length; i++)
            if (player == _players[i].NetPlayer)
                return i;
        return -1;
    }

    public List<NetworkPlayer> GetPlayersByTeam(int team)
    {
        List<NetworkPlayer> result = new List<NetworkPlayer>();

        for (int i = 0; i < _players.Length; i++)
            if (_players[i].Team == team)
                result.Add(_players[i].NetPlayer);
        return result;
    }

    public PlayerScript GetPlayer(int playerIndex)
    {
        return _players[playerIndex];
    }
}

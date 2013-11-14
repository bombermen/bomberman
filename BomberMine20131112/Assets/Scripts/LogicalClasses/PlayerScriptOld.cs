using UnityEngine;
using System.Collections;

public class PlayerScriptOld
{

    private int _team;
    private NetworkPlayer _networkPlayer;
    private string _nom;
    private bool _initialized = false;
    private bool _connected = false;
    //Indique si le joueur possède un item (type foudroiement) bloquant le clic de la souris
    private bool _item;

    public bool Item
    {
        get { return _item; }
        set { _item = value; }
    }

    public bool Connected
    {
        get
        {
            return this._connected;
        }
        set
        {
            _connected = value;
        }
    }

    public bool Initialized
    {
        get
        {
            return this._initialized;
        }
        set
        {
            _initialized = value;
        }
    }

    public NetworkPlayer NetPlayer
    {
        get
        {
            return this._networkPlayer;
        }
        set
        {
            _networkPlayer = value;
        }
    }

    public string Nom
    {
        get
        {
            return this._nom;
        }
        set
        {
            _nom = value;
        }
    }

    public int Team
    {
        get
        {
            return this._team;
        }
        set
        {
            _team = value;
        }
    }
}

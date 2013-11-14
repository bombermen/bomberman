using UnityEngine;
using System.Collections;

public class NetworkScript : MonoBehaviour
{
    private ManagerScript _manager;

	private float _speed = 5.0f;
    private float _speedCourse = 5.0f;
	private float _mouseSensitivity;
	private float _rotLeftRight;

    [SerializeField]
    private GameObject[] _players;
    [SerializeField]
	private GameObject[] _bombs;
    [SerializeField]
	private GameObject[] _mines;
    [SerializeField]
    private MiniMapScript _miniMap;
    [SerializeField]
	private Transform[] _positionDepot;
    [SerializeField]
	private Transform[] _playerTransforms;
    [SerializeField]
	private Rigidbody[] _playerRigidbodies;
    [SerializeField]
	private BombScript[] _bombScripts;
    [SerializeField]
	private MineDeclenchementScript[] _mineScripts;
	private int[] _joueurDansCapsule = {-1, -1, -1};
	private int[] _minesDuJoueur = {0, 1, 2, 0};
	private int _joueurAyantLeBouclier = -1;
	private int _dernierJoueurAyantEuLeBouclier = -1;
    private int _monIndiceJoueur;

    [SerializeField]
    private Camera[] _cameras;

	[SerializeField]
	private GameObject[] _boucliersJoueurs;
	
	[SerializeField]
	private GameObject _bouclier;

	private NetworkPlayer[] _playerNetworks;
	
	//Intention de déplacement des joueurs (joueur1 > [0], joueur2 > [1], ...)
	private bool[] avance = {false, false, false, false};
	private bool[] recule = {false, false, false, false};
	private bool[] droite = {false, false, false, false};
	private bool[] gauche = {false, false, false, false};
    private bool[] courir = {false, false, false, false};	
	
	
	void Awake ()
	{
        _manager = ManagerScript.Instance;

		_mouseSensitivity = 80.0f;
		Application.runInBackground = true;
        Application.targetFrameRate = 60;

        /*
		_playerNetworks = new NetworkPlayer[4];
		_playerTransforms = new Transform[4];
		_playerRigidbodies = new Rigidbody[4];
		_bombScripts = new BombScript[4];
		_mineScripts = new MineDeclenchementScript[4];
        _manager.Players = new PlayerScript[4];

		for(int i=0; i<_players.Length; i++)
		{
			_playerRigidbodies[i] = _players[i].rigidbody;
			_playerTransforms[i] = _players[i].transform;
			_bombScripts[i] = _bombs[i].transform.GetChild(0).GetComponent<BombScript>();
			_mineScripts[i] = _mines[i].transform.GetChild(0).GetComponent<MineDeclenchementScript>();
		}
         */

        _players[_manager.PlayerIndex].SetActive(true);
	}

	void Update() 
	{
		if(Network.isServer)
		{
            float save = _rotLeftRight;
			
			_rotLeftRight = Input.GetAxis ("Mouse X") * _mouseSensitivity * Time.deltaTime;
			if(save != _rotLeftRight)
			{
				this.networkView.RPC("Souris", RPCMode.All, _manager.PlayerIndex, _rotLeftRight);
			}
			
			if(Input.GetKeyDown(KeyCode.Mouse0))
	        {
				Bombs(_manager.PlayerIndex);
			}
            
			if(Input.GetKeyDown(KeyCode.Mouse1))
	        {
				DepotOuRepriseMines(_manager.PlayerIndex);
			}
             
			if (Input.GetKeyDown(KeyCode.Z))
	        {
	            Avancer(_manager.PlayerIndex, true);
	        }
			else if (Input.GetKeyUp(KeyCode.Z))
	        {
	            Avancer(_manager.PlayerIndex, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.S))
	        {
	            Reculer(_manager.PlayerIndex, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.S))
	        {
	            Reculer(_manager.PlayerIndex, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.Q))
	        {
	            Gauche(_manager.PlayerIndex, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.Q))
	        {
	            Gauche(_manager.PlayerIndex, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.D))
	        {
	            Droite(_manager.PlayerIndex, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.D))
	        {
	            Droite(_manager.PlayerIndex, false);
			}
			if (Input.GetKeyDown(KeyCode.Space))
            {
                Courir(_manager.PlayerIndex, true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                Courir(_manager.PlayerIndex, false);
            }
		}
		else if (Network.isClient)
		{
			float save = _rotLeftRight;
	
			_rotLeftRight = Input.GetAxis ("Mouse X") * _mouseSensitivity * Time.deltaTime;
			if(save != _rotLeftRight)
			{
				this.networkView.RPC("Souris", RPCMode.All, _manager.PlayerIndex, _rotLeftRight);
			}
			
			if(Input.GetKeyDown(KeyCode.Mouse0))
	        {
				this.networkView.RPC("Bombs", RPCMode.Server, _manager.PlayerIndex);
			}
			
			if(Input.GetKeyDown(KeyCode.Mouse1))
			{
				this.networkView.RPC("DepotOuRepriseMines", RPCMode.Server, _manager.PlayerIndex);
			}
			
	        if (Input.GetKeyDown(KeyCode.Z))
	        {
	            this.networkView.RPC("Avancer", RPCMode.Server, _manager.PlayerIndex, true);
                Avancer(_manager.PlayerIndex, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.Z))
	        {
	            this.networkView.RPC("Avancer", RPCMode.Server, _manager.PlayerIndex, false);
                Avancer(_manager.PlayerIndex, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.S))
	        {
	            this.networkView.RPC("Reculer", RPCMode.Server, _manager.PlayerIndex, true);
                Reculer(_manager.PlayerIndex, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.S))
	        {
	            this.networkView.RPC("Reculer", RPCMode.Server, _manager.PlayerIndex, false);
                Reculer(_manager.PlayerIndex, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.Q))
	        {
	            this.networkView.RPC("Gauche", RPCMode.Server, _manager.PlayerIndex, true);
                Gauche(_manager.PlayerIndex, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.Q))
	        {
	            this.networkView.RPC("Gauche", RPCMode.Server, _manager.PlayerIndex, false);
                Gauche(_manager.PlayerIndex, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.D))
	        {
	            this.networkView.RPC("Droite", RPCMode.Server, _manager.PlayerIndex, true);
                Droite(_manager.PlayerIndex, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.D))
	        {
	            this.networkView.RPC("Droite", RPCMode.Server, _manager.PlayerIndex, false);
                Droite(_manager.PlayerIndex, false);
			}
			if (Input.GetKeyDown(KeyCode.Space))
            {
                this.networkView.RPC("Courir", RPCMode.Server, _manager.PlayerIndex, true);
                Courir(_manager.PlayerIndex, true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                this.networkView.RPC("Courir", RPCMode.Server, _manager.PlayerIndex, false);
                Courir(_manager.PlayerIndex, false);
            }
		}
	}
	
	[RPC]
    void Courir(int playerIndex, bool keyDown)
    {
        courir[playerIndex] = keyDown;
		
		if(keyDown)
		{
			_cameras[playerIndex].cullingMask &= ~(1 << 10);
		}
		else
		{
			_cameras[playerIndex].cullingMask |= 1 << 10;
		}
    }
	
	[RPC]
	void ConfirmerBombe(int playerIndex, Vector3 positionDepot)
	{
		_bombs[playerIndex].transform.position = positionDepot;
		_bombs[playerIndex].SetActive(true);
		_bombScripts[playerIndex].poser = false;
	}
	
	[RPC]
    void Bombs(int playerIndex)
    {
		if(_bombScripts[playerIndex].poser && !_manager.Players[playerIndex].Item)
		{
			Vector3 positionDepot = _positionDepot[playerIndex].position;
			this.networkView.RPC("ConfirmerBombe", RPCMode.All, playerIndex, positionDepot);
		}
	}

    [RPC]
    void ConfirmerMines(int playerIndex, int mine)
    {
        _mines[mine].transform.position = _positionDepot[playerIndex].position;
        _mines[mine].SetActive(true);
    }

    [RPC]
    void ConfirmerMinesReprise(int playerIndex, int mine)
    {
        _mineScripts[mine]._minePoser = false;
        _mineScripts[mine]._mineRecuperable = false;
        _mines[mine].SetActive(false);
        _mineScripts[mine]._leJoueur = null;
    }

    [RPC]
    void DepotOuRepriseMines(int playerIndex)
    {
        int _nb = JoueurSurUneMines(playerIndex);

        if (_nb > -1)
        {
            this.networkView.RPC("ConfirmerMinesReprise", RPCMode.All, playerIndex, _nb);
            RepriseMines(playerIndex, _nb);
        }
        else
        {
            int _jaiUneMine = PoserUneMines(playerIndex);
            if (_jaiUneMine > -1)
                this.networkView.RPC("ConfirmerMines", RPCMode.All, playerIndex, _jaiUneMine);
        }
    }

    public void ImpacterJoueur(int playerIndex)
    {
        if (_joueurAyantLeBouclier == playerIndex)
        {
            this.networkView.RPC("RendreBouclierDisponible", RPCMode.AllBuffered);
        }
        else
        {
            this.networkView.RPC("TuerJoueur", RPCMode.AllBuffered, playerIndex);
        }
    }

    public void ImpacterJoueur(Transform joueur)
    {
        int playerIndex = GetPlayerIndexByTransform(joueur);
        if(playerIndex >= 0)
        {
            ImpacterJoueur(playerIndex);
        }
    }

    [RPC]
    public void RecupererBouclier(Transform joueurTransform)
    {
        int playerIndex = GetPlayerIndexByTransform(joueurTransform);
        Debug.Log("RecupererBouclier : " + playerIndex);
        if (playerIndex != _dernierJoueurAyantEuLeBouclier)
        {
            this.networkView.RPC("BouclierRecupere", RPCMode.All, playerIndex);
        }
    }

    [RPC]
    void BouclierRecupere(int playerIndex)
    {
        //Debug.Log("BouclierRecupere : " + playerIndex);
        _boucliersJoueurs[playerIndex].SetActive(true);
        _joueurAyantLeBouclier = playerIndex;
        _bouclier.SetActive(false);
    }

    [RPC]
    void RendreBouclierDisponible()
    {
        if (_joueurAyantLeBouclier != -1)
        {
            _boucliersJoueurs[_joueurAyantLeBouclier].SetActive(false);
            _bouclier.transform.position = _playerTransforms[_joueurAyantLeBouclier].position;
            _dernierJoueurAyantEuLeBouclier = _joueurAyantLeBouclier;
            _joueurAyantLeBouclier = -1;
            _bouclier.SetActive(true);
        }
    }

    [RPC]
    public void DemanderFoudroiement(Transform playerTransform)
    {
        int playerIndex = GetPlayerIndexByTransform(playerTransform);
        if(playerIndex == 0)
        {
            ConfirmerFoudroiement(true, playerIndex);
        }
        else
        {
            Debug.Log("salut");
            this.networkView.RPC("ConfirmerFoudroiement", _manager.Players[playerIndex].NetPlayer, true, playerIndex);
        }
        StartCoroutine(AttendreTempsFoudroiement(10.0f, playerIndex, _manager.Players[playerIndex].NetPlayer));
    }

    [RPC]
    void ConfirmerFoudroiement(bool activate, int playerIndex)
    {
        _miniMap.FoudroiementActif = activate;
        _manager.Players[playerIndex].Item = activate;
    }

    private IEnumerator AttendreTempsFoudroiement(float seconds, int playerIndex, NetworkPlayer player)
    {
        yield return new WaitForSeconds(seconds);
        if (playerIndex == 0)
        {
            ConfirmerFoudroiement(false, playerIndex);
        }
        else
        {
            this.networkView.RPC("ConfirmerFoudroiement", player, false, playerIndex);
        }
    }

    [RPC]
    void TuerJoueur(int playerIndex)
    {
        _cameras[playerIndex].gameObject.SetActive(false);
        _players[playerIndex].SetActive(false);
    }

	
	[RPC]
    void Souris(int playerIndex, float _rotLeftRight)
    {
		_playerTransforms[playerIndex].Rotate(0,_rotLeftRight,0);
    }

    [RPC]
    void Avancer(int playerIndex, bool keyDown)
    {
        avance[playerIndex] = keyDown;
    }
	
    [RPC]
    void Reculer(int playerIndex, bool keyDown)
    {
        recule[playerIndex] = keyDown;
    }
	
	[RPC]
    void Gauche(int playerIndex, bool keyDown)
    {
        gauche[playerIndex] = keyDown;
    }
	
	[RPC]
    void Droite(int playerIndex, bool keyDown)
    {
		droite[playerIndex] = keyDown;
    }
	
	void FixedUpdate()
	{

        if (Network.isServer)
        {
            Vector3[] velocities = new Vector3[_players.Length];

            for (int i = 0; i < Network.connections.Length + 1; i++)
            {
                Rigidbody rigidbodyPlayer = _playerRigidbodies[i];
                Transform transformPlayer = _playerTransforms[i];

                if (rigidbodyPlayer != null && transformPlayer != null)
                {
                    //Vitesse du point de vue du perso
                    Vector3 localSpeed = Vector3.zero;
                    //Vitesse globale (incluant la rotation du perso)
                    Vector3 speed = Vector3.zero;

                    if (avance[i])
                    {
                        localSpeed.z += 1;
                    }
                    if (recule[i])
                    {
                        localSpeed.z -= 1;
                    }
                    if (droite[i])
                    {
                        localSpeed.x += 1;
                    }
                    if (gauche[i])
                    {
                        localSpeed.x -= 1;
                    }

                    //Normalisation du vecteur (pour éviter le problème du straff)
                    if (courir[i])
                        localSpeed = localSpeed.normalized * (_speed + _speedCourse);
                    else
                        localSpeed = localSpeed.normalized * _speed;

                    //Retour aux coordonées globales
                    speed = transformPlayer.TransformDirection(localSpeed);
                    //Déplacement du personnage
                    rigidbodyPlayer.velocity = speed;

                    this.networkView.RPC("UpdateVelocity", RPCMode.Others, speed, i);
                }
            }
        }
        else
        {
            Rigidbody rigidbodyPlayer = _playerRigidbodies[_monIndiceJoueur];
            Transform transformPlayer = _playerTransforms[_monIndiceJoueur];

            //Vitesse du point de vue du perso
            Vector3 localSpeed = Vector3.zero;
            //Vitesse globale (incluant la rotation du perso)
            Vector3 speed = Vector3.zero;

            if (avance[_monIndiceJoueur])
            {
                localSpeed.z += 1;
            }
            if (recule[_monIndiceJoueur])
            {
                localSpeed.z -= 1;
            }
            if (droite[_monIndiceJoueur])
            {
                localSpeed.x += 1;
            }
            if (gauche[_monIndiceJoueur])
            {
                localSpeed.x -= 1;
            }

            //Normalisation du vecteur (pour éviter le problème du straff)
            if (courir[_monIndiceJoueur])
                localSpeed = localSpeed.normalized * (_speed + _speedCourse);
            else
                localSpeed = localSpeed.normalized * _speed;

            //Retour aux coordonées globales
            speed = transformPlayer.TransformDirection(localSpeed);
            //Déplacement du personnage
            rigidbodyPlayer.velocity = speed;
        }
	}

    [RPC]
    void UpdateVelocity(Vector3 velocity, int player)
    {
        _playerRigidbodies[player].velocity = velocity;
    }

	private int GetPlayerIndex(NetworkPlayer player)
	{
		for(int i=0; i<_players.Length; i++)
			if(player == _playerNetworks[i])
				return i;
		return -1;
	}
	
	private int GetPlayerIndexByTransform(Transform joueurTransform)
	{
		for(int i=0; i<_playerTransforms.Length; i++)
		{
			if(joueurTransform == _playerTransforms[i])
			{
				return i;
			}
		}
		return -1;
	}
	
	public int PoserUneMines(int playerIndex)
	{
		for(int i =0; i<4; i++)
		{
			if(_minesDuJoueur[i] == playerIndex)
			{
                _minesDuJoueur[i] = -1;
				return i;
			}
		}
		return -1;
	}
	
	public void RepriseMines(int playerIndex , int mine)
	{
		_minesDuJoueur[mine] = playerIndex;
	}
	
	public void EntrerDansCapsule(Transform playerTransform, int capsuleIndex)
	{
		int playerIndex = GetPlayerIndexByTransform(playerTransform);
		if(Network.isServer)
		{
			if(CapsuleLibre(capsuleIndex))
			{
				_joueurDansCapsule[capsuleIndex] = playerIndex;
			}
		}
	}
	
	public void ReinitialiserCapsules()
	{
		CapsuleScript._nbCapsules--;
		
		for(int i=0; i<CapsuleScript._nbCapsules; i++)
		{
			_joueurDansCapsule[i] = -1;
		}
	}
	
	private bool CapsuleLibre(int capsuleIndex)
	{
		return _joueurDansCapsule[capsuleIndex] < 0;
	}
	
	public void ExploserBombeH()
	{
		Debug.Log("J'explose");
		if(Network.isServer)
		{
			for(int i=0; i<4; i++)
			{
				if(!JoueurSauf(i))
				{
					ImpacterJoueur(i);
				}
			}
		}
	}
	
	private bool JoueurSauf(int playerIndex)
	{
		for(int i=0; i<CapsuleScript._nbCapsules; i++)
		{
			if(_joueurDansCapsule[i] == playerIndex)
				return true;
		}
		
		return false;
	}
	
	public void JoueurMort(Transform playerTransform)
	{
		if(Network.isServer)
		{
            int playerIndex = GetPlayerIndexByTransform(playerTransform);
            ImpacterJoueur(playerIndex);
		}
	}

    private int JoueurSurUneMines(int playerIndex)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_mineScripts[i]._leJoueur == _players[playerIndex])
                return i;
        }
        return -1;
    }
	
}
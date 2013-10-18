using UnityEngine;
using System.Collections;

public class NetworkScript : MonoBehaviour
{
	private Rigidbody _rigidbody;
	private Vector3 wantedMovement;
	public float _speed = 5.0f;
	private float _mouseSensitivity;
	private float _upDownRange;
	private float _verticalRotation;
	private float _rotLeftRight;
	private float _rotLeftRightJ1;
	private float _rotLeftRightJ2;
	
	public GameObject[] _players;
	private Transform[] _playerTransforms;
	private Rigidbody[] _playerRigidbodies;
	private Camera[] _cameras;
	
	private bool _player1initialized = false;
	private bool _player2initialized = false;
	
	private NetworkPlayer _player1;
	private NetworkPlayer _player2;
	
	//Intention de déplacement des joueurs (joueur1 > [0], joueur2 > [1], ...)
	private bool[] avance = {false, false, false, false};
	private bool[] recule = {false, false, false, false};
	private bool[] droite = {false, false, false, false};
	private bool[] gauche = {false, false, false, false};
	
	void Start ()
	{
		_verticalRotation = 0;
		
		_rotLeftRightJ1 = 0;
		_rotLeftRightJ2 = 0;
		
		_mouseSensitivity = 80.0f;
		_upDownRange = 60.0f;
		wantedMovement = Vector3.zero;
		Application.runInBackground = true;
		
		_playerTransforms = new Transform[4];
		_playerRigidbodies = new Rigidbody[4];
		_cameras = new Camera[4];
		
		for(int i=0; i<_players.Length; i++)
		{
			_playerRigidbodies[i] = _players[i].rigidbody;
			_playerTransforms[i] = _players[i].transform;
			_cameras[i] = _playerTransforms[i].GetChild(0).camera;
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

	void Update() 
	{
		if(Network.isServer)
		{
			float save = _rotLeftRight;
			
			_rotLeftRight = Input.GetAxis ("Mouse X") * _mouseSensitivity * Time.deltaTime;
			if(save != _rotLeftRight)
			{
				this.networkView.RPC("Souris", RPCMode.All, Network.player, _rotLeftRight);
			}
			
			if (Input.GetKeyDown(KeyCode.Z))
	        {
	            Avancer(Network.player, true);
	        }
			else if (Input.GetKeyUp(KeyCode.Z))
	        {
	            Avancer(Network.player, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.S))
	        {
	            Reculer(Network.player, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.S))
	        {
	            Reculer(Network.player, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.Q))
	        {
	            Gauche(Network.player, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.Q))
	        {
	            Gauche(Network.player, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.D))
	        {
	            Droite(Network.player, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.D))
	        {
	            Droite(Network.player, false);
			}
		}
		else if (Network.isClient)
		{
			float save = _rotLeftRight;
	
			_rotLeftRight = Input.GetAxis ("Mouse X") * _mouseSensitivity * Time.deltaTime;
			if(save != _rotLeftRight)
			{
				this.networkView.RPC("Souris", RPCMode.All, Network.player, _rotLeftRight);
			}
	
	        if (Input.GetKeyDown(KeyCode.Z))
	        {
	            this.networkView.RPC("Avancer", RPCMode.Server, Network.player, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.Z))
	        {
	            this.networkView.RPC("Avancer", RPCMode.Server, Network.player, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.S))
	        {
	            this.networkView.RPC("Reculer", RPCMode.Server, Network.player, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.S))
	        {
	            this.networkView.RPC("Reculer", RPCMode.Server, Network.player, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.Q))
	        {
	            this.networkView.RPC("Gauche", RPCMode.Server, Network.player, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.Q))
	        {
	            this.networkView.RPC("Gauche", RPCMode.Server, Network.player, false);
	        }
			
	        if (Input.GetKeyDown(KeyCode.D))
	        {
	            this.networkView.RPC("Droite", RPCMode.Server, Network.player, true);
	        }
	        else if (Input.GetKeyUp(KeyCode.D))
	        {
	            this.networkView.RPC("Droite", RPCMode.Server, Network.player, false);
			}
		}
	}
	
	[RPC]
    void Souris(NetworkPlayer player, float _rotLeftRight)
    {
	    if (player == _player1)
		{
			_playerTransforms[0].Rotate(0,_rotLeftRight,0);
		}
	    if (player == _player2)
		{
			_playerTransforms[1].Rotate(0,_rotLeftRight,0);
		}
    }
	
    [RPC]
    void Avancer(NetworkPlayer player, bool keyDown)
    {
		if (player == _player1)
		{
	        avance[0] = keyDown;
		}
		else if (player == _player2)
		{
	        avance[1] = keyDown;
		}
    }
	
    [RPC]
    void Reculer(NetworkPlayer player, bool keyDown)
    {
	    if (player == _player1)
		{
	        recule[0] = keyDown;
		}
		else if (player == _player2)
		{
	        recule[1] = keyDown;
		}
    }
	
	[RPC]
    void Gauche(NetworkPlayer player, bool keyDown)
    {
		if (player == _player1)
		{
	        gauche[0] = keyDown;
		}
		else if (player == _player2)
		{
	        gauche[1] = keyDown;
		} 
    }
	
	[RPC]
    void Droite(NetworkPlayer player, bool keyDown)
    {
	 	if (player == _player1)
		{
	        droite[0] = keyDown;
		}
		else if (player == _player2)
		{
	        droite[1] = keyDown;
		}
    }

	
	[RPC]
	void NotifyNewPlayer(NetworkPlayer p)
	{
		if (!_player1initialized)
		{
			_player1 = p;
			_player1initialized = true;
		}
		else
		{
			_player2 = p;
			_player2initialized = true;
		}
		if (_player1 == Network.player)
		{
			_cameras[1].enabled = false;
		}
		else
		{
			_cameras[0].enabled = false;
		}
	}
	
	void FixedUpdate() {
		
		if(Network.isServer)
		{
			Vector3[] velocities = new Vector3[_players.Length];
			
			for(int i=0; i<_players.Length; i++)
			{
				Rigidbody rigidbodyPlayer = _playerRigidbodies[i];
				Transform transformPlayer = _playerTransforms[i];
				
				if(rigidbodyPlayer != null && transformPlayer != null)
				{
					//Vitesse du point de vue du perso
					Vector3 localSpeed = Vector3.zero;
					//Vitesse globale (incluant la rotation du perso)
					Vector3 speed = Vector3.zero;
					
					if(avance[i])
					{
						localSpeed.z += 1;
					}
					if(recule[i])
					{
						localSpeed.z -= 1;
					}
					if(droite[i])
					{
						localSpeed.x += 1;
					}
					if(gauche[i])
					{
						localSpeed.x -= 1;
					}
					
					//Normalisation du vecteur (pour éviter le problème du straff)
					localSpeed = localSpeed.normalized * _speed;
					//Retour aux coordonées globales
					speed = transformPlayer.TransformDirection(localSpeed);
					//Déplacement du personnage
					rigidbodyPlayer.velocity = speed;
									
					velocities[i] = speed;
					
					this.networkView.RPC("UpdateVelocity", RPCMode.Others, speed);
				}
			}
			
		}
	}
	[RPC]
	void UpdateVelocity(Vector3 velocity, int player)
	{
		_playerRigidbodies[player].velocity = velocity;
	}
}

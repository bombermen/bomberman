using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatScript : MonoBehaviour
{

    [SerializeField]
    private ManagerScript _manager;
    [SerializeField]
    private TextAreaScript _textInput;
    [SerializeField]
    private TextAreaScript _textAreaMessages;

    private int MAX_MESSAGES = 9;
    private bool _chatActif = true;
    private List<MessageScript> _AllMessages;
    private List<MessageScript> _messages;

    void Start()
    {
        _messages = new List<MessageScript>();
        _textAreaMessages.SetText("test\ntest\ntest et encore un enorme test sur cette ligne\netsurla suivante");
    }

    void Update()
    {
        if (_chatActif)
        {
            if (Input.GetKeyDown(KeyCode.Return) && _textInput.GetText() != "")
            {
                if (Network.isClient)
                {
                    this.networkView.RPC("EnvoyerMessage", RPCMode.Server, _manager.PlayerIndex, _textInput.GetText());
                }
                else
                {
                    EnvoyerMessage(_manager.PlayerIndex, _textInput.GetText());
                }
                _textInput.SetText("");
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeState(false);
            }
        }
        else if(Input.GetKeyDown(KeyCode.T))
        {
            ChangeState(true);
        }
    }

    void ChangeState(bool activate)
    {
        _chatActif = activate;
    }

    string MessagesToText()
    {
        string resultat = "";

        foreach (MessageScript message in _messages)
        {
            resultat += "<0000AA>";
            resultat += message.DatePublication.ToString("'['HH':'mm']'");
            resultat += "<00AA00>";
            resultat += message.Player.Nom + ":";
            resultat += "<FFFFFF>";
            resultat += message.Text + "\n";
        }

        return resultat;
    }

    [RPC]
    void EnvoyerMessage(int playerIndex, string message)
    {
        if (Network.isServer)
        {
            MessageScript msg = new MessageScript();

            //Recuperation de commande /
            string[] mots = message.Split(new char[] { ' ' });

            //int playerIndex = _manager.GetPlayerIndex(player);
            PlayerScript joueur = _manager.Players[playerIndex];

            //groupe
            if (mots[0] == "/g")
            {
                int team = joueur.Team;
                string corpsDuMessage = message.Substring(mots[0].Length + 1);

                List<NetworkPlayer> players = _manager.GetPlayersByTeam(team);
                foreach (NetworkPlayer p in players)
                {
                    this.networkView.RPC("AjouterMessage", p, corpsDuMessage, team, playerIndex);
                }
            }
            else
            {
                msg.SetMessageToAll(message, joueur);
                this.networkView.RPC("AjouterMessage", RPCMode.All, msg.Text, msg.Group, playerIndex);
            }
        }
    }

    [RPC]
    void AjouterMessage(string message, int groupe, int playerIndex)
    {
        MessageScript msg = new MessageScript();
        PlayerScript player = _manager.GetPlayer(playerIndex);
        msg.SetMessageToGroup(groupe, message, player);
        if (_messages.Count >= MAX_MESSAGES)
        {
            _messages.RemoveAt(0);
        }
        _messages.Add(msg);
        _textAreaMessages.SetText(MessagesToText());
        _textInput.SetText("");
    }
}

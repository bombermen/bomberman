using UnityEngine;
using System.Collections;
using System;

public class MessageScript
{

    private int _group;
    private string _text;
    private PlayerScript _player;
    private DateTime _datePublication;

    private const int TO_ALL = -1;

    public void SetMessageToGroup(int groupe, string msg, PlayerScript player)
    {
        _group = groupe;
        _text = msg;
        _datePublication = DateTime.Now;
        _player = player;
    }

    public void SetMessageToAll(string msg, PlayerScript player)
    {
        _group = TO_ALL;
        _text = msg;
        _datePublication = DateTime.Now;
        _player = player;
    }

    public bool LisibleParEquipe(int groupe)
    {
        return (_group == TO_ALL) || (_group == groupe);
    }

    public PlayerScript Player
    {
        get { return _player; }
        set { _player = value; }
    }

    public int Group
    {
        get { return this._group; }
        set { _group = value; }
    }

    public DateTime DatePublication
    {
        get { return _datePublication; }
        set { _datePublication = value; }
    }

    public string Text
    {
        get { return this._text; }
        set { _text = value; }
    }
}

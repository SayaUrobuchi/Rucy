using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandMaid : MonoBehaviour
{
    public enum State
    {
        None, 
        Summon, 
        Cast, 
        Attack, 
        TurnEnd, 
        Summoning, 
        Casting, 
        Attacking, 
        Cancel, 
    }

    [System.Serializable]
    public struct Item
    {
        public State Type;
        public string CommandName;
        public Color Color;
        public bool Clickable;
    }

    public List<Item> Settings = new List<Item>();

    [Header("reference")]
    public Text CommandText;

    private Image image;
    private Button button;
    private Dictionary<State, Item> table = new Dictionary<State, Item>();
    private State cmd;

    private void Init()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        if (table.Count != Settings.Count)
        {
            for (int i = 0; i < Settings.Count; i++)
            {
                table[Settings[i].Type] = Settings[i];
            }
        }
    }
    
    public void SetCommand(State command, bool clickable = false)
    {
        Init();
        cmd = command;
        if (cmd == State.None)
        {
            this.SetVisible(false);
        }
        else
        {
            this.SetVisible(true);
            Item item = table[cmd];
            image.color = item.Color;
            CommandText.text = item.CommandName;
            button.interactable = (item.Clickable && clickable);
        }
    }

    public void OnClick()
    {
        BattleMaid.Summon.CommandExecute(null, cmd);
    }
}

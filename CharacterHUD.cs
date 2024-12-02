using UnityEngine;

[RequireComponent(typeof(CharacterSystem))]
public class CharacterHUD : MonoBehaviour
{
    [HideInInspector]
    public CharacterSystem character;

    private CharacterLiving living;

    public Texture2D Icon_HP;
    public Texture2D Icon_Armor;
    public Texture2D Icon_Food;
    public Texture2D Icon_Water;
    public Texture2D Bg;
    public Texture2D Bar_Food;
    public Texture2D Bar_Water;
    public Texture2D Bar_Health;
    public GUISkin Skin;

    private void Start()
    {
        character = GetComponent<CharacterSystem>();
        living = GetComponent<CharacterLiving>();
        StyleManager styleManager = FindObjectOfType<StyleManager>();
        if (styleManager && !Skin)
        {
            Skin = styleManager.GetSkin(0);
        }
    }

    private void DrwaIcon(int pos, Texture2D icon, string text, float percent, Texture2D bar)
    {
        if (icon)
        {
            GUI.DrawTexture(new Rect(20 + pos * 180 + 20, Screen.height - 65, 50f, 50f), icon);
        }
        if (Bg && bar)
        {
            GUI.DrawTexture(new Rect(80 + pos * 180 + 20, Screen.height - 55, 100f, 30f), Bg);
            GUI.DrawTexture(new Rect(80 + pos * 180 + 20, Screen.height - 55, 100f * percent, 30f), bar);
        }
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.fontSize = 22;
        GUI.Label(new Rect(80 + pos * 180 + 30, Screen.height - 55, 200f, 30f), text);
    }

    private void OnGUI()
    {
        if (Skin)
        {
            GUI.skin = Skin;
        }

        DrwaIcon(0, Icon_HP, character.HP + " %", (float)character.HP / (float)character.HPmax, Bar_Health);
        if (living)
        {
            DrwaIcon(1, Icon_Food, living.Hungry + " %", (float)living.Hungry / (float)living.HungryMax, Bar_Food);
            DrwaIcon(2, Icon_Water, living.Water + " %", (float)living.Water / (float)living.WaterMax, Bar_Water);
        }
    }
}
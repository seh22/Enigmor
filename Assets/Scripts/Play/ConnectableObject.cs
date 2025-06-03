using UnityEngine;

public enum ObjectType
{
    NPC,
    Weapon,  // 기존 Tool에서 변경
    Place
}

public enum ColorType
{
    None = -1,
    Red = 0,
    Green = 1,
    Blue = 2
}

public class ConnectableObject : MonoBehaviour
{
    public ObjectType objectType;
    public ColorType colorType = ColorType.None;

    public void UpdateVisual()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        switch (colorType)
        {
            case ColorType.Red:
                sr.color = Color.red;
                break;
            case ColorType.Green:
                sr.color = Color.green;
                break;
            case ColorType.Blue:
                sr.color = Color.blue;
                break;
            default:
                sr.color = Color.white;
                break;
        }
    }
}

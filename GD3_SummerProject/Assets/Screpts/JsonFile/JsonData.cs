
[System.Serializable]
public class JsonData
{
    public WeaponList[] weaponList;
}

[System.Serializable]
public class WeaponList
{
    // Nuber
    public int number;
    // Tags
    public string name;
    public string typeName;
    public int typeNum;
    public string trail;
    public string icon;
    // Status
    public int damage;
    public int defknockback;
    public int maxknockback;
    public int stanpower;
    // Sprites
    public int wideth;
    public int height;
    public float offset;
    // Description
    public string text;
}

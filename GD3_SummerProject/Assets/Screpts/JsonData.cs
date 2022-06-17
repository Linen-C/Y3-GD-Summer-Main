
[System.Serializable]
public class JsonData
{
    public WeaponList[] weaponList;
}

[System.Serializable]
public class WeaponList
{
    //public Tags tags;
    public string name;
    public string image;
    //public Status status;
    public int damage;
    public int defknockback;
    public int maxknockback;
    public int maxcharge;
    //public Sprites sprites;
    public int wideth;
    public int height;
    public float offset;
}


public abstract class Role
{
    protected string color;
    protected int index;
    protected string name;
    protected int bonusHealth = 0;

    public int BonusHealth {
        get {
            return this.bonusHealth;
        }
    }
    public string Name {
        get {
            return this.name;
        }
	}
	public int Index
	{
		get
		{
			return this.index;
		}
	}
	public string Color
	{
		get
		{
			return this.color;
		}
	}
}

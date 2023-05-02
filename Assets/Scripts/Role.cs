
public abstract class Role
{
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
}

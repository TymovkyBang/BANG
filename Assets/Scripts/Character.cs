
public abstract class Character
{
    protected string[] name = {"Forename", "Surname"};
    protected int health, index;

    public string[] Name {
        get {
            return this.name;
        }
    }
    
    public int Health {
        get {
            return this.health;
        }
    }

    
    protected void setName(string forename, string surname){
        this.name[0] = forename;
        this.name[1] = surname;
    }

    public int Index { get { return this.index; } }
}

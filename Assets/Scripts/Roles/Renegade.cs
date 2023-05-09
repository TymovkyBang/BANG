
public class Renegade : Role {
    public Renegade() {
        this.name = "Renegade"; // Odpadlík
		this.index = 1;

		GameManager.rolesList.Add(this);
	}
}
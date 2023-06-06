
public class Outlaw : Role {

    public Outlaw() {
        this.name = "Outlaw"; // Bandita
		this.color = "red";
		this.index = 2;

		GameManager.rolesList.Add(this);
	}
}
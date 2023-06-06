
public class Deputy : Role {

    public Deputy() {
        this.name = "Deputy"; // Pomocník šerifa
		this.index = 3;
		this.color = "blue";

		GameManager.rolesList.Add(this);
	}
}
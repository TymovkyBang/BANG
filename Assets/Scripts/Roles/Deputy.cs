
public class Deputy : Role {

    public Deputy() {
        this.name = "Deputy"; // Pomocník šerifa
		this.index = 3;

		GameManager.rolesList.Add(this);
	}
}
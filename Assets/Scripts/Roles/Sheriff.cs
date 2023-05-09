
public class Sheriff : Role {

    public Sheriff() {
        this.name = "Sheriff"; // Šerif XD
        this.bonusHealth = 1;
        this.index = 0;

		GameManager.rolesList.Add(this);
	}
}
using UnityEngine;
public class GeneralStore : Card
{
    protected override void init()
    {
        this.numberOfCards = 2;
    }

    public override bool use(){
        GameManager.localNetwork.generalStoreServerRpc(GameManager.localNetwork.ID);
		this.numberOfCardsCurrently--;
		return true;
	}
}

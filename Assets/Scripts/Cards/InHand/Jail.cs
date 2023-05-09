using Unity.VisualScripting;
using UnityEngine;
public class Jail : Placeable, Targetable
{
    protected override void init()
    {
		this.numberOfCards = 3; // 3
    }

    public override bool use()
	{
		if ((int)GameManager.selectedPlayer.ID == GameManager.localNetwork.sheriffID) return false;
		target(GameManager.selectedPlayer.ID);
		return true;

	}
	public void target(ulong targetID)
	{
		place();
	}
	public override void place()
	{
		GameManager.localNetwork.jailServerRpc(GameManager.localNetwork.ID, GameManager.selectedPlayer.ID);
		numberOfCardsCurrently--;
	}
}

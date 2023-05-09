using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField] int index;

	private void Start()
	{
		GameManager.chairList.Add(this);
		this.Index = this.index;
	}
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = value;
			gameObject.name = "Chair " + this.index.ToString();
		}
	}


}

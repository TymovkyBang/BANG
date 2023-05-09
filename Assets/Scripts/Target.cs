using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Target : MonoBehaviour
{
    private new Renderer renderer;
    public Color defaultColor;

    [SerializeField] private GameObject playerNetwork;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.selectedPlayer == playerNetwork.GetComponent<PlayerNetwork>()) renderer.material.color = Color.red;
		else renderer.material.color = defaultColor;
	}

	private void OnMouseDown()
	{
        if (Input.mousePosition.y > 350) {
            if (GameManager.selectedPlayer != playerNetwork.GetComponent<PlayerNetwork>()) GameManager.selectedPlayer = playerNetwork.GetComponent<PlayerNetwork>();
            else GameManager.selectedPlayer = null;
        }
	}
}

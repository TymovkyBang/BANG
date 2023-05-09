using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class LogManager {

	TextMeshProUGUI textField;
	bool deleting = false;
	string defaultColor = "black";

	public void setTextField(TextMeshProUGUI textField)
	{
		this.textField = textField;
		defaultColor = "#" + textField.color.ToHexString();
	}
	public void log(string message, string color = "default")
	{
		if (color == "default") color = defaultColor; 

		if (color.Contains('#')) textField.text += $"\n<color={color}>" + message;
		else textField.text += $"\n<color=\"{color}\">" + message;
		if ( !deleting ) { delete(GameManager.cooldownTime + 2); }
	}

	public async void delete(int time)
	{
		deleting = true;

		await Task.Delay(time * 1000);
		string[] list = textField.text.Split("\n");
		list[0] = "";

		string newText = "";

		for (int i = 0; i < list.Length; i++)
		{
			newText += "\n" + list[i];
		}

		textField.text = newText;

		if (textField.text.Length > 0 ) { delete(time); }
		else deleting = false;
	}
}

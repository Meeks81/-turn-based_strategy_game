using System.Collections.Generic;
using UnityEngine;

public enum TeamColor
{
	none, blue, green, yellow, orange, red, cyan, indigo, teal
}

public class Team
{

	public readonly TeamColor Color;
	public Wallet Wallet;

	public Team(TeamColor color, int startCoins)
	{
		if (color == TeamColor.none)
			throw new System.Exception("Team color can't be NONE");

		Color = color;
		Wallet = new Wallet(startCoins);
	}

	public int CalculateCoinsPerMove(List<Cell> map)
	{
		int value = 0;

		foreach (var item in map)
		{
			if (item.Color != Color)
				continue;

			if (item.PlacedObject == null)
			{
				value++;
			}
			else
			{
				value += item.PlacedObject.coinsPerMove;
			}
		}

		return value;
	}

	public Color GetColor() => GetColor(Color);
	public static Color GetColor(TeamColor color)
	{
		switch (color)
		{
			case TeamColor.none:
				return new Color(0.4f, 0.45f, 0.4f);
			case TeamColor.blue:
				return new Color(0f, 0f, 1f);
			case TeamColor.green:
				return new Color(0f, 1f, 0f);
			case TeamColor.yellow:
				return new Color(1f, 1f, 0f);
			case TeamColor.orange:
				return new Color(1f, 0.5f, 0f);
			case TeamColor.red:
				return new Color(1f, 0f, 0f);
			case TeamColor.cyan:
				return new Color(0f, 1f, 1f);
			case TeamColor.indigo:
				return new Color(0.3f, 0f, 0.5f);
			case TeamColor.teal:
				return new Color(0f, 0.5f, 0.5f);
			default:
				return new Color(0.2f, 0.2f, 0.2f);
		}
	}

}

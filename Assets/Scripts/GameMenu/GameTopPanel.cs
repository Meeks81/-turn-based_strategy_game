using TMPro;
using UnityEngine;

public class GameTopPanel : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _playerCoinsText;
    [SerializeField] private TextMeshProUGUI _playerMovesCountText;
	[SerializeField] private TextMeshProUGUI _allStatistic;
	[Space]
	[SerializeField] private Level _level;

	private void Update()
	{
		_playerCoinsText.text = $"<sprite name=coin> {_level.player.ControlTeam.Wallet.Coins}";
		UpdateAllStatistic();
	}

	private void UpdateAllStatistic()
	{
		_allStatistic.text = "";
		foreach (var item in _level.map.Teams)
		{
			_allStatistic.text += $"{item.Color}: <sprite name=coin> {item.Wallet.Coins}\n";
		}
	}

}

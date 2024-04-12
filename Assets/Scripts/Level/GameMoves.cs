using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMoves : MonoBehaviour
{

	[SerializeField] private Level _level;

	public int CurrentMoveCount { get; private set; }
	public Team CurrentTeamMove => _teamMovesQueue[_currentTeamMoveIndex];
	public Team[] TeamMovesQueue => _teamMovesQueue;

	public Level level => _level;

	public UnityAction OnFinishMove;

	private Team[] _teamMovesQueue;
	private int _currentTeamMoveIndex = 0;

	public void Initialize(Team[] teams)
	{
		UpdateTeamMovesQueue(teams);
		FinishMove();
	}

	public void FinishMove()
	{
		if (_teamMovesQueue == null || _teamMovesQueue.Length == 0)
			throw new System.Exception("Teams queue is not available");

		_currentTeamMoveIndex++;
		if (_currentTeamMoveIndex >= _teamMovesQueue.Length)
		{
			CurrentMoveCount++;
			_currentTeamMoveIndex = 0;
		}

		OnFinishMove?.Invoke();

		int coinsPerMove = CurrentTeamMove.CalculateCoinsPerMove(_level.GetAllCells());
		CurrentTeamMove.Wallet.AddCoins(coinsPerMove);

		Debug.Log(CurrentTeamMove.Color);
	}

	private void UpdateTeamMovesQueue(Team[] teams)
	{
		List<Team> avaiableTeams = new List<Team>(teams);
		_teamMovesQueue = new Team[teams.Length];

		for (int i = 0; i < teams.Length; i++)
		{
			int index = Random.Range(0, avaiableTeams.Count);
			_teamMovesQueue[i] = avaiableTeams[index];
			avaiableTeams.RemoveAt(index);
		}
	}

}

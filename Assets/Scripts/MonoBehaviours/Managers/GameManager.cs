using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameDifficulty
{
	Easy,
	Medium,
	Hard,
	Custom,
}

public enum GridType
{
	None = 0,
	Hexagonal = 1,
	Square = 2,
}

public class GameManager : MonoBehaviour
{
	public GameDifficulty gameDifficulty;
	public GridType gridType;

	//hex grid
	public int gridRadius;

	//square grid
	public int gridWidth;
	public int gridHeight;

	public int minesCount;

	public static GameManager Instance { get; private set; }

	private void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}

		DontDestroyOnLoad(gameObject);
	}

	public void PlayGame()
	{
		SceneManager.LoadScene((int)gridType); //Game Scene
		SceneManager.sceneLoaded += (scene, loadMode) =>
		{
			if(scene.buildIndex == 0)
				return;

			UIManager.Instance.inGameUI.FindGridManager();
			UIManager.Instance.ChangeUserInterface(UIManager.Instance.inGameUI);
		};
	}

	public void ToMainMenu()
	{
		SceneManager.LoadScene(0); //Main Menu Scene
		SceneManager.MoveGameObjectToScene(Instance.gameObject, SceneManager.GetActiveScene()); //Remove GameManager GO from DontDestroyOnLoad()
		SceneManager.sceneLoaded += (scene, loadMode) =>
		{
			if(scene.buildIndex != 0)
				return;

			UIManager.Instance.ChangeUserInterface(UIManager.Instance.mainMenuUI);
		};
	}
}

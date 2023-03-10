using System;
using UnityEngine;
using System.Collections.Generic;
using MyCustomAttribute;

public class GameManager : Singleton<GameManager>
{
    public Transform player {get; private set;}
    private int money;
    public event Action OnStartGame;
    public event Action OnPause;
    public event Action OnResume;
    public event Action<bool> OnEndGame;
    public event Action OnNewGame;
    public event Action OnEnemyDeath;
    public event Action<int> OnUpdateMoney;
    public event Action<Transform> OnLose;
    public event Action OnEnemiesDestroyed;
    public event Action OnGoneCheckPoint;
    public GameObject uiMenu;
    private PlayerData playerData;
    public SettingData settingData;

    [ReadOnly, SerializeField] private List<Transform> enemies = new List<Transform>();
    public int enemiesCount {
        get {
            return enemies.Count;
        }
    }

    private void Start() {
        Application.targetFrameRate = 60;
    }

    public void SetPlayer(Transform player) {
        this.player =  player;
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        OnPause?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        OnResume?.Invoke();
    }

    public void EnemyDeath()
    {
        OnEnemyDeath?.Invoke();
    }

    public void GameWin()
    {
        uiMenu.GetComponent<UIMenu>().GameWin();
    }

    public void GameLose()
    {
        uiMenu.GetComponent<UIMenu>().GameLose();
        OnLose?.Invoke(player);
    }

    public void NewGame(bool win)
    {
        //Add new Level
        if (win)
        {
            playerData = PlayerData.Load();
            int nextLevel = playerData.LatestLevel+1;
            playerData.levels.Add(nextLevel);
            playerData.Save();
        }
        OnNewGame?.Invoke();
        //Invoke for Sound
        OnEndGame?.Invoke(win);
    }

    public void UpdateMoney(int amount) {
        money += amount;
        OnUpdateMoney?.Invoke(money);
    }

    public List<Transform> GetEnemies() {
        return enemies;
    }

    public void AddEnemy(Transform enemy) {
        enemies.Add(enemy);
    }

    //remove enemy kh???i danh s??ch enemies
    public void RemoveEnemy(Transform enemy) {
        enemies.Remove(enemy);
        if(enemiesCount == 0) {
            OnEnemiesDestroyed?.Invoke();
        }
    }

    //clear to??n b??? enemy
    public void ClearEnemies() {
        enemies.Clear();
    }

    public void GoneCheckPoint()
    {
        OnGoneCheckPoint?.Invoke();
    }

}

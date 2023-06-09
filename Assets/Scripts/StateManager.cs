using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState { SpawnHeroes,SpawnEnemies,HeroesTurn,EnemiesTurn}
public class StateManager : MonoBehaviour
{
    public UnityEvent OnEnemyStart;

    public static StateManager Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else if (this != Instance) { Destroy(this.gameObject); }
        ChangeState(GameState.HeroesTurn); // TODO: Fix this plx
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(GameState.EnemiesTurn);
        }
    }
    public static GameState currentState;

    public void ChangeState(GameState state)
    {
        currentState = state;

        switch(state)
        {
            case GameState.SpawnHeroes:
                break;
            case GameState.SpawnEnemies:
                break;
            case GameState.HeroesTurn:
                break;
            case GameState.EnemiesTurn:
                OnEnemyStart.Invoke();
                break;
        }
    }
}

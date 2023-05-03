using System;
using UnityEngine;

public class GameRunner: MonoBehaviour
{
    private ServiceLocator _serviceLocator;
    private StateMachine _stateMachine;
    
    private void Awake()
    {
        _serviceLocator = new ServiceLocator();
        _stateMachine = new StateMachine(_serviceLocator);
            
        DontDestroyOnLoad(gameObject);
        
        _stateMachine.Enter<InitializingState>();
    }

    private void OnApplicationQuit() => 
        _stateMachine.Enter<DisposeState>();
}
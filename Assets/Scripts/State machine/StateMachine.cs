using System;
using System.Collections.Generic;

public class StateMachine
{
    private Dictionary<Type, IState> _states;
    private IState _currentState;
    
    public StateMachine (ServiceLocator serviceLocator)
    {
        _states = new Dictionary<Type, IState>()
        {
            {typeof(InitializingState), new InitializingState(serviceLocator, this)},
            {typeof(GameLoopState), new GameLoopState(serviceLocator)},
            {typeof(DisposeState), new DisposeState(serviceLocator)},
        };
    }

    public void Enter<T>() where T : class, IState
    {
        _currentState?.Exit();
        _currentState = _states[typeof(T)];
        _currentState.Enter();
    }
}

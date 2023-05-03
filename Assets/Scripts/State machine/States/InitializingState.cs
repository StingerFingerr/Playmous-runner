using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class InitializingState : IState
{
    private ServiceLocator _serviceLocator;
    private StateMachine _stateMachine;

    public InitializingState (ServiceLocator serviceLocator, StateMachine stateMachine)
    {
        _serviceLocator = serviceLocator;
        _stateMachine = stateMachine;
    }
    
    public async void Enter()
    {
        var tasks = new Queue<Task>();
        
        tasks.Enqueue(_serviceLocator.GetService<AssetProvider>().Initialize());
        tasks.Enqueue(_serviceLocator.Initialize());
        
        await Task.WhenAll(tasks);
        
        _stateMachine.Enter<GameLoopState>();
    }

    public void Exit()
    {
        
    }
}

public class DisposeState: IState
{
    private ServiceLocator _serviceLocator;

    public DisposeState(ServiceLocator serviceLocator) => 
        _serviceLocator = serviceLocator;


    public void Enter() => 
        _serviceLocator.GetService<AssetProvider>().Dispose();

    public void Exit()
    {
        
    }
}
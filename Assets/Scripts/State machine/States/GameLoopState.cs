using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopState : IState
{
    private AssetProvider _assetProvider;
    private StaticDataProvider _staticDataProvider;
    private ServiceLocator _serviceLocator;
    
    private LevelBuilder _levelBuilder;
    private Player _player;

    private WinWindow _winWindow;
    private LoseWindow _loseWindow;
    
    public GameLoopState(ServiceLocator serviceLocator)
    {
        _serviceLocator = serviceLocator;
    }


    public void Enter()
    {
        _assetProvider = _serviceLocator.GetService<AssetProvider>();
        _staticDataProvider = _serviceLocator.GetService<StaticDataProvider>();
        
        _levelBuilder = new LevelBuilder(_staticDataProvider.levelConfigData, _assetProvider, _staticDataProvider.bustersStats);
        _player = Object.Instantiate(_assetProvider.Player).GetComponent<Player>();
        _player.Initialize(_serviceLocator.GetService<StaticDataProvider>().playerStats);

        SceneManager.SetActiveScene(_assetProvider.GameScene);

        CreateLevel();
    }

    private void CreateLevel()
    {
        _levelBuilder.Reset();
        _levelBuilder.CreateLevel();
        _player.ResetPlayer(_levelBuilder.StartPoint.transform.position, OnWin, OnLose);
    }


    private async void OnWin()
    {
        _winWindow = await 
            _assetProvider.InstantiateAsync<WinWindow>(_serviceLocator.GetService<AssetReferences>().winWindow);
        _winWindow.Show(() =>
        {
            _assetProvider.ReleaseInstance(_winWindow.gameObject);
            CreateLevel();
        },
            _levelBuilder.GetReport());
    }

    private async void OnLose()
    {
        _loseWindow = await
            _assetProvider.InstantiateAsync<LoseWindow>(_serviceLocator.GetService<AssetReferences>().loseWindow);
        _loseWindow.Show(() =>
        {
            _assetProvider.ReleaseInstance(_loseWindow.gameObject);
            CreateLevel();
        }, () =>
        {
            _assetProvider.ReleaseInstance(_loseWindow.gameObject);
            ContinueLevel();
        },
            _levelBuilder.GetReport(_player.LastBlock));
    }

    private void ContinueLevel()
    {
        _player.RespawnPlayer();
    }


    public void Exit()
    {
        
    }
}

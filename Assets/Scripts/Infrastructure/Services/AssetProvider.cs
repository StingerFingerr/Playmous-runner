using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blocks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AssetProvider: IInitializable, IDisposable, IService
{
    public GameObject Player;
    public Scene GameScene;
    public List<BaseBlock> Blocks = new List<BaseBlock>(5);
    public List<BaseBuster> Busters = new List<BaseBuster>(2);
    public GameObject WinWindow;
    public GameObject LoseWindow;


    public AssetReferences AssetReferences;

    private List<AsyncOperationHandle> _resources = new List<AsyncOperationHandle>();

    public async Task Initialize()
    {
        AsyncOperationHandle<AssetReferences> handle = Addressables.LoadAssetAsync<AssetReferences>("Assets references");
        await handle.Task;
        AssetReferences = handle.Result;
        _resources.Add(handle);
        await LoadAssets();
    }

    private async Task LoadAssets()
    {
        await LoadPlayer();
        await LoadGameScene();
        await LoadBlocks();
        await LoadBusters();
    }

    private async Task LoadBusters()
    {
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(
            AssetReferences.busters,
            buster => { this.Busters.Add(buster.GetComponent<BaseBuster>()); });
        await handle.Task;
        _resources.Add(handle);
    }

    private async Task LoadBlocks()
    {
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(
            AssetReferences.blocks,
            block =>
            {
                Blocks.Add(block.GetComponent<BaseBlock>());
            });

        await handle.Task;
        _resources.Add(handle);
    }

    private async Task LoadGameScene()
    {
        AsyncOperationHandle<SceneInstance> scene = AssetReferences.gameScene.LoadSceneAsync();
        await scene.Task;
        GameScene = scene.Result.Scene;
        _resources.Add(scene);
    }

    private async Task LoadPlayer()
    {
        AsyncOperationHandle<GameObject> player = AssetReferences.player.LoadAssetAsync<GameObject>();
        await player.Task;
        Player = player.Result;
        _resources.Add(player);
    }

    public async Task<T> GetAssetAsync<T>(object key)
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;
        _resources.Add(handle);
        return handle.Result;
    }

    public async Task<T> InstantiateAsync<T>(AssetReferenceGameObject assetReference)
    {
        AsyncOperationHandle<GameObject> handle = assetReference.InstantiateAsync();
        await handle.Task;
        return handle.Result.GetComponent<T>();
    }

    public void ReleaseInstance(GameObject instance)
    {
        instance.SetActive(false);
        Addressables.ReleaseInstance(instance);
    }

    public void Dispose()
    {
        Addressables.Release(_resources);
    }
}
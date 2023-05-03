using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ServiceLocator: IInitializable
{
    private Dictionary<Type, IService> _services;

    public ServiceLocator ()
    {
        _services = new Dictionary<Type, IService>();
        
        SetService<AssetProvider>(new AssetProvider());
    }

    private void SetService<TService>(IService service) where TService : class, IService => 
        _services.Add(typeof(TService), service);

    public TService GetService<TService>() where TService : class, IService => 
        _services[typeof(TService)] as TService;

    public async Task Initialize()
    {
        SetService<StaticDataProvider>(
            await GetService<AssetProvider>().GetAssetAsync<StaticDataProvider>("Static Data Provider"));
        SetService<AssetReferences>(
            GetService<StaticDataProvider>().assetReferences);
    }
}
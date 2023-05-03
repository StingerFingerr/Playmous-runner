using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Static data/Create assets references", fileName = "Assets references")]
public class AssetReferences: ScriptableObject, IService
{
    public AssetReferenceGameObject player;
    public AssetReference gameScene;
    public AssetLabelReference blocks;
    public AssetLabelReference busters;
    public AssetReferenceGameObject winWindow;
    public AssetReferenceGameObject loseWindow;
}
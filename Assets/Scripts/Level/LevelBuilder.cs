using System.Collections.Generic;
using System.Linq;
using Blocks;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class LevelBuilder
{
    public BaseBlock StartPoint { get; private set; }
    private LevelConfigData _configData;
    private AssetProvider _assetProvider;
    private BustersStats _bustersStats;

    private List<BaseBlock> _blocks;
    private List<BaseBlock> _blocksPrefabs;

    private List<BaseBuster> _busters;
    private List<BaseBuster> _bustersPrefabs;

    public LevelBuilder (LevelConfigData configData, AssetProvider assetProvider, BustersStats bustersStats)
    {
        _configData = configData;
        _assetProvider = assetProvider;
        _bustersStats = bustersStats;
    }
    
    public void CreateLevel()
    {
        if (_blocks is null)
        {
            _blocksPrefabs = _assetProvider.Blocks;
            _blocks = new List<BaseBlock>();

            _bustersPrefabs = _assetProvider.Busters;
            _busters = new List<BaseBuster>();
        }

        var block = SetFirstBlock();
        for (int i = 1; i < _configData.blocksAmount; i++)        
            block = SetNextBlock(block);
        SetLastBlock(block);
    }

    private void SetLastBlock(BaseBlock block)
    {
        var newBlock = GetBlock(BlockType.FinishBlock);
        newBlock.transform.position = block.transform.position + block.transform.forward;
    }

    private BaseBlock SetFirstBlock()
    {
        var block = GetBlock();
        block.transform.position = Vector3.zero;
        StartPoint = block;
        return block;
    }

    public void Reset() => 
        _blocks?.ForEach(b=> b.gameObject.SetActive(false));

    private BaseBlock SetNextBlock(BaseBlock previousBlock)
    {
        var newBlock = 
            previousBlock.Type == BlockType.TurnBlock ||
            previousBlock.Type == BlockType.Hole ||
            previousBlock.Type == BlockType.FenceBlock ||
            previousBlock.Type == BlockType.SawBlock
            ? GetBlock() 
            : GetBlock(BlockType.Random);
        newBlock.transform.position = previousBlock.transform.position + previousBlock.transform.forward;
        newBlock.transform.rotation = previousBlock.transform.rotation;
        newBlock.OnSpawned();
        previousBlock = newBlock;
        if (newBlock.Type == BlockType.Road)
            SpawnRandomBuster(newBlock);
        return previousBlock;
    }

    private BaseBlock GetBlock(BlockType type = BlockType.Road)
    {
        if (type == BlockType.Random)
            type = GetRandomBlockType();
        var block = _blocks.FirstOrDefault(b => b.Type == type && b.gameObject.activeSelf is false);
        if (block)
        {
            block.gameObject.SetActive(true);
            return block;
        }

        block = CreateBlock(type);
        _blocks.Add(block);
        
        block.gameObject.SetActive(true);
        return block;
    }

    private BaseBlock CreateBlock(BlockType type)
    {
        var prefab = _blocksPrefabs.Find(b => b.Type == type);
        return GameObject.Instantiate(prefab).GetComponent<BaseBlock>();
    }

    private BlockType GetRandomBlockType()
    {
        int random = Random.Range(1, 35);

        return random switch
        {
            1 => BlockType.Hole,
            2 => BlockType.FenceBlock,
            3 => BlockType.TurnBlock,
            4 => BlockType.SawBlock,
            _ => BlockType.Road
        };
    }

    public ReportData GetReport(BaseBlock lastBlock = null)
    {
        var report = new ReportData();
        foreach (var block in _blocks)
        {
            if(block.gameObject.activeSelf is false)
                continue;
            if(block == lastBlock)
                break;

            switch (block.Type)
            {
                case BlockType.Hole:
                    report.HoleBlocksAmount++;
                    break;
                case BlockType.SawBlock:
                    report.SawBlockAmount++;
                    break;
                case BlockType.FenceBlock:
                    report.FenceBlocksAmount++;
                    break;
            }
        }

        return report;
    }

    private void SpawnRandomBuster(BaseBlock block)
    {
        var spawn = Random.Range(0, 100) <= _configData.busterChance;
        if (spawn)
        {
            var buster = _busters.FirstOrDefault(b => b.gameObject.activeSelf is false);
            if (buster is null)            
                buster = CreateRandomBuster();
            buster.gameObject.SetActive(true);
            buster.transform.position = block.transform.position;
        }
    }

    private BaseBuster CreateRandomBuster()
    {
        var buster = GameObject.Instantiate(_bustersPrefabs[Random.Range(0, _bustersPrefabs.Count)]).GetComponent<BaseBuster>();
        buster.Initialize(_bustersStats);
        _busters.Add(buster);
        return buster;
    }
}
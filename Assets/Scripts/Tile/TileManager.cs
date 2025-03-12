using System;
using System.Collections;
using System.Collections.Generic;
using ilsFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class TileManager : ManagerSingleton<TileManager>,IManager,IAssemblyForeach
{
    
    private TileManagerConfig _managerConfig;
    
    private TileConfig _tileConfig;
    [ShowInInspector]
    private Dictionary<Type,int> TileIDMap;
    
    [ShowInInspector]
    private GameObject unityTileHandler;
    [ShowInInspector]
    private Grid grid;
    [ShowInInspector]
    private TilemapRenderer tilemapRenderer;

    private BaseTile[,] tiles;
    private RectInt tilesRange;
    
    
    
    
    
    
    public void Init()
    {
        _managerConfig = Config.GetConfig<TileManagerConfig>();
        _tileConfig = Config.GetConfig<TileConfig>();

        InitTileGrids();
    }
    
    public void ForeachCurrentAssembly(Type[] types)
    {
        foreach (var type in types)
        {
            if (type.IsAssignableFrom(typeof(BaseTile)) && !type.IsAbstract)
            {
                if (_tileConfig.TryGetTileID(type,out var id))
                {
                    TileIDMap.Add(type,id);
                }
                //理论上不该出现这个情况的，编辑器下直接退出Play得了，顺带警告一下
                else
                {
#if UNITY_EDITOR
                    EditorUtils.EditorUtils.ExitEditor();
                    $"出现不在ConfigID表范围内的Tile:{type.FullName},尝试TileConfig的RefreshTileID刷新ID表".WarningSelf();
#endif
                }
            }
        }
    }
    
    public void Update()
    {
       
    }

    public void LateUpdate()
    {
       
    }

    public void FixedUpdate()
    {
      
    }

    public void OnDestroy()
    {
       
    }

    public void OnDrawGizmos()
    {
      
    }

    public void OnDrawGizmosSelected()
    {
     
    }

    /// <summary>
    /// 检查输入坐标是否在地图格子范围内
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckPositionInGrid(Vector2Int position)
    {
        return tilesRange.Contains(position);
    }
    
    
    
    #region TileMethod

    private void InitTileGrids()
    {
        unityTileHandler = Object.Instantiate(_managerConfig.UnityTileHandler, Vector3.zero, Quaternion.identity,ContainerObject.transform);  
        
        grid = unityTileHandler.GetComponent<Grid>();
        tilemapRenderer = unityTileHandler.GetComponentInChildren<TilemapRenderer>();
        
        TileIDMap = new Dictionary<Type, int>();
        
        
        tiles = new BaseTile[_managerConfig.MapSize.x, _managerConfig.MapSize.y];
        tilesRange = new RectInt(0, 0, _managerConfig.MapSize.x, _managerConfig.MapSize.y);
    }

    /// <summary>
    /// 生成地形
    /// </summary>
    private void GenerateTiles()
    {
        
    }

    public bool TryGetTile(Vector2Int position, out BaseTile tile)
    {
        if (!CheckPositionInGrid(position))
        {
            tile = null;
            return false;
        }
        tile = tiles[position.x, position.y];
        return true;
    }
    
    
    
    
    private bool TryGetTileProperty<T>(out BaseTileProperty tileProperty) where T : BaseTile
    {
        return _tileConfig.TryGetTileProperty<T>(out tileProperty);
    }

    private bool TryGetTileProperty(Type type, out BaseTileProperty tileProperty)
    {
        return _tileConfig.TryGetTileProperty(type, out tileProperty);
    }

    private bool TryGetTileID(Type type, out int id)
    {
        return _tileConfig.TryGetTileID(type, out id);
    }

    private bool InnerCreateTile(Type type, out BaseTile tile)
    {
        tile = null;
        return false;
    }

    private bool InnerSetTile(Type type, Vector2Int position,int belongsToID,out BaseTile tileInstance)
    {
        if (!CheckPositionInGrid(position))
        {
            tileInstance = null;
            return false;
        }
        //创建新的Tile
        if (InnerCreateTile(type, out BaseTile tile) && TryGetTileProperty(type, out BaseTileProperty tileProperty))
        {
            tile.Initialize(tileProperty);
            tile.TileBelongToID = belongsToID;
            tile.Position = position;
            tiles[position.x, position.y] = tile;
            
            tileInstance = tile;
            return true;
        }

        tileInstance = null;
        return false;
    }


    public void SetTile(Type type, Vector2Int position, int belongsToID)
    {
        if (InnerSetTile(type,position,belongsToID, out BaseTile tile))
        {
            //程序化的音效什么的
        }
    }

    public void ReplaceTile(Type type, Vector2Int position, int belongsToID)
    {
        if (!CheckPositionInGrid(position))
        {
            return;
        }
        var oldTile = tiles[position.x, position.y];
        oldTile.Destroy();
        //程序化的执行
        
        
        //放置方块
        SetTile(type,position,belongsToID);
    }
    
    /// <summary>
    /// 摧毁方块（实际上是替换为空气Tile）
    /// </summary>
    /// <param name="position"></param>
    public void DestroyTile(Vector2Int position)
    {
        
    }
    
    #endregion
}

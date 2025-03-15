using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ilsFramework;
using Sirenix.OdinInspector;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
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


    private List<Collider2D> areaCheckBuffer;
    private List<Vector2Int> areaGetTileBuffer;
    
    //用于记录对应玩家的得分
    private Dictionary<int, float> scoreCollection;
    
    public void Init()
    {
        _managerConfig = Config.GetConfig<TileManagerConfig>();
        _tileConfig = Config.GetConfig<TileConfig>();

        areaCheckBuffer = new List<Collider2D>();
        areaGetTileBuffer = new List<Vector2Int>();
        
        scoreCollection = new Dictionary<int, float>();
        
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


    #region 获取Tile相关（重载与范围获取）

        /// <summary>
    /// 尝试获取指定坐标位置的Tile
    /// </summary>
    /// <param name="position">指定的格子坐标</param>
    /// <param name="tile">对应的Tile</param>
    /// <returns></returns>
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
    
    /// <summary>
    /// 尝试获取指定世界坐标位置的Tile
    /// </summary>
    /// <param name="worldPosition">世界坐标</param>
    /// <param name="tile">对应的Tile</param>
    /// <returns></returns>
    public bool TryGetTile(Vector2 worldPosition, out BaseTile tile)
    {
        return TryGetTile(GetTilePosition(worldPosition), out tile);
    }
    
    /// <summary>
    /// 尝试获取指定格子坐标位置的指定类型Tile
    /// </summary>
    /// <param name="position">指定的格子坐标</param>
    /// <param name="tile">对应的Tile</param>
    /// <typeparam name="T">指定的Tile类型</typeparam>
    /// <returns></returns>
    public bool TryGetTile<T>(Vector2Int position, out T tile) where T : BaseTile
    {
        if (TryGetTile(position, out var _tile) && _tile is T result)
        {
            tile = result;
            return true;
        }
        tile = null;
        return false;
    }
    /// <summary>
    /// 尝试获取指定世界坐标位置的指定类型Tile
    /// </summary>
    /// <param name="worldPosition">世界坐标</param>
    /// <param name="tile">对应的Tile</param>
    /// <typeparam name="T">指定的Tile类型</typeparam>
    /// <returns></returns>
    public bool TryGetTile<T>(Vector2 worldPosition, out T tile) where T : BaseTile
    {
        return TryGetTile(GetTilePosition(worldPosition), out tile);
    }
    /// <summary>
    /// 尝试获取指定格子坐标位置的指定类型Tile
    /// </summary>
    /// <param name="position">指定的格子坐标</param>
    /// <param name="targetType">指定的Tile类型</param>
    /// <param name="tile">对应的Tile</param>
    /// <returns>是否找到</returns>
    public bool TryGetTile(Vector2Int position, Type targetType, out BaseTile tile)
    {
        if (CheckTypeIsTileType(targetType) && CheckPositionInGrid(position))
        {
            tile = tiles[position.x, position.y];
            if (tile.GetType()== targetType)
            {
                return true;
            }
        }

        tile = null;
        return false;
    }
    /// <summary>
    /// 尝试获取指定世界坐标位置的指定类型Tile
    /// </summary>
    /// <param name="worldPosition">世界坐标</param>
    /// <param name="targetType">对应的Tile</param>
    /// <param name="tile">指定的Tile类型</param>
    /// <returns>是否找到</returns>
    public bool TryGetTile(Vector2 worldPosition, Type targetType, out BaseTile tile)
    {
        return TryGetTile(GetTilePosition(worldPosition), targetType, out tile);
    }
    
    /// <summary>
    /// 尝试获取在碰撞箱范围内所有的Tile
    /// </summary>
    /// <param name="areaCollider">所指定的Collder2d</param>
    /// <param name="areaTiles">搜索结果</param>
    public void GetAreaTiles(Collider2D areaCollider,List<BaseTile> areaTiles)
    {
        areaCheckBuffer.Clear();
        areaGetTileBuffer.Clear();
        GetCoveredCells(areaCollider,areaGetTileBuffer,areaCheckBuffer);
        foreach (var vector2Int in areaGetTileBuffer)
        {
            if (TryGetTile(vector2Int, out var tile))
            {
                areaTiles.Add(tile);
            }
        }
    }
    /// <summary>
    /// 尝试获取在碰撞箱范围内所有的Tile,并指定Tile类型
    /// </summary>
    /// <param name="areaCollider">所指定的Collider2d</param>
    /// <param name="areaTiles">搜索结果</param>
    /// <typeparam name="T">指定的Tile类型</typeparam>
    public void GetAreaTiles<T>(Collider2D areaCollider, List<T> areaTiles) where T : BaseTile
    {
        areaCheckBuffer.Clear();
        areaGetTileBuffer.Clear();
        GetCoveredCells(areaCollider,areaGetTileBuffer,areaCheckBuffer);
        foreach (var vector2Int in areaGetTileBuffer)
        {
            if (TryGetTile<T>(vector2Int, out var tile))
            {
                areaTiles.Add(tile);
            }
        }
    }
    /// <summary>
    /// 尝试获取在碰撞箱范围内所有的Tile,并指定Tile类型
    /// </summary>
    /// <param name="areaCollider">所指定的Collider2d</param>
    /// <param name="targetType">指定的Tile类型</param>
    /// <param name="areaTiles">搜索结果</param>
    public void GetAreaTiles(Collider2D areaCollider,Type targetType ,List<BaseTile> areaTiles)
    {
        areaCheckBuffer.Clear();
        areaGetTileBuffer.Clear();
        GetCoveredCells(areaCollider,areaGetTileBuffer,areaCheckBuffer);
        foreach (var vector2Int in areaGetTileBuffer)
        {
            if (TryGetTile(vector2Int,targetType, out var tile))
            {
                areaTiles.Add(tile);
            }
        }
    }

    #endregion

    
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
        return TileIDMap.TryGetValue(type, out id);
    }

    private bool CheckTypeIsTileType(Type type)
    {
        return TileIDMap.ContainsKey(type);
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

    /// <summary>
    /// 放置一个新的方块，使用此方法会直接覆盖原有Tile，不会触发原有Tile的破坏相关函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    /// <param name="belongsToID"></param>
    public void SetTile(Type type, Vector2Int position, int belongsToID)
    {
        if (InnerSetTile(type,position,belongsToID, out BaseTile tile))
        {
            //程序化的音效什么的
            SetTileRender();
            
            //检查合并
            CheckTileCanMerge(position);
        }
    }
    /// <summary>
    /// 替换一个新的方块，会先触发原有Tile的破坏相关函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    /// <param name="belongsToID"></param>
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
    /// <summary>
    /// 对方块造成伤害
    /// </summary>
    /// <param name="position"></param>
    /// <param name="damage"></param>
    public void ApplyDamageToTile(Vector2Int position, int damage)
    {
        
    }

    private void CalulateDamageToTile(Vector2Int position, int damage)
    {
        
    }
    
    #endregion

    #region 方块渲染相关

    public void SetTileRender()
    {
        
    }

    #endregion

    #region 格子相关

    public Vector2Int GetTilePosition(Vector2 position)
    {
        var v = grid.WorldToCell(position);
        return new Vector2Int(v.x,v.y);
    }
    
    
    // 检测指定碰撞体覆盖的所有格子
    public void GetCoveredCells(Collider2D targetCollider,List<Vector2Int> result,List<Collider2D> checkBuffer)
    {
        if (result == null) return;
        
        // 获取包围盒范围
        Bounds bounds = targetCollider.bounds;
        int startX = Mathf.FloorToInt(bounds.min.x);
        int startY = Mathf.FloorToInt(bounds.min.y);
        int endX = Mathf.FloorToInt(bounds.max.x - 1e-6f); // 避免浮点误差
        int endY = Mathf.FloorToInt(bounds.max.y - 1e-6f);
        
        Vector2 cellSize = new Vector2(1, 1);

        // 遍历每个格子
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Vector2 cellCenter = new Vector2(x + 0.5f, y + 0.5f);
                if (IsCellOverlapping(targetCollider, cellCenter, cellSize,checkBuffer))
                {
                    result.Add(new Vector2Int(x, y));
                }
            }
        }
    }

    // 检测单个格子是否与碰撞体重叠
    private bool IsCellOverlapping(Collider2D targetCollider, Vector2 center, Vector2 size,List<Collider2D> checkBuffer)
    {
        checkBuffer.Clear();
        Physics2D.OverlapBox(
            center, 
            size, 
            0f, 
            _managerConfig.TileContactFilter, 
            checkBuffer
        );
        
        foreach (var collider in checkBuffer)
        {
            if (collider == targetCollider) return true;
        }
        return false;
    }

    #endregion


    #region 方块合成相关

    public void CheckTileCanMerge(Vector2Int lastSetTilePosition)
    {
        if (!TryGetTile(lastSetTilePosition, out var lastTile))
        {
            return;
        }
        
        ReInitScoreCollection();
        //检查行
        //满足条件后直接返回
        if (CheckRowIsFull(lastSetTilePosition.y))
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                Vector2Int position = new Vector2Int(i,lastSetTilePosition.y);
                if (TryGetTile(position,out var tile))
                {
                    CalculateEachTileScore(scoreCollection, tile, lastTile);
                }
            }
            //计算完数值，发送合并事件给外界，并尝试清除对应行的tile
            BroadcastTileMergeEvent();
            return;
        }
                
        //检查列
        ReInitScoreCollection();
        if (CheckColumnIsFull(lastSetTilePosition.x))
        {
            for (int i = 0; i < tiles.GetLength(1); i++)
            {
                Vector2Int position = new Vector2Int(lastSetTilePosition.x,i);
                if (TryGetTile(position,out var tile))
                {
                    CalculateEachTileScore(scoreCollection, tile, lastTile);
                }
                
                
                //计算完数值，发送合并事件给外界，并尝试清除对应列的tile
                BroadcastTileMergeEvent();
                return;
            }
        }

    }
    /// <summary>
    /// 检测输入的行是否在范围内
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private bool CheckInputRowValueIsVaild(int row)
    {
        return (tilesRange.xMin, tilesRange.xMax).Contains(row);
    }
    
    /// <summary>
    /// 检测指定行是否被填满（没有空气方块）
    /// </summary>
    /// <param name="row">对应行（y轴坐标）</param>
    /// <returns>是否被填满</returns>
    public bool CheckRowIsFull(int row)
    {
        if (!CheckInputRowValueIsVaild(row))
        {
            return false;
        }
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            Vector2Int position = new Vector2Int(i, row);
            if (IsAir(position))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 检测输入的列是否在范围内
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    private bool CheckInputColumnValueIsVaild(int column)
    {
        return (tilesRange.yMin, tilesRange.yMax).Contains(column);
    }
    
    /// <summary>
    /// 检查指定列是否被填满（没有空气方块）
    /// </summary>
    /// <param name="column">对应列（x轴坐标）</param>
    /// <returns>是否被填满</returns>
    public bool CheckColumnIsFull(int column)
    {
        if (!CheckInputColumnValueIsVaild(column))
        {
            return false;
        }
        for (int i = 0; i < tiles.GetLength(1); i++)
        {
            Vector2Int position = new Vector2Int(column, i);
            if (IsAir(position))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsAir(Vector2Int position)
    {
        return TryGetTile<AirTile>(position, out _);
    }
    
    /// <summary>
    /// 计算每个方块给玩家的能量
    /// </summary>
    /// <param name="scoreCollection"></param>
    /// <param name="currectTile"></param>
    /// <param name="lastTile"></param>
    private void CalculateEachTileScore(Dictionary<int,float> scoreCollection,BaseTile currectTile,BaseTile lastTile)
    {
        //中立方块
        if (currectTile.TileBelongToID == TileManagerConfig.TileSystemID)
        {
            //更具最后一个方块属于谁来判断
            //中立则给两者都增加
            if (lastTile.TileBelongToID == TileManagerConfig.TileSystemID)
            {
                foreach (var scoreCollectionKey in scoreCollection.Keys)
                {
                    scoreCollection[scoreCollectionKey] += CalculateTileCurrectScore(currectTile);
                }
            }
            else
            {
                //给最后一个增加
                if (scoreCollection.ContainsKey(lastTile.TileBelongToID))
                {
                    scoreCollection[lastTile.TileBelongToID] += CalculateTileCurrectScore(currectTile);
                }
            }
        }
        else
        {
            if (scoreCollection.ContainsKey(currectTile.TileBelongToID))
            {
                scoreCollection[currectTile.TileBelongToID] += CalculateTileCurrectScore(currectTile);
            }
        }
    }
    /// <summary>
    /// 具体计算单个方块实际上输出的能量，目前只有最基本的，后续补充TODO
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    private float CalculateTileCurrectScore(BaseTile tile)
    {
        return tile.BaseMergeScore;
    }
    /// <summary>
    /// 重新构建scoreCollection
    /// </summary>
    private void ReInitScoreCollection()
    {
        scoreCollection.Clear();
        foreach (var playerID in GetAllPlayerIDs())
        {
            scoreCollection.Add(playerID, 0);
        }
    }

    /// <summary>
    /// 广播一个合并事件，未具体实现TODO
    /// </summary>
    private void BroadcastTileMergeEvent()
    {
        
    }
    
    #endregion


    #region Tile属性计算相关
    
    

    #endregion



    #region 获取与玩家相关属性

    /// <summary>
    /// 获取所有的玩家ID，未具体实现TODO
    /// </summary>
    /// <returns></returns>
    public List<int> GetAllPlayerIDs()
    {
        return new[] { 0, 1 }.ToList();
    }

    #endregion
}

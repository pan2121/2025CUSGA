using System;
using UnityEngine;

/// <summary>
/// 方块的基类，运行时实例
/// </summary>
public abstract class BaseTile
{
    public Vector2Int Position;
    
    public float Health;
    
    public float MaxHealth;

    public int BaseMaxHealth;

    public int TileBelongToID;

    public bool CanBeDestroyed;

    public bool CanBeMerged;

    public int BaseMergeScore;

    public int TileID;
    
    /// <summary>
    /// 需要的PropertyType，正式初始化时会将对应类型的tileProperty传入Initialize
    /// </summary>
    public abstract Type TilePropertyType { get; }
    
    public abstract void Initialize(BaseTileProperty tileProperty);



    public virtual void Destroy()
    {
        
    }
}
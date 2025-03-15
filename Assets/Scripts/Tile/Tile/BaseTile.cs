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

    /// <summary>
    /// 这个Tile隶属于哪个玩家或者系统（-1），玩家ID为自然数
    /// </summary>
    public int TileBelongToID;

    public bool CanBeDestroyed;

    public bool CanBeMerged;

    public int BaseMergeScore;

    public int TileID;
    
    /// <summary>
    /// 需要的PropertyType，正式初始化时会将对应类型的tileProperty传入Initialize
    /// </summary>
    public abstract Type TilePropertyType { get; }

    public virtual void Initialize(BaseTileProperty tileProperty)
    {
        BasePropertyInitialize(tileProperty);
    }

    public void BasePropertyInitialize(BaseTileProperty tileProperty)
    {
        BaseMaxHealth = tileProperty.BaseMaxHealth;
        
        CanBeDestroyed = tileProperty.CanBeDestroyed;

        CanBeMerged = tileProperty.CanBeMerged;

        BaseMergeScore = tileProperty.BaseMergeScore;
    }

    
    public virtual void Update()
    {
        
    }
    
    public virtual void Destroy()
    {
        
    }

    public void ApplyDamage(float damage)
    {
        
    }
}
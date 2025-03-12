using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[ShowInInlineEditors()]
public abstract class BaseTileProperty
{

    public  int BaseMaxHealth;
    
    public bool CanBeDestroyed;

    public bool CanBeMerged;

    public int BaseMergeScore;


}
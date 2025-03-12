using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ilsFramework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

[AutoBuildOrLoadConfig(TileManagerConfig.ConfigFilePath.TileConfigFilePath)]
public class TileConfig : ConfigScriptObject
{
    public override string ConfigName => "TileConfig";
    
    public const string TileTypeEnumName = "ETileType";
    public const string TileTypeEnumDescription = "";
    
    private Dictionary<Type,BaseTileProperty> TileProperties;

    [ShowInInspector]
    [LabelText("TileProperties")]
    [ListDrawerSettings(ShowFoldout = false,HideAddButton = true,HideRemoveButton = true,DraggableItems = false)]
    [Searchable]
    [InlineProperty]
    [FoldoutGroup("TileDetailConfig")]
    private List<BaseTileProperty> DictionaryValues;

    [ShowInInspector]
    private Dictionary<string, int> TileIDMaps;
    
    
    public bool TryGetTileProperty(Type type, out BaseTileProperty property)
    {
        return TileProperties.TryGetValue(type, out property);
        
    }

    public bool TryGetTileProperty<T>(out BaseTileProperty property)
    {
        return TileProperties.TryGetValue(typeof(T), out property);
    }

    
    [Button(ButtonSizes.Medium)]
    [FoldoutGroup("TileDetailConfig")]
    public void RebuildTileProperties()
    {
        TileProperties = new Dictionary<Type, BaseTileProperty>();
        foreach (var type in  TypeCache.GetTypesDerivedFrom<BaseTile>())
        {
            var instance = (BaseTile)Activator.CreateInstance(type);
            var tileproperty = (BaseTileProperty)Activator.CreateInstance(instance.TilePropertyType);
            TileProperties.Add(type,tileproperty);
        }
        DictionaryValues = TileProperties.Select((pair) => pair.Value).ToList();
    }
    [Button(ButtonSizes.Medium)]
    public void RebuildTileIDMaps()
    {
        TileIDMaps = new Dictionary<string, int>();
        foreach (var type in  TypeCache.GetTypesDerivedFrom<BaseTile>())
        {
            if (!TileIDMaps.ContainsKey(type.Name))
            {
                TileIDMaps.Add(type.Name,TileIDMaps.Count);
            }
        }
    }
#if UNITY_EDITOR
    [Button(ButtonSizes.Medium)]
    public void BuildTileIDEnum()
    {
        if (TileIDMaps.Count == 0)
        {
            return;
        }
        ScriptGenerator generator = new ScriptGenerator();
        EnumGenerator enumGenerator = new EnumGenerator(EAccessType.Public, TileTypeEnumName, TileTypeEnumDescription);
        foreach (var tileIDMap in TileIDMaps)
        {
            enumGenerator.Append((tileIDMap.Key,tileIDMap.Value));
        }
        generator.Append(enumGenerator);
        
        StackTrace st  = new StackTrace(0,true);
        
        DirectoryInfo directoryInfo = new DirectoryInfo(st.GetFrame(0).GetFileName());

        string parentPath = directoryInfo.Parent.Parent.FullName;
        generator.GenerateScript("ETileType",parentPath);
        AssetDatabase.Refresh();
    }
#endif
    
    
    [Button(ButtonSizes.Medium)]
    public void RebuildAllSets()
    {
        RebuildTileProperties();
        RebuildTileIDMaps();
    }
    public void CheckTileProperty(List<Type> tileTypes)
    {
        TileProperties ??= new Dictionary<Type, BaseTileProperty>();
        TileIDMaps ??= new Dictionary<string, int>();
        Dictionary<Type,Type> tileTotileProperties = new Dictionary<Type, Type>();
        HashSet<Type> needTileProperties = new HashSet<Type>();
        HashSet<Type> currentTileProperties = TileProperties.Select((tileProperty) => tileProperty.Key).ToHashSet();
        foreach (var tileType in tileTypes)
        {
            var instance = (BaseTile)Activator.CreateInstance(tileType);
            needTileProperties.Add(tileType);
            tileTotileProperties.Add(tileType,instance.TilePropertyType);
            if (!TileIDMaps.ContainsKey(tileType.Name))
            {
                TileIDMaps.Add(tileType.Name,TileIDMaps.Count);
            }
        }
        
        //找到没有的TileProperty
        var needAdd = needTileProperties.Except(currentTileProperties);
        //找到需要删除的TileProperty
        var needRemove = currentTileProperties.Except(needTileProperties);
        foreach (var type in needAdd)
        {
            if (tileTotileProperties.TryGetValue(type, out var tilePropertyType))
            {
                var instance = (BaseTileProperty)Activator.CreateInstance(tilePropertyType);
                TileProperties.Add(type, instance);
            }
        }

        foreach (var type in needRemove)
        {
            TileProperties.Remove(type);
        }
        DictionaryValues = TileProperties.Select((pair) => pair.Value).ToList();
    }

    public bool TryGetTileID(Type type, out int tileID)
    {
        return TileIDMaps.TryGetValue(type.Name, out tileID);
    }
    
}
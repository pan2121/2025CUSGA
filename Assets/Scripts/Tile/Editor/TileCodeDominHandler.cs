using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ilsFramework;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Editor
{
    public static class TileCodeDominHandler
    {
        [DidReloadScripts]
        private static void HandleTileSetting()
        {
            var managerConfig  = AssetDatabase.LoadAssetAtPath("Assets/Resources/ilsFramework/Configs/" + typeof(TileManagerConfig).GetCustomAttribute<AutoBuildOrLoadConfig>().ConfigTargetPath+".asset", typeof(TileManagerConfig)) as TileManagerConfig;

            if (managerConfig.AutoUpdateTileConfigs)
            {
                var tileConfig = AssetDatabase.LoadAssetAtPath("Assets/Resources/ilsFramework/Configs/" + typeof(TileConfig).GetCustomAttribute<AutoBuildOrLoadConfig>().ConfigTargetPath+".asset", typeof(TileConfig)) as TileConfig;

                List<Type> allTileTypes = TypeCache.GetTypesDerivedFrom<BaseTile>().ToList();
                
                tileConfig.CheckTileProperty(allTileTypes);
            }
            
        }
    }
}
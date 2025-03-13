using ilsFramework;
using Sirenix.OdinInspector;
using UnityEngine;

[AutoBuildOrLoadConfig(ConfigFilePath.TileManagerConfigFilePath)]
public class TileManagerConfig : ConfigScriptObject
{
    public struct ConfigFilePath
    {
        public const string TileManagerConfigFilePath = "Tile/ManagerConfig";
        
        public const string TileConfigFilePath = "Tile/TileConfig";
    }
    
    
    public override string ConfigName => "TileManagerConfig";

    public const int TileSystemID = -1;
    
    public GameObject UnityTileHandler;
    
    [LabelText("地图大小")]
    public Vector2Int MapSize = new Vector2Int(10, 10);
    
    public bool AutoUpdateTileConfigs = true;

    public LayerMask TileLayerMask;
    
    public ContactFilter2D TileContactFilter;
}
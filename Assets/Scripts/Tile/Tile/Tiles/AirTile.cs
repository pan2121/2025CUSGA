using System;

namespace Tiles
{
    public class AirTile : BaseTile
    {
        public override Type TilePropertyType => typeof(AirTileProperty);
        public override void Initialize(BaseTileProperty tileProperty)
        {
            base.Initialize(tileProperty);
        }
    }
    
    
    public class AirTileProperty : BaseTileProperty
    {
        
    }
}
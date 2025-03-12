using System;

namespace Tiles
{
    public class TEst2Tile : BaseTile
    {
        public override Type TilePropertyType => typeof(TEst2TileProproperty);
        public override void Initialize(BaseTileProperty tileProperty)
        {
            throw new NotImplementedException();
        }
    }
}
using System;

namespace Tiles
{
    public class TEst1Tile : BaseTile
    {
        public override Type TilePropertyType => typeof(TEst1TileProproperty);
        public override void Initialize(BaseTileProperty tileProperty)
        {
            throw new NotImplementedException();
        }
    }
}
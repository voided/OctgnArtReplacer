using System;

namespace OctgnArtReplacer.Octgn.Mtg
{
    class MtgContext : OctgnContext
    {
        public readonly Guid GameId = new Guid("A6C8D2E8-7CD8-11DD-8F94-E62B56D89593");

        public MtgContext()
        {

        }


        protected override Guid GetGameId()
        {
            return GameId;
        }
    }
}

using RimWorld.Planet;
using Verse;

namespace AllowTool.Settings
{
    /// <summary>
    ///     Store settings for a specific world save file
    /// </summary>
    public class WorldSettings : WorldComponent
    {
        private PartyHuntSettings partyHunt;
        private StripMineWorldSettings stripMine;

        public WorldSettings(World world) : base(world)
        {
        }

        public StripMineWorldSettings StripMine
        {
            get => stripMine ?? (stripMine = new StripMineWorldSettings());
            set => stripMine = value;
        }

        public PartyHuntSettings PartyHunt
        {
            get => partyHunt ?? (partyHunt = new PartyHuntSettings());
            set => partyHunt = value;
        }

        public override void ExposeData()
        {
            Scribe_Deep.Look(ref stripMine, "stripMine");
            Scribe_Deep.Look(ref partyHunt, "partyHunt");
        }
    }
}
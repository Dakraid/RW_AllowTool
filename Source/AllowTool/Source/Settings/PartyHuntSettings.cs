using System.Collections.Generic;
using Verse;

namespace AllowTool
{
    public class PartyHuntSettings : IExposable
    {
        private bool autoFinishOff = true;

        private bool huntDesignatedOnly;
        private HashSet<int> partyHuntingPawns = new HashSet<int>();

        private bool unforbidDrops;

        public bool AutoFinishOff
        {
            get => autoFinishOff;
            set => autoFinishOff = value;
        }

        public bool HuntDesignatedOnly
        {
            get => huntDesignatedOnly;
            set => huntDesignatedOnly = value;
        }

        public bool UnforbidDrops
        {
            get => unforbidDrops;
            set => unforbidDrops = value;
        }

        public void ExposeData()
        {
            // convert to list for serialization
            var partyHuntingList = new List<int>(partyHuntingPawns);
            Scribe_Collections.Look(ref partyHuntingList, "pawns");
            partyHuntingPawns = new HashSet<int>(partyHuntingList);

            Scribe_Values.Look(ref autoFinishOff, "finishOff", true);
            Scribe_Values.Look(ref huntDesignatedOnly, "designatedOnly");
            Scribe_Values.Look(ref unforbidDrops, "unforbid");
        }

        public bool PawnIsPartyHunting(Pawn pawn)
        {
            return partyHuntingPawns.Contains(pawn.thingIDNumber);
        }

        public void TogglePawnPartyHunting(Pawn pawn, bool enable)
        {
            var id = pawn.thingIDNumber;
            if (enable)
                partyHuntingPawns.Add(id);
            else
                partyHuntingPawns.Remove(id);
        }
    }
}
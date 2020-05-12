using Verse;

namespace AllowTool.Settings
{
    /// <summary>
    ///     Stores settings for the Strip Mine designator that unique for each save file
    /// </summary>
    public class StripMineWorldSettings : IExposable, IConfigurableStripMineSettings
    {
        private const int DefaultSpacingX = 5;
        private const int DefaultSpacingY = 5;

        private int hSpacing = DefaultSpacingX;

        private IntVec2 lastGridOffset;

        private bool showWindow = true;

        private bool variableGridOffset = true;

        private int vSpacing = DefaultSpacingY;

        public IntVec2 LastGridOffset
        {
            get => lastGridOffset;
            set => lastGridOffset = value;
        }

        public int HorizontalSpacing
        {
            get => hSpacing;
            set => hSpacing = value;
        }

        public int VerticalSpacing
        {
            get => vSpacing;
            set => vSpacing = value;
        }

        public bool VariableGridOffset
        {
            get => variableGridOffset;
            set => variableGridOffset = value;
        }

        public bool ShowWindow
        {
            get => showWindow;
            set => showWindow = value;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref hSpacing, "hSpacing", DefaultSpacingX);
            Scribe_Values.Look(ref vSpacing, "vSpacing", DefaultSpacingY);
            Scribe_Values.Look(ref variableGridOffset, "variableOffset", true);
            Scribe_Values.Look(ref showWindow, "showWindow", true);
            Scribe_Values.Look(ref lastGridOffset, "lastOffset");
        }

        public StripMineWorldSettings Clone()
        {
            return (StripMineWorldSettings) MemberwiseClone();
        }
    }
}
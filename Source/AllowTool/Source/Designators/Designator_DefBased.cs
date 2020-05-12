using AllowTool.Context;
using RimWorld;
using Verse;

namespace AllowTool
{
    /// <summary>
    ///     Base class for Allow Tool designators.
    ///     The designator is expected to have an associated <see cref="ThingDesignatorDef" />
    ///     that customizes its appearance and functionality.
    /// </summary>
    public abstract class Designator_DefBased : Designator, IReversePickableDesignator, IGlobalHotKeyProvider
    {
        private bool visible = true;

        protected Designator_DefBased()
        {
            useMouseIcon = true;
            soundDragSustain = SoundDefOf.Designate_DragStandard;
            soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
        }

        public ThingDesignatorDef Def { get; private set; }

        public override bool Visible => visible;

        public KeyBindingDef GlobalHotKey => Def.hotkeyDef;

        public virtual Designator PickUpReverseDesignator()
        {
            return this;
        }

        protected void UseDesignatorDef(ThingDesignatorDef def)
        {
            Def = def;
            defaultLabel = def.label;
            defaultDesc = def.description;
            soundSucceeded = def.soundSucceeded;
            hotKey = def.hotkeyDef;
            visible = AllowToolController.Instance.Handles.IsDesignatorEnabled(def);
            ResolveIcon();
            OnDefAssigned();
        }

        protected virtual void OnDefAssigned()
        {
        }

        protected virtual void ResolveIcon()
        {
            Def.GetIconTexture(tex => icon = tex);
        }
    }
}
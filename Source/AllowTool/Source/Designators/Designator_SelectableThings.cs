using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AllowTool
{
    /// <summary>
    ///     Base class for all designators that use a dragger to select Things, rather than cells.
    ///     The purpose is twofold: efficiently highlight cells with valid things while designating an unlimited area
    ///     and deliver confirmation messages to the player that include the number of affected items.
    /// </summary>
    public abstract class Designator_SelectableThings : Designator_UnlimitedDragger
    {
        private Material highlightMaterial;

        protected Designator_SelectableThings()
        {
            var highlighter = new MapCellHighlighter(SelectHighlightedCells);
            Action<CellRect> clearHighlightedCells = r => highlighter.ClearCachedCells();
            Dragger.SelectionStart += clearHighlightedCells;
            Dragger.SelectionChanged += clearHighlightedCells;
            Dragger.SelectionComplete += clearHighlightedCells;
            Dragger.SelectionUpdate += r => highlighter.DrawCellHighlights();
        }

        protected override void OnDefAssigned()
        {
            Def.GetDragHighlightTexture(tex =>
                highlightMaterial = MaterialPool.MatFrom(tex, ShaderDatabase.MetaOverlay, Color.white)
            );
        }

        public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
        {
            // the cells argument is empty, because we return false in CanDesignateCell. 
            // We have our own Dragger we can query for cells, however.
            var map = Find.CurrentMap;
            if (map == null) return;
            
            var thingGrid = map.thingGrid;
            var mapRect = Dragger.SelectedArea.ClipInsideMap(map);
            var designateableThings = new List<Thing>();
            var hitCount = 0;
            foreach (var cell in mapRect.Cells)
            {
                var cellThings = thingGrid.ThingsListAtFast(cell);
                foreach (var t in cellThings.Where(t => CanDesignateThing(t).Accepted))
                {
                    designateableThings.Add(t);
                    hitCount++;
                }
            }

            DesignateMultiThing(designateableThings);
            if (hitCount > 0)
            {
                if (Def.messageSuccess != null) Messages.Message(Def.messageSuccess.Translate(hitCount.ToString()), MessageTypeDefOf.SilentInput);
                FinalizeDesignationSucceeded();
            }
            else
            {
                if (Def.messageFailure != null) Messages.Message(Def.messageFailure.Translate(), MessageTypeDefOf.RejectInput);
                FinalizeDesignationFailed();
            }
        }

        private void DesignateMultiThing(IEnumerable<Thing> things)
        {
            foreach (var thing in things) DesignateThing(thing);
        }

        private IEnumerable<MapCellHighlighter.Request> SelectHighlightedCells()
        {
            var allTheThings = Map.listerThings.AllThings;
            foreach (var thing in allTheThings.Where(thing => thing.def.selectable && Dragger.SelectedArea.Contains(thing.Position) && CanDesignateThing(thing).Accepted)) yield return new MapCellHighlighter.Request(thing.Position, highlightMaterial);
        }
    }
}
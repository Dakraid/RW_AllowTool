﻿using System;
using Verse;

namespace AllowTool
{
    /// <summary>
    ///     Def for AllowTool designators to be used as reverse designators (designators shown on selected items).
    ///     These are automatically instantiated and injected.
    /// </summary>
    public class ReverseDesignatorDef : Def
    {
        public Type designatorClass;
        public ThingDesignatorDef designatorDef;
        public Type replaces;
    }
}
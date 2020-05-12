// ReSharper disable UnassignedField.Global

using System;
using HugsLib;
using UnityEngine;
using Verse;

namespace AllowTool
{
    /// <summary>
    ///     Base def for AllowTool designators.
    /// </summary>
    public class ThingDesignatorDef : Def
    {
        private readonly DeferredTextureResolver highlightResolver = new DeferredTextureResolver();
        private readonly DeferredTextureResolver iconResolver = new DeferredTextureResolver();

        public Type designatorClass;
        public string dragHighlightTex;
        public KeyBindingDef hotkeyDef = null;
        public string iconTex;
        public string messageFailure = null;
        public string messageSuccess = null;
        public SoundDef soundSucceeded = null;

        public void GetIconTexture(Action<Texture2D> onLoaded)
        {
            iconResolver.ResolveTexture(iconTex, onLoaded);
        }

        public void GetDragHighlightTexture(Action<Texture2D> onLoaded)
        {
            highlightResolver.ResolveTexture(dragHighlightTex, onLoaded);
        }

        public override void PostLoad()
        {
            Assert(designatorClass != null, "designatorClass field must be set");
            Assert(designatorClass != null && typeof(Designator_DefBased).IsAssignableFrom(designatorClass),
                "designatorClass must extend " + nameof(Designator_DefBased));
            Assert(iconTex != null, "icon texture must be set");
            Assert(dragHighlightTex != null, "drag highlight texture must be set");
        }

        private void Assert(bool check, string errorMessage)
        {
            if (!check) Log.Error($"[AllowTool] Invalid data in ThingDesignatorDef {defName}: {errorMessage}");
        }

        // ensures that textures are loaded in the main thread, since designators are created in a work thread while the game is loading
        private class DeferredTextureResolver
        {
            private bool resolved;
            private Texture2D texture;

            public void ResolveTexture(string path, Action<Texture2D> onLoaded)
            {
                if (resolved)
                    onLoaded(texture);
                else
                    HugsLibController.Instance.DoLater.DoNextUpdate(() =>
                    {
                        resolved = true;
                        texture = ContentFinder<Texture2D>.Get(path);
                        onLoaded(texture);
                    });
            }
        }
    }
}
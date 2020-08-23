using System;
using System.Linq;
using UnityEngine;

namespace Deepwell
{
    public static class LayerMaskExtensions
    {
        public static bool matchesLayer(this LayerMask mask, int layer)
        {
            return ((1 << layer) & mask) != 0;
        }

        public static int GetMaskOrThrow(params string[] layerNames)
        {
            var mask = LayerMask.GetMask(layerNames);
            if (mask == 0)
            {
                var layerNameList = layerNames.Aggregate("", (current, item) => current + (current.Length != 0 ? "|":"") + item);
                throw new Exception($"At least one of the layers of [{layerNameList}] does not exist.");
            }

            return mask;
        }
    }
}
//LayerMaskExtensions from Unity Community Wiki: http://wiki.unity3d.com/index.php/LayerMaskExtensions
//improved by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/10

using UnityEngine;
using System.Collections.Generic;

namespace UnityExtensions
{
    public static class LayerMaskExtensions
    {
        public static LayerMask Create(params string[] layerNames)
        {
            return NamesToMask(layerNames);
        }

        public static LayerMask Create(params int[] layerNumbers)
        {
            return LayerNumbersToMask(layerNumbers);
        }

        public static LayerMask NamesToMask(params string[] layerNames)
        {
            LayerMask ret = 0;
            foreach (var name in layerNames)
            {
                ret |= (1 << LayerMask.NameToLayer(name));
            }
            return ret;
        }

        public static LayerMask LayerNumbersToMask(params int[] layerNumbers)
        {
            LayerMask ret = 0;
            foreach (var layer in layerNumbers)
            {
                ret |= (1 << layer);
            }
            return ret;
        }

        public static LayerMask Inverse(this LayerMask original)
        {
            return ~original;
        }

        public static LayerMask AddToMask(this LayerMask original, params string[] layerNames)
        {
            return original | NamesToMask(layerNames);
        }

        public static LayerMask RemoveFromMask(this LayerMask original, params string[] layerNames)
        {
            LayerMask invertedOriginal = ~original;
            return ~(invertedOriginal | NamesToMask(layerNames));
        }

        public static string[] MaskToNames(this LayerMask original)
        {
            var output = new List<string>();

            for (int i = 0; i < 32; ++i)
            {
                int shifted = 1 << i;
                if ((original & shifted) == shifted)
                {
                    string layerName = LayerMask.LayerToName(i);
                    if (!string.IsNullOrEmpty(layerName))
                    {
                        output.Add(layerName);
                    }
                }
            }
            return output.ToArray();
        }

        public static string MaskToString(this LayerMask original)
        {
            return MaskToString(original, ", ");
        }

        public static string MaskToString(this LayerMask original, string delimiter)
        {
            return string.Join(delimiter, MaskToNames(original));
        }

        /// <summary>
        /// Check if the target Layer is in the LayerMask.
        /// </summary>
        /// <param name="layer">The target layer. Try "gameobject.layer" </param>
        /// <param name="layerMask">The layerMask. Consider using along with AddToMask() if you want to check multiple layer masks.</param>
        /// <returns></returns>
        public static bool IsInLayerMask(int layer, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << layer));
        }
        /// <summary>
        /// Check if the (layer of) target object is in the LayerMask.
        /// </summary>
        /// <param name="obj">The target gameobject.</param>
        /// <param name="layerMask">The layerMask. Consider using along with AddToMask() if you want to check multiple layer masks.</param>
        /// <returns></returns>
        public static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << obj.layer));
        }

    }
}
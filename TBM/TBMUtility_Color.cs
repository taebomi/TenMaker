using UnityEngine;

namespace TenMaker.Utility
{
    public static partial class TBMUtility
    {
        
        
        public static Color FromHex(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out var color) is false)
            {
                color = Color.white;
                Debug.LogError($"Failed to parse color from hex string: {hex}");
            }

            return color;
        }
    }
}
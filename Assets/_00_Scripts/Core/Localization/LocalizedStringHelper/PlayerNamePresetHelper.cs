using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

namespace TenMaker.Core.Localization
{
    public class PlayerNamePresetHelper
    {
        public static async UniTask<string> GetRandomName()
        {
            const string tableName = "PlayerNamePreset";
            var randomPrefixKey = $"prefix_{Random.Range(1, 101):000}";
            var randomSuffixKey = $"suffix_{Random.Range(1, 101):000}";
            var prefixLocalizedString = new LocalizedString(tableName, randomPrefixKey);
            var suffixLocalizedString = new LocalizedString(tableName, randomSuffixKey);
            var prefix = await prefixLocalizedString.GetLocalizedStringAsync();
            var suffix = await suffixLocalizedString.GetLocalizedStringAsync();
            if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(suffix))
            {
                return $"Guest{Random.Range(0, 100000):000000}";
            }

            return $"{prefix}{suffix}";
        }
    }
}
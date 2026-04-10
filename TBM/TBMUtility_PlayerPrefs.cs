using System;
using System.Globalization;
using UnityEngine;

namespace TenMaker.Utility
{
    public static class PlayerPrefsUtility
    {
        #region Wrapper

        public static bool HasKey(string key) => PlayerPrefs.HasKey(key);
        public static void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);


        public static float GetFloat(string key, float value) => PlayerPrefs.GetFloat(key, value);
        public static void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);

        public static string GetString(string key) => PlayerPrefs.GetString(key);
        public static string GetString(string key, string value) => PlayerPrefs.GetString(key, value);
        public static void SetString(string key, string value) => PlayerPrefs.SetString(key, value);

        public static int GetInt(string key, int value) => PlayerPrefs.GetInt(key, value);
        public static void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);

        #endregion

        #region bool

        public static void SetBool(string key, bool value)
        {
            var intValue = value ? 1 : 0;
            PlayerPrefs.SetInt(key, intValue);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            var intValue = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
            if (intValue is not (0 or 1))
            {
                TBMLog.HeaderWarning($"PlayerPrefs ({key}) is not bool.");
            }

            return intValue != 0;
        }

        #endregion

        #region DateTime

        public static void SetDateTime(string key, DateTime dateTime)
        {
            var dateString = $"{dateTime:O}";
            PlayerPrefs.SetString(key, dateString);
        }

        public static DateTime GetDateTime(string key, DateTime defaultValue = default)
        {
            if (PlayerPrefs.HasKey(key) is false)
            {
                return defaultValue;
            }

            var dateString = PlayerPrefs.GetString(key);
            if (!DateTime.TryParseExact
                    (dateString, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dateTime))
            {
                TBMLog.HeaderWarning($"PlayerPrefs ({key}, {dateString}) is not date.");
                return defaultValue;
            }

            return dateTime;
        }

        #endregion
    }
}
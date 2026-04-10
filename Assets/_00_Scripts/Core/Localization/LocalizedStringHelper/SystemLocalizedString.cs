using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace TenMaker.Core.Localization.Keys
{
    public static class SystemLocalizedString
    {
        public const string TABLE_NAME = "System";
        public static TableReference TableReference => TABLE_NAME;

        public static LocalizedString Ok => new(TableReference, "Ok");
        public static LocalizedString Yes => new(TableReference, "yes");
        public static LocalizedString No => new(TableReference, "no");
        public static LocalizedString Confirm => new(TableReference, "confirm");
        public static LocalizedString Cancel => new(TableReference, "cancel");
        public static LocalizedString Warning => new(TableReference, "warning");
        public static LocalizedString Info => new(TableReference, "info");

        public static LocalizedString ContactUs => new(TableReference, "contact_us");

        public static LocalizedString Error => new(TableReference, "error");
        public static LocalizedString Error_Code => new(TableReference, "error.code");
        public static LocalizedString Error_Message => new(TableReference, "error.message");
        public static LocalizedString Error_Code_And_Message => new(TableReference, "error.code_and_message");

        public static LocalizedString ArguOnly => new(TableReference, "argu_only");

        public static LocalizedString Get(string key) => new(TableReference, key);
    }
}
using TenMaker.Core.Localization.Keys;
using UnityEngine.Localization;

namespace TenMaker.Core.Localization
{
    public static class LocalizedStringHelper
    {


        public static LocalizedString GetLocalizedString(params object[] arguments)
        {
            var message = Keys.SystemLocalizedString.ArguOnly;
            message.Arguments = arguments;
            return message;
        }
    }
}
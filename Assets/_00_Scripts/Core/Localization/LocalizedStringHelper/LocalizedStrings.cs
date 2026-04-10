using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace TenMaker.Core.Localization
{
    public static class LocalizedStrings
    {
        public static class System
        {
            public const string TABLE_NAME = "System";

            public static TableReference TableReference = TABLE_NAME;
            public static LocalizedString Get(string key) => new(TableReference, key);

            public static LocalizedString Button_Confirm => Get("button.confirm");
            
            public static LocalizedString quit_game => Get("quit_game");

            public static LocalizedString MessageBoxButton_Confirm => Get("messagebox.button.confirm");
            public static LocalizedString MessageBoxButton_Yes => Get("messagebox.button.yes");
            public static LocalizedString MessageBoxButton_No => Get("messagebox.button.no");

            public static LocalizedString Authentication_Initial_SignIn_Failed => Get("authentication.initial_sign_in.failed");
            public static LocalizedString Authentication_SignIn_Success => Get("authentication.sign_in.success");
            public static LocalizedString Authentication_SignIn_Failed => Get("authentication.sign_in.failed");
            public static LocalizedString Authentication_SignOut_Success => Get("authentication.sign_out.success");
            public static LocalizedString Authentication_SignOut_Failed => Get("authentication.sign_out.failed");
        }

        public static class MainScene
        {
            public const string TABLE_NAME = "MainScene";

            public static TableReference TableReference = TABLE_NAME;
            public static LocalizedString Get(string key) => new(TableReference, key);
            
            public static LocalizedString Multiplay_JoinRoom_MessageBox => Get("multiplay.join_room.messagebox");
            public static LocalizedString Multiplay_CreateRoom_Failed => Get("multiplay.create_room.failed");
            public static LocalizedString Multiplay_JoinRoom_Failed => Get("multiplay.join_room.failed");
            public static LocalizedString Multiplay_LeaveRoom_Message => Get("multiplay.leave_room.message");
        }
    }
}
namespace MobileCommander.Const
{
    public static class AdbCommandKeywords
    {
        public const string FileName = "adb";

        public const string KillBackgroundTasks = "shell am kill-all";

        public const string InitChrome = "shell am start -n com.android.chrome/com.google.android.apps.chrome.Main";

        public const string Enter = "shell input keyevent 66";

        public const string GetCurrentScreenAndSave = "shell uiautomator dump /sdcard/ui_dump.xml";

        public const string DownloadScreen = "shell cat /sdcard/ui_dump.xml";

        public const string SearchQueryForChrome = "shell input text \"{0}\"";
    }
}

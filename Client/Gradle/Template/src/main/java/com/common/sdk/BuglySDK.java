package com.common.sdk;

import com.common.manager.Manager;

//微信导入
import com.tencent.bugly.crashreport.BuglyLog;
import com.tencent.bugly.crashreport.CrashReport;

public class BuglySDK extends SDKBase
{
    private static String TAG = "JJJ_BuglySDK";

    // APP_ID 替换为你的应用从官方网站申请到的合法appID
    private static String m_appId = "c8972f4814";

    @Override public void onCreate()
    {
        CrashReport.initCrashReport(Manager.getActivity(), m_appId, !Manager.getReleaseBoolean());
    }

    public static void logV(String tag, String log) {
        BuglyLog.v(tag, log);
    }

    public static void logD(String tag, String log) {
        BuglyLog.d(tag, log);
    }

    public static void logI(String tag, String log) {
        BuglyLog.i(tag, log);
    }

    public static void logW(String tag, String log) {
        BuglyLog.w(tag, log);
    }

    public static void logE(String tag, String log) {
        BuglyLog.e(tag, log);
    }

    public static void postCatchedException(Throwable throwable) {
        CrashReport.postCatchedException(throwable);
    }
}

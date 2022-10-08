package com.common.sdk;

import com.common.manager.Manager;
import com.tenjin.android.TenjinSDK;

public class cTenjinSDK extends SDKBase
{
    public static String Tag = "cTenjinSDK";
    private String APIKey = "LYWC4C72BZOEBRSVPGYZS5P425MRKR3G";
    private TenjinSDK Instance;

    @Override
    public void onCreate() {
        Instance = TenjinSDK.getInstance(Manager.getActivity(), APIKey);
        Instance.setAppStore(TenjinSDK.AppStoreType.googleplay);
    }

    public void onResume() {
        Instance.connect();
    }

    /**
     * 上报数据
     * @param msg
     */
    public void ReportData(String msg)
    {
        if (null == msg || 0 == msg.length())
        {
            return;
        }
        Instance.eventWithName(msg);
    }
}
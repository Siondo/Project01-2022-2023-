package com.common.sdk;

import com.common.manager.Manager;
import android.app.Activity;
import android.util.Log;
//友盟库导入
import com.umeng.commonsdk.UMConfigure;
import com.umeng.commonsdk.listener.OnGetOaidListener;
import com.umeng.analytics.MobclickAgent;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

public class UMengSDK extends SDKBase
{
    //TAG
    private static String TAG = "JJJ_UMengSDK";

    /**
     * 启动
     */
    @Override public void onCreate()
    {
        //获取Appkey和渠道
        String appKey = Manager.getInstance().getMetaDataString(Manager.META_DATA_UMAPPKEY);
        String channel = Manager.getInstance().channel;
        Log.i(TAG, String.format("appKey: %s, channel: %s", appKey, channel));

        //友盟SDK,设置LOG开关，默认为false
        UMConfigure.setLogEnabled(!Manager.getReleaseBoolean());
        //友盟SDK预初始化函数
        UMConfigure.preInit(Manager.getActivity(),appKey,channel);
        //同意隐私政策
        UMConfigure.submitPolicyGrantResult(Manager.getActivity(), true);
        //支持在子进程中统计自定义事件
        UMConfigure.setProcessEvent(true);
        //用户同意《隐私政策》授权后，友盟SDK正式初始化函数
        UMConfigure.init(Manager.getActivity(), appKey, channel, UMConfigure.DEVICE_TYPE_PHONE, "secret");
        //UM采集
        UMConfigure.getOaid(Manager.getActivity(), new OnGetOaidListener() {
            @Override
            public void onGetOaid(String oaid) {
                if (null != oaid)
                {
                    Manager.getInstance().SendOaid(oaid);
                    Log.i(TAG, "oaid: "+ oaid);
                }
            }
        });
    }

    /**
     * 登录成功
     * @param json
     */
    @Override public void onLoginSuccess(JSONObject json)
    {
        //try
        {
            //String provider = json.getString("provider");
            //String ID = json.getString("ID");
            //当用户使用第三方账号（如微信）登录时，可以这样统计：
            //MobclickAgent.onProfileSignIn(provider, ID);
        }
        //catch (JSONException e)
        {
            //BuglySDK.postCatchedException(e);
            //Log.e(TAG, String.format("onLoginSuccess: error: %s", e.toString()));
        }
    }

    /**
     * 登出
     */
    @Override public void onLogout()
    {
        MobclickAgent.onProfileSignOff();
    }

    /**
     * 退出
     */
    @Override public void onExit()
    {
        MobclickAgent.onKillProcess(Manager.getActivity());
    }

    /**
     * 自定义数据埋点
     * @param context
     * @param msg
     */
    @Override public void onEventObject(Activity context, String msg) {
        try {
            if (null == msg)
            {
                msg = "";
            }
            Log.i(TAG, String.format("onEventObject: %s", msg));
            //JSONObject json = new JSONObject(msg);
            //String event = json.getString("event");

            Map<String, Object> map = new HashMap<String, Object>();
            map.put("event_name", msg);
            //map.put("palyer_id", json.getString("id"));
            //map.put("event_value", json.getString("value"));
            MobclickAgent.onEventObject(context, msg, map);
        }
        catch (Exception e)
        {
            BuglySDK.postCatchedException(e);
            Log.e(TAG, String.format("onEventObject: %s", e.toString()));
        }
    }
}

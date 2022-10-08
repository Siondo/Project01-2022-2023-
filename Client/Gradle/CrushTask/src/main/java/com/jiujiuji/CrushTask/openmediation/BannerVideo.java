package com.jiujiuji.CrushTask.openmediation;

import android.app.Activity;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;

import com.common.manager.Manager;
import com.common.sdk.OpenMediationSDK;
import com.common.sdk.SDKManager;
import com.openmediation.sdk.banner.AdSize;
import com.openmediation.sdk.banner.BannerAd;
import com.openmediation.sdk.banner.BannerAdListener;
import com.openmediation.sdk.nativead.AdInfo;
import com.openmediation.sdk.utils.error.Error;

import org.json.JSONObject;

public class BannerVideo extends Activity
{
    public static BannerAd bannerAd;
    public static RelativeLayout adParent;
    private static String placementId = "11586";
    private static boolean needShow = false;

    public static void onShowBannerAd()
    {
        needShow = true;
        if (bannerAd != null) {
            Log.e(OpenMediationSDK.Tag, "横幅广告开始播放");
            bannerAd.loadAd();
        }
        else {
            Log.e(OpenMediationSDK.Tag, "无法播放横幅广告, 没有初始化成功");
            ImpressionVideoData.onSendCallBack("Banner", new JSONObject(), false);
        }
    }

    public static void onInitBannerAd()
    {
        bannerAd = new BannerAd(placementId, new BannerAdListener() {
            @Override
            public void onBannerAdLoaded(String s, View view) {
                if (!needShow)
                {
                    return;
                }
                if (null != view.getParent()) {
                    ((ViewGroup) view.getParent()).removeView(view);
                }

                adParent.removeAllViews();
                RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(
                        RelativeLayout.LayoutParams.WRAP_CONTENT, RelativeLayout.LayoutParams.WRAP_CONTENT);
                layoutParams.addRule(RelativeLayout.ALIGN_PARENT_BOTTOM);
                layoutParams.addRule(RelativeLayout.CENTER_HORIZONTAL);
                adParent.addView(view, layoutParams);
                ImpressionVideoData.onSendCallBack("Banner", ImpressionVideoData.BannerJsonInfo, true);
            }

            @Override
            public void onBannerAdLoadFailed(String s, Error error) {
                ImpressionVideoData.onSendCallBack("Banner", new JSONObject(), false);
            }

            @Override
            public void onBannerAdClicked(String s) {
                Log.e(OpenMediationSDK.Tag, "横幅广告清理成功");
                adParent.removeAllViews();
            }
        });

        bannerAd.setAdSize(AdSize.BANNER); //设置尺寸
    }

    public static void onDestroyBannerAd()
    {
        if (bannerAd != null) {
            bannerAd.destroy();
        }
    }

    public static void BannerAdHide()
    {
        needShow = false;
        Log.i(OpenMediationSDK.Tag, "BannerAdHide: ");
        Manager.getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                adParent.removeAllViews();
            }
        });
    }
}

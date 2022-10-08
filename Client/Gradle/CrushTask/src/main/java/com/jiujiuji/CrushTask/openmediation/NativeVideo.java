package com.jiujiuji.CrushTask.openmediation;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.common.manager.Manager;
import com.common.sdk.OpenMediationSDK;
import com.jiujiuji.CrushTask.MainActivity;
import com.jiujiuji.CrushTask.R;
import com.openmediation.sdk.nativead.AdIconView;
import com.openmediation.sdk.nativead.AdInfo;
import com.openmediation.sdk.nativead.MediaView;
import com.openmediation.sdk.nativead.NativeAd;
import com.openmediation.sdk.nativead.NativeAdListener;
import com.openmediation.sdk.nativead.NativeAdView;
import com.openmediation.sdk.utils.error.Error;

import org.json.JSONObject;

public class NativeVideo extends Activity
{
    public static NativeAdView nativeAdView;
    public static AdInfo adInfo;
    public static View adView;
    public static RelativeLayout adContainer;
    public static String placementId = "11587";
    public static Context mainAcitive;

    public static void onShowNativeAd()
    {
        Log.e(OpenMediationSDK.Tag,"原生广告开始播放");
        NativeAd.loadAd(placementId);
    }

    public static void onInitNativeVideoAd()
    {
        NativeAdListener listener = new NativeAdListener() {
            @Override
            public void onNativeAdLoaded(String s, AdInfo info) {

                adContainer.removeAllViews();
                adInfo = info;
                // Native Template rendering, for Admost, TikTok_cn, Tencent
                if (info.isTemplateRender()) {
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(
                            RelativeLayout.LayoutParams.WRAP_CONTENT, RelativeLayout.LayoutParams.WRAP_CONTENT);
                    layoutParams.addRule(Gravity.CENTER);
                    adContainer.addView(info.getView(), layoutParams);
                    Log.e(OpenMediationSDK.Tag,"Admost, TikTok_cn, Tencent");
                } else {
                    adView = LayoutInflater.from(mainAcitive).inflate(R.layout.native_ad_layout, null);
                    TextView title = adView.findViewById(R.id.ad_title);
                    title.setText(info.getTitle());
                    //TextView desc = adView.findViewById(R.id.desc);
                    //desc.setText(info.getDesc());
                    Button btn = adView.findViewById(R.id.ad_btn);
                    btn.setText(info.getCallToActionText());
                    MediaView mediaView = adView.findViewById(R.id.ad_media);
                    nativeAdView = new NativeAdView(mainAcitive);
                    AdIconView adIconView = adView.findViewById(R.id.ad_icon_media);
                    nativeAdView.addView(adView);
                    nativeAdView.setTitleView(title);
                    //nativeAdView.setDescView(desc);
                    nativeAdView.setAdIconView(adIconView);
                    nativeAdView.setCallToActionView(btn);
                    nativeAdView.setMediaView(mediaView);
                    NativeAd.registerNativeAdView(placementId, nativeAdView, info);
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(
                            RelativeLayout.LayoutParams.WRAP_CONTENT, RelativeLayout.LayoutParams.WRAP_CONTENT);
                    adContainer.addView(nativeAdView, layoutParams);
                    Log.e(OpenMediationSDK.Tag,"Other");
                }

                ImpressionVideoData.onSendCallBack("Native", ImpressionVideoData.NativeJsonInfo, true);
            }

            @Override
            public void onNativeAdLoadFailed(String s, Error error) {
                ImpressionVideoData.onSendCallBack("Native", new JSONObject(), false);
            }

            @Override
            public void onNativeAdImpression(String s, AdInfo adInfo) { }

            @Override
            public void onNativeAdClicked(String s, AdInfo adInfo) {
                Log.e(OpenMediationSDK.Tag,"原生广告清理完毕");
                adContainer.removeAllViews();
            }
        };
        NativeAd.addAdListener(placementId, listener);
    }
}

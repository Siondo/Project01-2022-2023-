package com.jiujiuji.CrushTask;

import com.common.manager.Manager;
import com.common.sdk.OpenMediationSDK;
import com.common.sdk.SDKManager;
import com.common.sdk.cTenjinSDK;
import com.jiujiuji.CrushTask.openmediation.BannerVideo;
import com.jiujiuji.CrushTask.openmediation.NativeVideo;
import com.jiujiuji.CrushTask.openmediation.SplashVideo;
import com.openmediation.sdk.nativead.NativeAdView;
import com.unity3d.player.*;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.Window;
import android.widget.RelativeLayout;

public class MainActivity extends Activity
{
    //TAG
    private static String TAG = "JJJ_MainActivity";

    protected UnityPlayer mUnityPlayer; // don't change the name of this variable; referenced from native code

    //SharedPreferencesHelper sharedPreferencesHelper;
    // Override this in your custom UnityPlayerActivity to tweak the command line arguments passed to the Unity Android Player
    // The command line arguments are passed as a string, separated by spaces
    // UnityPlayerActivity calls this from 'onCreate'
    // Supported: -force-gles20, -force-gles30, -force-gles31, -force-gles31aep, -force-gles32, -force-gles, -force-vulkan
    // See https://docs.unity3d.com/Manual/CommandLineArguments.html
    // @param cmdLine the current command line arguments, may be null
    // @return the modified command line string or null
    protected String updateUnityCommandLineArguments(String cmdLine)
    {
        return cmdLine;
    }

    // Setup activity layout
    @Override protected void onCreate(Bundle savedInstanceState)
    {
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        super.onCreate(savedInstanceState);

        String cmdLine = updateUnityCommandLineArguments(getIntent().getStringExtra("unity"));
        getIntent().putExtra("unity", cmdLine);

        mUnityPlayer = new UnityPlayer(this);
        setContentView(mUnityPlayer);
        mUnityPlayer.requestFocus();

        //??????????????????
        onCreateBannerView(); //??????????????????View??????
        onCreateNativeView(); //??????????????????View??????
        OnCreateSplashView(); //??????????????????

        //???????????????
        Manager.getInstance().onCreate(this);
    }

    public void onCreateBannerView()
    {
        BannerVideo.adParent = new RelativeLayout(getApplicationContext());
        RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(
                RelativeLayout.LayoutParams.MATCH_PARENT, RelativeLayout.LayoutParams.MATCH_PARENT);
        layoutParams.addRule(RelativeLayout.CENTER_IN_PARENT);
        this.getWindow().addContentView(BannerVideo.adParent, layoutParams);
    }

    public void onCreateNativeView()
    {
        NativeVideo.mainAcitive = this;
        NativeVideo.adContainer = new RelativeLayout(getApplicationContext());
        RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(
                RelativeLayout.LayoutParams.MATCH_PARENT, RelativeLayout.LayoutParams.MATCH_PARENT);
        layoutParams.addRule(RelativeLayout.CENTER_IN_PARENT);
        this.getWindow().addContentView(NativeVideo.adContainer, layoutParams);
    }

    public void OnCreateSplashView()
    {
        SplashVideo.container = new RelativeLayout(getApplicationContext());
        RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(
                RelativeLayout.LayoutParams.MATCH_PARENT, RelativeLayout.LayoutParams.MATCH_PARENT);
        layoutParams.addRule(RelativeLayout.CENTER_IN_PARENT);
        this.getWindow().addContentView(SplashVideo.container, layoutParams);
    }

    /**
     * ????????????
     * @param msg
     */
    public void RequestPermission(String msg)
    {
        Manager.getInstance().requestPermission(msg);
    }

    /**
     * ????????????
     * @param msg
     */
    public void MobOnEventObj(String msg)
    {
        Manager.getInstance().onEventObject(msg);
    }

    //Tenjin???????????????
    public void onTouchTenjinEvent(String msg)
    {
        Log.i(cTenjinSDK.Tag, msg);
        Manager.getInstance().onReceiveTenjinsEvent(msg);
    }

    /**
     * ????????????
     * @param msg
     */
    public void SetInfo(String msg)
    {
        Manager.getInstance().SetInfo(msg);
    }

    /**
     * ??????
     * @param msg
     */
    public void OnLogin(String msg)
    {
        Manager.getInstance().onLogin(msg);
    }

    /**
     * ????????????
     * @param msg
     */
    public void OnLoginSuccess(String msg)
    {
        Manager.getInstance().onLoginSuccess(msg);
    }

    /**
     * ??????
     */
    public void OnExit()
    {
        Manager.getInstance().onExit();
    }

    /**
     * ??????????????????
     */
    public void RewardVideoAdShow(String scenario)
    {
        Manager.getInstance().onRewardVideoAdShow(scenario);
    }

    /**
     * ??????????????????
     */
    public void InterstitialAdShow(String scenario)
    {
        Manager.getInstance().onInterstitialAdShow(scenario);
    }

    /**
     * ??????????????????
     */
    public void BannerAdShow(String scenario)
    {
        Manager.getInstance().onBannerAdShow(scenario);
    }

    /**
     * ??????????????????
     */
    public void BannerAdHide()
    {
        Manager.getInstance().BannerAdHide();
    }

    /**
     * ??????????????????
     */
    public void SplashAdShow()
    {
        Manager.getInstance().onSplashAdShow();
    }

    /**
     * ??????????????????
     */
    public void NativeAdShow()
    {
        Manager.getInstance().OnNativeAdShow();
    }

    /**
     * ??????????????????
     * @param apkInfo
     */
    public void InstallAPK(String apkInfo)
    {
        Manager.getInstance().InstallAPK(apkInfo);
    }

    @Override protected void onNewIntent(Intent intent)
    {
        // To support deep linking, we need to make sure that the client can get access to
        // the last sent intent. The clients access this through a JNI api that allows them
        // to get the intent set on launch. To update that after launch we have to manually
        // replace the intent with the one caught here.
        setIntent(intent);
    }

    // Quit Unity
    @Override protected void onDestroy ()
    {
        //??????
        Manager.getInstance().onExit();
        mUnityPlayer.destroy();
        super.onDestroy();
    }

    // Pause Unity
    @Override protected void onPause()
    {
        //??????
        Manager.getInstance().onPause();
        super.onPause();
        mUnityPlayer.pause();
    }

    // Resume Unity
    @Override protected void onResume()
    {
        //??????
        Manager.getInstance().onResume();
        super.onResume();
        mUnityPlayer.resume();
    }

    @Override protected void onStart()
    {
        super.onStart();
        mUnityPlayer.start();
    }

    @Override protected void onStop()
    {
        super.onStop();
        mUnityPlayer.stop();
    }

    // Low Memory Unity
    @Override public void onLowMemory()
    {
        super.onLowMemory();
        mUnityPlayer.lowMemory();
    }

    // Trim Memory Unity
    @Override public void onTrimMemory(int level)
    {
        super.onTrimMemory(level);
        if (level == TRIM_MEMORY_RUNNING_CRITICAL)
        {
            mUnityPlayer.lowMemory();
        }
    }

    // This ensures the layout will be correct.
    @Override public void onConfigurationChanged(Configuration newConfig)
    {
        super.onConfigurationChanged(newConfig);
        mUnityPlayer.configurationChanged(newConfig);
    }

    // Notify Unity of the focus change.
    @Override public void onWindowFocusChanged(boolean hasFocus)
    {
        super.onWindowFocusChanged(hasFocus);
        mUnityPlayer.windowFocusChanged(hasFocus);
    }

    // For some reason the multiple keyevent type is not supported by the ndk.
    // Force event injection by overriding dispatchKeyEvent().
    @Override public boolean dispatchKeyEvent(KeyEvent event)
    {
        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent(event);
        return super.dispatchKeyEvent(event);
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    @Override public boolean onKeyUp(int keyCode, KeyEvent event)     { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onKeyDown(int keyCode, KeyEvent event)   { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onTouchEvent(MotionEvent event)          { return mUnityPlayer.injectEvent(event); }
    /*API12*/ public boolean onGenericMotionEvent(MotionEvent event)  { return mUnityPlayer.injectEvent(event); }
}

package com.common.util;

import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.util.Log;

import androidx.annotation.*;
import androidx.appcompat.app.AppCompatActivity;

import com.common.sdk.BuglySDK;

import org.json.JSONObject;

public class InstallActivity extends AppCompatActivity {

    private InstallUtil mInstallUtil;
    private static String TAG = "JJJ_InstallActivity";

    private AppInstallReceiver mDynamicReceiver = null;

    private static boolean mInstallStart = false;
    private static String mIdentifier = "";

    /**
     * 安装是否开始
     * @return
     */
    public static boolean isInstallStart()
    {
        return mInstallStart;
    }

    /**
     * 安装结束
     */
    public static void InstallEnd()
    {
        mInstallStart = false;
    }

    /**
     * 安装的包名
     * @return
     */
    public static String getIdentifier()
    {
        return mIdentifier;
    }

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        String apkInfo = getIntent().getStringExtra("apkInfo");
        mInstallStart = true;
        String filePath = "";
        try {
            JSONObject json = new JSONObject(apkInfo);
            filePath = json.getString("filePath");
            mIdentifier = json.getString("identifier");

        } catch (Exception e) {
            BuglySDK.postCatchedException(e);
            Log.e(TAG, e.toString());
        }

        //实例化IntentFilter对象
        IntentFilter filter = new IntentFilter();
        filter.addAction(Intent.ACTION_PACKAGE_ADDED);
        filter.addAction(Intent.ACTION_PACKAGE_CHANGED);
        filter.addAction(Intent.ACTION_PACKAGE_REPLACED);
        //filter.addAction(Intent.ACTION_PACKAGE_REMOVED);
        filter.addDataScheme("package");
        mDynamicReceiver = new AppInstallReceiver();
        registerReceiver(mDynamicReceiver, filter);
        Log.i(TAG, "onCreate   " + mInstallStart);

        mInstallUtil = new InstallUtil(this, filePath);
        mInstallUtil.install();
    }

    @Override
    protected void onResume() {
        super.onResume();
        Log.i(TAG, "onResume   " + mInstallStart);
        if (InstallUtil.unknownInstallprmission && mInstallUtil != null) {
            InstallUtil.unknownInstallprmission = false;
            mInstallUtil.install();
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        Log.i(TAG, "onDestroy   " + mInstallStart);
        unregisterReceiver(mDynamicReceiver);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        Log.i(TAG, "onActivityResult   " + requestCode + "  " + resultCode + "  " + data.getDataString());
        if (resultCode == RESULT_OK && requestCode == InstallUtil.UNKNOWN_CODE) {
            mInstallUtil.install();//再次执行安装流程，包含权限判等
        }
    }
}

package com.common.util;

import android.app.Activity;
import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Build;
import android.util.Log;

import com.common.manager.Manager;
import com.common.sdk.BuglySDK;
import com.jiujiuji.CrushTask.MainActivity;

public class AppInstallReceiver extends BroadcastReceiver
{
    private static String TAG = "JJJ_AppInstallReceiver";

    @Override
    public void onReceive(Context context, Intent intent)
    {
        String msg = intent.getData().toString();
        String action = intent.getAction();
        Log.i(TAG, action + "   " + msg);

        if (!InstallActivity.isInstallStart()) {
            return;
        }
        InstallActivity.InstallEnd();

        boolean reStart = false;
        if (Intent.ACTION_PACKAGE_ADDED.equals(action) || Intent.ACTION_PACKAGE_CHANGED.equals(action) || Intent.ACTION_PACKAGE_REPLACED.equals(action))
        {
            reStart = true;
        } else if (Intent.ACTION_PACKAGE_REMOVED.equals(action))
        {
        }

        if (reStart) {
            String identifier = InstallActivity.getIdentifier();
            if (identifier.length() == 0) {
                identifier = context.getPackageName();
            }
            Log.i(TAG, "identifier == " + identifier);
            if (msg.contains(identifier)) {
                if (context.getPackageName().equals(identifier)) {
                    //重启
                    final Intent newIntent = context.getPackageManager().getLaunchIntentForPackage(identifier);
                    newIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                    newIntent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                    Manager.getActivity().startActivity(newIntent);
                    //杀掉以前进程
                    android.os.Process.killProcess(android.os.Process.myPid());
                } else {
                    //启动
                    if (isInstalled(context, identifier)) {
                        if (true) {
                            Intent newIntent = new Intent();
                            newIntent.setClassName(identifier, identifier + ".MainActivity");
                            Manager.getActivity().startActivity(newIntent);
                        } else {
                            Intent newIntent = new Intent();
                            ComponentName comp = new ComponentName(identifier, identifier + ".MainActivity");
                            newIntent.setComponent(comp);
                            newIntent.setAction("android.intent.action.MAIN");
                            newIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                            Manager.getActivity().startActivity(newIntent);
                        }
                        //杀掉以前进程
                        android.os.Process.killProcess(android.os.Process.myPid());
                    }
                }
            }
        }
    }

    private boolean isInstalled(Context context, String packageName) {
        try {
            context.getPackageManager().getPackageInfo(packageName, 0);
            return true;
        } catch (PackageManager.NameNotFoundException e) {
            BuglySDK.postCatchedException(e);
            Log.e(TAG, e.toString());
            return false;
        }
    }
}

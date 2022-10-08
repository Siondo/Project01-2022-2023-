package com.common.util;


import android.app.Activity;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.provider.Settings;
import android.util.Log;

import androidx.annotation.*;
import androidx.appcompat.app.AlertDialog;
import androidx.core.content.FileProvider;

import com.common.manager.Manager;

import java.io.File;

public class InstallUtil {
    private static String TAG = "JJJ_InstallUtil";
    private Activity mAct;
    private String mPath;//下载下来后文件的路径
    public static int UNKNOWN_CODE = 2018;
    public static boolean unknownInstallprmission = false;

    public InstallUtil(Activity mAct, String mPath) {
        this.mAct = mAct;
        this.mPath = mPath;
    }

    public void install(){
        Log.i(TAG, "install");
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) startInstallO();
        else if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) startInstallN();
        else startInstall();
    }

    /**
     * android1.x-6.x
     */
    private void startInstall() {
        Intent install = new Intent(Intent.ACTION_VIEW);
        install.setDataAndType(Uri.parse("file://" + mPath), "application/vnd.android.package-archive");
        install.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        mAct.startActivity(install);
        Log.i(TAG, "startInstall");
    }

    /**
     * android7.x
     */
    private void startInstallN() {
        //参数1 上下文, 参数2 在AndroidManifest中的android:authorities值, 参数3  共享的文件
        Uri apkUri = FileProvider.getUriForFile(mAct, Manager.getActivity().getPackageName() + ".provider", new File(mPath));
        Intent install = new Intent(Intent.ACTION_VIEW);
        install.addCategory(Intent.CATEGORY_DEFAULT);
        //由于没有在Activity环境下启动Activity,设置下面的标签
        install.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        //添加这一句表示对目标应用临时授权该Uri所代表的文件
        install.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
        install.setDataAndType(apkUri, "application/vnd.android.package-archive");
        mAct.startActivity(install);
        Log.i(TAG, "startInstallN");
    }

    /**
     * android8.x
     */
    @RequiresApi(api = Build.VERSION_CODES.O)
    private void startInstallO() {
        boolean isGranted = mAct.getPackageManager().canRequestPackageInstalls();
        if (isGranted) startInstallN();//安装应用的逻辑(写自己的就可以)
        else new AlertDialog.Builder(mAct)
                .setCancelable(false)
                .setTitle("安装应用需要打开未知来源权限，请去设置中开启权限")
                .setPositiveButton("确定", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface d, int w) {
                        Intent intent = new Intent(Settings.ACTION_MANAGE_UNKNOWN_APP_SOURCES);
                        mAct.startActivityForResult(intent, UNKNOWN_CODE);
                        unknownInstallprmission = true;
                    }
                }).show();
    }
}
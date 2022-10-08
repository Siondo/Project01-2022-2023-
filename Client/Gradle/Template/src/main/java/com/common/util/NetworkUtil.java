package com.common.util;

import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;

import com.common.sdk.BuglySDK;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLConnection;

import javax.net.ssl.HttpsURLConnection;

public class NetworkUtil
{
    private static String TAG = "JJJ_NetworkUtil";

    public static final int GET_TOKEN = 1;
    public static final int CHECK_TOKEN = 2;
    public static final int REFRESH_TOKEN = 3;
    public static final int GET_INFO = 4;

    public static void sendWxAPI(Handler handler, String url, int msgTag) {
        HttpsThread httpsThread = new HttpsThread(handler, url, msgTag);
        httpsThread.start();
    }

    static class HttpsThread extends Thread {

        private Handler handler;
        private String httpsUrl;
        private int msgTag;

        public HttpsThread(Handler handler, String url, int msgTag) {
            this.handler = handler;
            this.httpsUrl = url;
            this.msgTag = msgTag;
        }

        @Override
        public void run() {
            int resCode;
            InputStream in;
            String httpResult = null;
            try {
                URL url = new URL(httpsUrl);
                URLConnection urlConnection = url.openConnection();
                HttpsURLConnection httpsConn = (HttpsURLConnection) urlConnection;
                httpsConn.setAllowUserInteraction(false);
                httpsConn.setInstanceFollowRedirects(true);
                httpsConn.setRequestMethod("GET");
                httpsConn.connect();
                resCode = httpsConn.getResponseCode();

                if (resCode == HttpURLConnection.HTTP_OK) {
                    in = httpsConn.getInputStream();

                    BufferedReader reader = new BufferedReader(new InputStreamReader(
                            in, "iso-8859-1"), 8);
                    StringBuilder sb = new StringBuilder();
                    String line;
                    while ((line = reader.readLine()) != null) {
                        sb.append(line).append("\n");
                    }
                    in.close();
                    httpResult = sb.toString();
                    Log.i(TAG, String.format("httpsUrl: %s, httpResult: %s", httpsUrl,  httpResult));

                    Message msg = Message.obtain();
                    msg.what = msgTag;
                    Bundle data = new Bundle();
                    data.putString("result", httpResult);
                    msg.setData(data);
                    handler.sendMessage(msg);
                }
            } catch (Exception e) {
                BuglySDK.postCatchedException(e);
                Log.e(TAG, e.getMessage());
            }
        }
    }
}

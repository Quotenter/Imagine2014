﻿using SYNAPSE.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using SYNAPSE;
using Windows.System.Profile;
using Windows.Storage;



// 基本ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234237 を参照してください

namespace SYNAPSE
{
    /// <summary>
    /// 多くのアプリケーションに共通の特性を指定する基本ページ。
    /// </summary>
    public sealed partial class LogInPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        string id;
        Frame rootFrame;

        /// <summary>
        /// これは厳密に型指定されたビュー モデルに変更できます。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper は、ナビゲーションおよびプロセス継続時間管理を
        /// 支援するために、各ページで使用します。
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public LogInPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            rootFrame = Window.Current.Content as Frame;
        }

        /// <summary>
        /// このページには、移動中に渡されるコンテンツを設定します。前のセッションからページを
        /// 再作成する場合は、保存状態も指定されます。
        /// </summary>
        /// <param name="sender">
        /// イベントのソース (通常、<see cref="NavigationHelper"/>)>
        /// </param>
        /// <param name="e">このページが最初に要求されたときに
        /// <see cref="Frame.Navigate(Type, Object)"/> に渡されたナビゲーション パラメーターと、
        /// 前のセッションでこのページによって保存された状態の辞書を提供する
        /// セッション。ページに初めてアクセスするとき、状態は null になります。</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //デバイスIDを復元
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if(localSettings.Values.ContainsKey("did"))
            {
               id = localSettings.Values["did"].ToString();
            }
        }

        /// <summary>
        /// アプリケーションが中断される場合、またはページがナビゲーション キャッシュから破棄される場合、
        /// このページに関連付けられた状態を保存します。値は、
        /// <see cref="SuspensionManager.SessionState"/> のシリアル化の要件に準拠する必要があります。
        /// </summary>
        /// <param name="sender">イベントのソース (通常、<see cref="NavigationHelper"/>)</param>
        /// <param name="e">シリアル化可能な状態で作成される空のディクショナリを提供するイベント データ
        ///。</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper の登録

        /// このセクションに示したメソッドは、NavigationHelper がページの
        /// ナビゲーション メソッドに応答できるようにするためにのみ使用します。
        /// 
        /// ページ固有のロジックは、
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// および <see cref="GridCS.Common.NavigationHelper.SaveState"/> のイベント ハンドラーに配置する必要があります。
        /// LoadState メソッドでは、前のセッションで保存されたページの状態に加え、
        /// ナビゲーション パラメーターを使用できます。

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        async private void LogInOKButton_clik(object sender, RoutedEventArgs e)
        {
            if(username.Text == "")
            {
                var messageDialog = new MessageDialog("ユーザーネームが入力されていません", "エラー");
                await messageDialog.ShowAsync();
                return;
            }
            if(password.Password == "")
            {
                var messageDialog = new MessageDialog("パスワードが入力されていません", "エラー");
                await messageDialog.ShowAsync();
                return;
            }
            var Client = new HttpClient();
            HttpResponseMessage response;

            Uri ressorceAddress = new Uri("http://synapse-server.cloudapp.net/Set/LogIn.aspx");
            string str;

            /*
            //deviceIDファイルを開いてdidを取得
            StorageFolder storageFolder = KnownFolders.MusicLibrary;
            StorageFile storageFile = await storageFolder.GetFileAsync("deviceID.txt");
            string id = await FileIO.ReadTextAsync(storageFile);
            */

            //ハッシュ生成
            //SHA1のハッシュプロバイダーを取得

            var algorithm = HashAlgorithmProvider.OpenAlgorithm("SHA1");

            //暗号化前の文字列をバイナリ形式のバッファに変換する

            //IBuffer uid = CryptographicBuffer.ConvertStringToBinary(username.Text, BinaryStringEncoding.Utf8);
            IBuffer pass = CryptographicBuffer.ConvertStringToBinary(password.Password, BinaryStringEncoding.Utf8);
            //IBuffer did = CryptographicBuffer.ConvertStringToBinary(id, BinaryStringEncoding.Utf8);

            //バッファからハッシュ化されたデータを取得する

            //var hash_uid = algorithm.HashData(uid);
            var hash_pass = algorithm.HashData(pass);
            //var hash_did = algorithm.HashData(did);

            //ハッシュ化されたデータを16進数の文字列に変換

            //string uid_h = CryptographicBuffer.EncodeToHexString(hash_uid);
            string pass_h = CryptographicBuffer.EncodeToHexString(hash_pass);
            string did_h = null;
            //string did_h = CryptographicBuffer.EncodeToHexString(hash_did);
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if(localSettings.Values.ContainsKey("did_h"))
            {
                did_h = localSettings.Values["did_h"].ToString();
            }
            else
            {
                //デバイスIDの取得開始
                var token = HardwareIdentification.GetPackageSpecificToken(null);
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(token.Id);
                byte[] bytes = new byte[token.Id.Length];
                dataReader.ReadBytes(bytes);
                var id = BitConverter.ToString(bytes);
                IBuffer did = CryptographicBuffer.ConvertStringToBinary(id, BinaryStringEncoding.Utf8);
                var hash_did = algorithm.HashData(did);
                did_h = CryptographicBuffer.EncodeToHexString(hash_did);
                localSettings.Values["did_h"] = did_h;
            }
            
            //送信するデータの生成

            HttpFormUrlEncodedContent content = new HttpFormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("uid",username.Text),
                new KeyValuePair<string,string>("pass_h",pass_h),
                new KeyValuePair<string,string>("did_h",did_h),
            });
            
            try
            {
                //通信開始
                
                response = await Client.PostAsync(ressorceAddress, content);
                str = await response.Content.ReadAsStringAsync();
                result.Text = str;
            }
            catch
            {
                result.Text = "ログイン失敗\n";
                return;
            }

            //cookieの取得
            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            HttpCookieCollection cookieCollection = filter.CookieManager.GetCookies(ressorceAddress);

            foreach (HttpCookie cookie in cookieCollection)
            {
                result.Text += "--------------------\r\n";
                result.Text += "Name: " + cookie.Name + "\r\n";
                result.Text += "Domain: " + cookie.Domain + "\r\n";
                result.Text += "Path: " + cookie.Path + "\r\n";
                result.Text += "Value: " + cookie.Value + "\r\n";
                result.Text += "Expires: " + cookie.Expires + "\r\n";
                result.Text += "Secure: " + cookie.Secure + "\r\n";
                result.Text += "HttpOnly: " + cookie.HttpOnly + "\r\n";
                //valueｎ保存
                ApplicationDataContainer localSid = ApplicationData.Current.LocalSettings;
                ApplicationDataContainer localDomain = ApplicationData.Current.LocalSettings;
                localSid.Values["sid"] = cookie.Value;
                localDomain.Values["Domain"] = cookie.Domain;
            }
            rootFrame.Navigate(typeof(TimeLine));
        }

        private void GotoSingUpButton_clik(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(UserInformationPage));
        }
    }
}

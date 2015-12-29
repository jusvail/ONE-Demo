﻿using Demo;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Demo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class StartPage : Page
    {
        public StartPage()
        {
            this.InitializeComponent();
        }

        public static List<DayObject> DayObjectCollection;

        public static string uri = "http://wufazhuce.com/";

        public static string uriOneObject = "http://wufazhuce.com/one/";

        public static HttpClient httpClient;

        public static string x;

        private async static Task<string> GetOneString(string uri)
        {
            if (httpClient != null) httpClient.Dispose();
            httpClient = new HttpClient();
            x = await httpClient.GetStringAsync(new Uri(uri));
            return x;
        }

        public async static Task<string> GetDayReallyObjectString(string vol)
        {
            if (httpClient != null) httpClient.Dispose();
            httpClient = new HttpClient();
            return await httpClient.GetStringAsync(new Uri(uriOneObject + vol));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                var bar = StatusBar.GetForCurrentView();
                await bar.HideAsync();
                StartAnimation.Begin();
                var what = bar.ProgressIndicator;
                what.ProgressValue = 0;
                what.Text = "Many-多个";
            }
            catch (Exception) { }
            //必须要做的事情
            try
            {
                await GetOneString(uri);
                //DayObjectCollection = new List<DayObject>();
                DayObjectCollection = GetOne.GetOneTodayObjectList(x);
                GetOne.SavePicHere(DayObjectCollection[0].DayImagePath);
            }
            catch (Exception)
            {
                await new MessageDialog("oops！请检查您的网络连接，离线版本将在后续版本开发o((⊙﹏⊙))o.").ShowAsync();
                this.Frame.Navigate(typeof(Main), "404");
                return;
            }
            this.Frame.Navigate(typeof(StartMainPage), new MyNavigationEventArgs(x, DayObjectCollection));

        }
    }
}

public class MyNavigationEventArgs
{
    public MyNavigationEventArgs(string xx, List<DayObject> dayObjectCollection)
    {
        XX = xx;
        this.dayObjectCollection = dayObjectCollection;
    }
    public string XX { get; set; }

    public List<DayObject> dayObjectCollection { get; set; }
}

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinansUygulamasi.Model
{
    public static class DboAnahtar
    {
        public static string BaseUrl { get; set; } = "https://financeapp-bc18c-default-rtdb.firebaseio.com/";
        public static string AuthKey { get; set; } = "wbdJREBfrDyFPN6askIwzRv4C1WVtADWeYRGcLtp";
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.Graphics;

namespace FinansUygulamasi.Model
{
    public static class ImageHelper
    {
        public static Bitmap ReturnImgFromUrl(string url)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    byte[] bytes = client.DownloadData(url);
                    if (bytes != null && bytes.Length > 0)
                    {
                        return BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }
    }
}
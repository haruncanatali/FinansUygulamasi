using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using FinansUygulamasi.Activities;
using FinansUygulamasi.Adapters;
using FinansUygulamasi.Model;
using Newtonsoft.Json;
using Refractored.Controls;
using AppCompatButton = AndroidX.AppCompat.Widget.AppCompatButton;

namespace FinansUygulamasi
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        private User user = null;

        private TextView adSoyadTxtControl = null, lokasyonTxtControl = null, baslangicParasiTxtControl = null, bakiyeTxtControl = null;
        private _BaseCircleImageView profilImgControl = null;
        private ImageView ayarlarImgControl = null;
        private ListView islemlerLvControl = null;
        private AppCompatButton yeniIslemBtnControl = null, tumIslemlerBtnControl = null;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            user = JsonConvert.DeserializeObject<User>(Intent.GetStringExtra("User"));

            if (user == null)
            {
                Toast.MakeText(this,"Giriş başarısız. Tekrar giriş yapın.",ToastLength.Long).Show();
                StartActivity(typeof(Login));
            }

            SetContentView(Resource.Layout.activity_main);
            Tanimla();
        }

        private void Tanimla()
        {
            adSoyadTxtControl = FindViewById<TextView>(Resource.Id.profileNameTxt);
            lokasyonTxtControl = FindViewById<TextView>(Resource.Id.sehirTxt);
            baslangicParasiTxtControl = FindViewById<TextView>(Resource.Id.baslangicParasiTxt);
            profilImgControl = FindViewById<_BaseCircleImageView>(Resource.Id.profilePhotoImg);
            ayarlarImgControl = FindViewById<ImageView>(Resource.Id.ayarlarImgBtn);
            bakiyeTxtControl = FindViewById<TextView>(Resource.Id.bakiyeTxt);
            islemlerLvControl = FindViewById<ListView>(Resource.Id.islemlerListView);
            yeniIslemBtnControl = FindViewById<AppCompatButton>(Resource.Id.yeniIslemBtn);
            tumIslemlerBtnControl = FindViewById<AppCompatButton>(Resource.Id.tumIslemlerBtn);
            islemlerLvControl = FindViewById<ListView>(Resource.Id.islemlerListView);

            if (user!=null)
            {
                profilImgControl.SetImageBitmap(ImageHelper.ReturnImgFromUrl(user.PhotoUrl));
                lokasyonTxtControl.Text = user.Location;
                baslangicParasiTxtControl.Text = user.StartMoney.ToString("#,##0.00") + " ₺ (Başlangıç Paranız)";
                adSoyadTxtControl.Text = user.Name + " " + user.Surname;
                bakiyeTxtControl.Text = user.Balance.ToString("#,##0.00") + " ₺";

                if (user.IslemGecmisi.Count>0)
                {
                    TransactionAdapter adapter = new TransactionAdapter(this, user.IslemGecmisi.Count>3 ?user.IslemGecmisi.OrderByDescending(c => c.Date).ToList().Take(3).ToList() as List<IslemGecmisi> : user.IslemGecmisi.OrderByDescending(c=>c.Date).ToList() as List<IslemGecmisi>);
                    islemlerLvControl.Adapter = adapter;
                }
                else
                {
                    TransactionAdapter adapter = new TransactionAdapter(this, new List<IslemGecmisi>());
                    islemlerLvControl.Adapter = adapter;
                }

            }
            else
            {
                Toast.MakeText(this,"Lütfen giriş yapın.",ToastLength.Short).Show();
                StartActivity(typeof(Login));
            }

            yeniIslemBtnControl.Click += YeniIslemBtnControl_Click;
            tumIslemlerBtnControl.Click += TumIslemlerBtnControl_Click;
            ayarlarImgControl.Click += AyarlarImgControl_Click;

        }

        private void AyarlarImgControl_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Ayarlar));
            intent.PutExtra("User", JsonConvert.SerializeObject(user));
            this.StartActivity(intent);
            this.Finish();
        }

        private void TumIslemlerBtnControl_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(History));
            intent.PutExtra("User", JsonConvert.SerializeObject(user));
            this.StartActivity(intent);
            this.Finish();
        }

        private void YeniIslemBtnControl_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(NewAct));
            intent.PutExtra("User", JsonConvert.SerializeObject(user));
            this.StartActivity(intent);
            this.Finish();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
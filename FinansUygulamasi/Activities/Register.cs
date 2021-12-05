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
using FinansUygulamasi.Model;
using Google.Android.Material.Button;
using Newtonsoft.Json;

namespace FinansUygulamasi.Activities
{
    [Activity(Label = "Register")]
    public class Register : Activity
    {
        private User user = null;

        private EditText adEdxControl = null,
            soyadEdxControl = null,
            konumEdxControl = null,
            fotoLinkEdxControl = null,
            kullaniciAdiEdxControl = null,
            sifreEdxControl = null,
            bakiyeEdxControl = null;

        private MaterialButton kaydetBtnControl = null, iptalBtnControl = null;

        private FirebaseConnection connection = null;

        private ProgressDialog progress;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register);
            Tanimla();
        }

        private void Tanimla()
        {
            connection = new FirebaseConnection();
            connection.ConnectionTest();
            adEdxControl = FindViewById<EditText>(Resource.Id.adRegTxt);
            soyadEdxControl = FindViewById<EditText>(Resource.Id.soyadRegTxt);
            konumEdxControl = FindViewById<EditText>(Resource.Id.konumRegTxt);
            fotoLinkEdxControl = FindViewById<EditText>(Resource.Id.fotoLinkRegTxt);
            kullaniciAdiEdxControl = FindViewById<EditText>(Resource.Id.kAdiRegTxt);
            sifreEdxControl = FindViewById<EditText>(Resource.Id.sifreRegTxt);
            bakiyeEdxControl = FindViewById<EditText>(Resource.Id.bakiyeRegTxt);

            kaydetBtnControl = FindViewById<MaterialButton>(Resource.Id.kaydetRgBtn);
            iptalBtnControl = FindViewById<MaterialButton>(Resource.Id.iptalRgBtn);

            iptalBtnControl.Click += delegate
            {
                StartActivity(typeof(Login));
            };

            kaydetBtnControl.Click += KaydetBtnControl_Click;
        }

        private async void KaydetBtnControl_Click(object sender, EventArgs e)
        {
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Bilgileriniz kaydediliyor... Lütfen bekleyin...");
            progress.SetCancelable(false);
            progress.Show();
            var result = await connection.AddUser(new User
            {
                Id = Guid.NewGuid(),
                Balance = decimal.Parse(bakiyeEdxControl.Text.ToString()),
                StartMoney = decimal.Parse(bakiyeEdxControl.Text.ToString()),
                Username = kullaniciAdiEdxControl.Text,
                Password = sifreEdxControl.Text,
                Surname = soyadEdxControl.Text,
                Name = adEdxControl.Text,
                Location = konumEdxControl.Text,
                PhotoUrl = fotoLinkEdxControl.Text
            });

            if (result == null)
            {
                Toast.MakeText(this,"Hata! Tekrar deneyin.",ToastLength.Long).Show();
            }
            else
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("User", JsonConvert.SerializeObject(result));
                this.StartActivity(intent);
                this.Finish();
            }
            progress.Dismiss();
        }
    }
}
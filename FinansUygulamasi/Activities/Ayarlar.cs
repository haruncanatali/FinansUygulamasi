using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FinansUygulamasi.Model;
using Google.Android.Material.Button;
using Newtonsoft.Json;

namespace FinansUygulamasi.Activities
{
    [Activity(Label = "Ayarlar")]
    public class Ayarlar : Activity
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
            user = JsonConvert.DeserializeObject<User>(Intent.GetStringExtra("User"));
            SetContentView(Resource.Layout.ayarlar);
            Tanimla();
        }

        private void Tanimla()
        {
            connection = new FirebaseConnection();
            connection.ConnectionTest();
            adEdxControl = FindViewById<EditText>(Resource.Id.adAyarTxt);
            soyadEdxControl = FindViewById<EditText>(Resource.Id.soyadAyarTxt);
            konumEdxControl = FindViewById<EditText>(Resource.Id.konumAyarTxt);
            fotoLinkEdxControl = FindViewById<EditText>(Resource.Id.fotoLinkAyarTxt);
            kullaniciAdiEdxControl = FindViewById<EditText>(Resource.Id.kAdiAyarTxt);
            sifreEdxControl = FindViewById<EditText>(Resource.Id.sifreAyarTxt);
            bakiyeEdxControl = FindViewById<EditText>(Resource.Id.bakiyeAyarTxt);

            kaydetBtnControl = FindViewById<MaterialButton>(Resource.Id.kaydetAyarBtn);
            iptalBtnControl = FindViewById<MaterialButton>(Resource.Id.iptalAyarBtn);

            if (user != null)
            {
                adEdxControl.Text = user.Name;
                soyadEdxControl.Text = user.Surname;
                konumEdxControl.Text = user.Location;
                fotoLinkEdxControl.Text = user.PhotoUrl;
                kullaniciAdiEdxControl.Text = user.Username;
                sifreEdxControl.Text = user.Password;
                bakiyeEdxControl.Text = user.Balance.ToString("#,##0.00");
            }
            else
            {
                Toast.MakeText(this, "Hata!", ToastLength.Long).Show();
                StartActivity(typeof(Login));
            }

            kaydetBtnControl.Click += KaydetBtnControl_Click;
            iptalBtnControl.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("User", JsonConvert.SerializeObject(user));
                this.StartActivity(intent);
                this.Finish();
            };
        }

        private async void KaydetBtnControl_Click(object sender, EventArgs e)
        {
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Bilgileriniz kaydediliyor... Lütfen bekleyin...");
            progress.SetCancelable(false);
            progress.Show();

            user.Name = adEdxControl.Text;
            user.Surname = soyadEdxControl.Text;
            user.Location = konumEdxControl.Text;
            user.PhotoUrl = fotoLinkEdxControl.Text;
            user.Username = kullaniciAdiEdxControl.Text;
            user.Password = sifreEdxControl.Text;
            user.Balance = decimal.Parse(bakiyeEdxControl.Text.ToString());

            var result = await connection.Update(user);

            if (result)
            {
                var userResult = await connection.Login(new LoginModel
                {
                    Username = user.Username,
                    Password = user.Password
                });
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("User", JsonConvert.SerializeObject(user));
                this.StartActivity(intent);
                this.Finish();
            }
            else
            {
                Toast.MakeText(this,"Hata! Tekrar giriş yapın.",ToastLength.Long).Show();
                StartActivity(typeof(Login));
            }

            progress.Dismiss();
        }
    }
}
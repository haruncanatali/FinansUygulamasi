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
    [Activity(Label = "NewAct")]
    public class NewAct : Activity
    {
        private MaterialButton gonderBtnControl = null, iptalBtnControl = null, tahsilEtBtnControl = null;
        private EditText paraMiktarEdxControl = null, aciklamaEdxControl = null;
        private FirebaseConnection connection = null;
        ProgressDialog progress;
        private User user = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.new_act);
            user = JsonConvert.DeserializeObject<User>(Intent.GetStringExtra("User"));
            Tanimla();
        }

        private async void Tanimla()
        {
            connection = new FirebaseConnection();
            connection.ConnectionTest();

            gonderBtnControl = FindViewById<MaterialButton>(Resource.Id.gonderNABtn);
            iptalBtnControl = FindViewById<MaterialButton>(Resource.Id.iptalNABtn);
            tahsilEtBtnControl = FindViewById<MaterialButton>(Resource.Id.alNABtn);

            paraMiktarEdxControl = FindViewById<EditText>(Resource.Id.miktarNATxt);
            aciklamaEdxControl = FindViewById<EditText>(Resource.Id.aciklamaNATxt);

            iptalBtnControl.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("User", JsonConvert.SerializeObject(user));
                this.StartActivity(intent);
                this.Finish();
            };
            gonderBtnControl.Click += async delegate
            {
                progress = new Android.App.ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetMessage("İşlem yapılıyor... Lütfen bekleyin...");
                progress.SetCancelable(false);
                progress.Show();
                var result = await connection.AddTransaction(new IslemGecmisi
                {
                    Date = DateTime.Now,
                    Id = Guid.NewGuid(),
                    FlowDirection = "Negative",
                    Money = decimal.Parse(paraMiktarEdxControl.Text.ToString()),
                    Note = aciklamaEdxControl.Text,
                    UserId = user.Id 
                });

                if (result != null)
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("User", JsonConvert.SerializeObject(result));
                    this.StartActivity(intent);
                    this.Finish();
                }
                else
                {
                    Toast.MakeText(this,"İşlem yapılamadı. Lütfen sonra tekrar deneyiniz.",ToastLength.Short).Show();
                }
                progress.Dismiss();
            };
            tahsilEtBtnControl.Click += async delegate
            {
                progress = new Android.App.ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetMessage("İşlem yapılıyor... Lütfen bekleyin...");
                progress.SetCancelable(false);
                progress.Show();
                var result = await connection.AddTransaction(new IslemGecmisi
                {
                    Date = DateTime.Now,
                    Id = Guid.NewGuid(),
                    FlowDirection = "Positive",
                    Money = decimal.Parse(paraMiktarEdxControl.Text.ToString()),
                    Note = aciklamaEdxControl.Text,
                    UserId = user.Id
                });

                if (result !=null)
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("User", JsonConvert.SerializeObject(result));
                    this.StartActivity(intent);
                    this.Finish();
                }
                else
                {
                    Toast.MakeText(this, "İşlem yapılamadı. Lütfen sonra tekrar deneyiniz.", ToastLength.Short).Show();
                }
                progress.Dismiss();
            };
        }
    }
}
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
using FinansUygulamasi.Adapters;
using FinansUygulamasi.Model;
using Google.Android.Material.Button;
using Newtonsoft.Json;

namespace FinansUygulamasi.Activities
{
    [Activity(Label = "History")]
    public class History : Activity
    {
        private User user = null;
        private MaterialButton anasayfaBtnControl = null;
        private ListView listeLvControl = null;
        private List<IslemGecmisi> tListe = null;
        private FirebaseConnection connection = null;
        private ProgressDialog progress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.history);
            user = JsonConvert.DeserializeObject<User>(Intent.GetStringExtra("User"));
            Tanimla();
        }

        private async void Tanimla()
        {
            connection = new FirebaseConnection();
            connection.ConnectionTest();

            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Giriş yapılıyor... Lütfen bekleyin...");
            progress.SetCancelable(false);
            progress.Show();
            tListe = await connection.TransactionsFilter(user.Id.ToString());
            listeLvControl = FindViewById<ListView>(Resource.Id.listeHisLv);
            TransactionAdapter adapter = new TransactionAdapter(this, tListe);
            listeLvControl.Adapter = adapter;
            progress.Dismiss();

            anasayfaBtnControl = FindViewById<MaterialButton>(Resource.Id.anasayfaHisBtn);
            anasayfaBtnControl.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("User",JsonConvert.SerializeObject(user));
                this.StartActivity(intent);
                this.Finish();
            };

        }
    }
}
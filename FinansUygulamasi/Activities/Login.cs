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
using Android.Animation;
using Android.Support.Design.Button;
using FinansUygulamasi.Model;
using Newtonsoft.Json;

namespace FinansUygulamasi.Activities
{
    [Activity(Label = "Login", MainLauncher = true)]
    public class Login : Activity
    {
        private MaterialButton girisBtnControl = null;
        private EditText kullaniciAdiEdxControl = null, sifreEdxControl = null;
        private FirebaseConnection connection = null;
        private TextView uyeOlBtnControl = null;
        ProgressDialog progress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            Tanimla();
        }

        private void Tanimla()
        {
            girisBtnControl = FindViewById<MaterialButton>(Resource.Id.girisBtn);
            kullaniciAdiEdxControl = FindViewById<EditText>(Resource.Id.usernameTxt);
            sifreEdxControl = FindViewById<EditText>(Resource.Id.passwordTxt);
            uyeOlBtnControl = FindViewById<TextView>(Resource.Id.uyeOlTxtBtn);

            connection = new FirebaseConnection();
            connection.ConnectionTest();

            girisBtnControl.Click += GirisBtnControl_Click;
            uyeOlBtnControl.Click += delegate
            {
                StartActivity(typeof(Register));
            };
        }

        private async void GirisBtnControl_Click(object sender, EventArgs e)
        {
            if (kullaniciAdiEdxControl.Text.Length>0 && sifreEdxControl.Text.Length>0)
            {
                progress = new Android.App.ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetMessage("Giriş yapılıyor... Lütfen bekleyin...");
                progress.SetCancelable(false);
                progress.Show();

                var result = await connection.Login(new LoginModel
                {
                    Username = kullaniciAdiEdxControl.Text,
                    Password = sifreEdxControl.Text
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
                    Toast.MakeText(this, "Hatalı kullanıcı adı/şifre.", ToastLength.Long).Show();
                }
                progress.Dismiss();
            }
            else
            {
                Toast.MakeText(this,"Hatalı kullanıcı adı/şifre.",ToastLength.Long).Show();
            }
        }
    }
}
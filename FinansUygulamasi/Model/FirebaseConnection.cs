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
using System.Threading.Tasks;
using Android.Accounts;
using Android.Icu.Text;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;

namespace FinansUygulamasi.Model
{
    public class FirebaseConnection
    {
        private IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = DboAnahtar.AuthKey,
            BasePath = DboAnahtar.BaseUrl
        };

        private IFirebaseClient client = null;
        private SetResponse response = null;
        private FirebaseResponse response_ = null;
        private User user = null;
        private IslemGecmisi transaction = null;

        protected internal string ConnectionTest(string x = null)
        {
            client = new FirebaseClient(config);

            if (client == null)
            {
                return "Başarısız! Bağlantı sağlanamadı.";
            }

            return "Başarılı! Bağlantı sağlandı.";
        }

        protected internal async Task<User> Login(LoginModel model)
        {
            response_ = await client.GetAsync("Account");
            var result = response_.Body;
            var data = JsonConvert.DeserializeObject<Dictionary<string, User>>(result);

            foreach (var user_ in data)
            {
                user = user_.Value as User;
                if (user.Username == model.Username && user.Password == model.Password)
                {
                    response_ = await client.GetAsync("Transactions");
                    var result1 = response_.Body;
                    var data1 = JsonConvert.DeserializeObject<Dictionary<string, IslemGecmisi>>(result1);
                    foreach (var transaction_ in data1)
                    {
                        transaction = transaction_.Value as IslemGecmisi;
                        if (transaction.UserId == user.Id)
                        {
                            user.IslemGecmisi.Add(transaction);
                        }
                    }

                    return user;
                }
            }

            return null;
        }

        protected internal async Task<List<IslemGecmisi>> TransactionsFilter(string Id)
        {
            List<IslemGecmisi> liste = new List<IslemGecmisi>();
            response_ = await client.GetAsync("Transactions");
            var result1 = response_.Body;
            var data1 = JsonConvert.DeserializeObject<Dictionary<string, IslemGecmisi>>(result1);
            foreach (var transaction_ in data1)
            {
                transaction = transaction_.Value as IslemGecmisi;
                if (transaction.UserId.ToString() == Id)
                {
                    liste.Add(transaction);
                }
            }

            return liste;
        }

        protected internal async Task<bool> Update(User model)
        {
            model.IslemGecmisi = null;
            var response = await client.UpdateAsync("Account/" + model.Id, model);
            var result = response.ResultAs<User>();

            if (result == null)
            {
                return false;
            }

            return true;
        }

        protected internal async Task<User> AddUser(User model)
        {
            response = await client.SetAsync("Account/" + model.Id + "/", model);
            var result = response.ResultAs<User>();
            return result;
        }

        protected internal async Task<User> AddTransaction(IslemGecmisi model)
        {
            response = await client.SetAsync("Transactions/" + model.Id + "/", model);
            var result = response.ResultAs<IslemGecmisi>();

            if (result != null)
            {
                response_ = await client.GetAsync("Account");
                var result1 = response_.Body;
                var data = JsonConvert.DeserializeObject<Dictionary<string, User>>(result1);

                foreach (var item in data)
                {
                    user = item.Value as User;
                    if (user.Id == model.UserId)
                    {
                        if (model.FlowDirection == "Positive")
                        {
                            user.Balance += model.Money;
                        }
                        else if (model.FlowDirection == "Negative")
                        {
                            user.Balance -= model.Money;

                        }

                        user.IslemGecmisi = null;

                        var responsex = await client.UpdateAsync("Account/" + user.Id, user);

                        if (responsex.StatusCode == HttpStatusCode.OK)
                        {
                            return await Login(new LoginModel
                            {
                                Username = user.Username,
                                Password = user.Password
                            });
                        }

                    }

                }

                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
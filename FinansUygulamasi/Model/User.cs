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
    public class User
    {
        public User()
        {
            IslemGecmisi = new List<IslemGecmisi>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Location { get; set; }
        public decimal Balance { get; set; }
        public decimal StartMoney { get; set; }
        public string PhotoUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public ICollection<IslemGecmisi> IslemGecmisi { get; set; }
    }
}
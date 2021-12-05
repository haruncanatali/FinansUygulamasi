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
    public class IslemGecmisi
    {
        public Guid Id { get; set; }
        public string FlowDirection { get; set; }
        public decimal Money { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
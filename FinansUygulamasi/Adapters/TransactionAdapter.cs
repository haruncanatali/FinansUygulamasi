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
using Android.Graphics;
using FinansUygulamasi.Model;

namespace FinansUygulamasi.Adapters
{
    public class TransactionAdapter : BaseAdapter<IslemGecmisi>
    {
        private Context context = null;
        private List<IslemGecmisi> transactionList = null;

        public TransactionAdapter(Context context, List<IslemGecmisi> transactionList)
        {
            this.context = context;
            this.transactionList = transactionList;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View? GetView(int position, View? convertView, ViewGroup? parent)
        {
            if (transactionList.Count>0)
            {
                View v = convertView;
                v = LayoutInflater.From(this.context).Inflate(Resource.Layout.transactions_satir, null, false);

                TextView aciklamaTxtTnsControl = v.FindViewById<TextView>(Resource.Id.aciklamaTnsTxt);
                TextView moneyTxtTnsControl = v.FindViewById<TextView>(Resource.Id.moneyTnsTxt);
                TextView dateTxtTnsControl = v.FindViewById<TextView>(Resource.Id.dateTnsTxt);

                aciklamaTxtTnsControl.Text = transactionList[position].Note;
                moneyTxtTnsControl.Text = transactionList[position].Money.ToString("#,##0.00") + " ₺";

                if (transactionList[position].FlowDirection == "Positive")
                {
                    moneyTxtTnsControl.SetTextColor(Color.Rgb(46, 204, 113));
                }
                else if (transactionList[position].FlowDirection == "Negative")
                {
                    moneyTxtTnsControl.SetTextColor(Color.Rgb(255, 0,0));
                }

                dateTxtTnsControl.Text = transactionList[position].Date.ToString();

                return v;
            }
            else if(transactionList == null)
            {
                View v_ = convertView;
                v_ = LayoutInflater.From(context).Inflate(Resource.Layout.empty_list, null, false);
                return v_;
            }
            else
            {
                return null;
            }
        }

        public override int Count
        {
            get { return transactionList.Count(); }
        }

        public override IslemGecmisi this[int position]
        {
            get { return transactionList[position]; }
        }
    }
}
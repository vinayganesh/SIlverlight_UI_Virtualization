using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ImportTradeFiles.Classes
{
    public class ITF
    {
        public string TRNO { get; set; }
        public string Account { get; set; }
        public string Quantity { get; set; }
        public string SecuritySymbol { get; set; }
        public string Price { get; set; }
        public string TimeLimit { get; set; }
        public string SpecialCondition { get; set; }
        public string DivRei { get; set; }
        public string BidPrice { get; set; }
        public string PriceTime { get; set; }
        public string EstimatedOrder { get; set; }
        public string MessageCode { get; set; }
        public string SwapToFunds { get; set; }
        public string TransFee { get; set; }
        public string AccountName { get; set; }
        public string NewMoney { get; set; }
    }
}
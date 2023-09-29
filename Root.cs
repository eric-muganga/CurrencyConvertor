using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CurrencyConvertor.MainWindow;

namespace CurrencyConvertor
{
    public class Root
    {
        public string license;
        public long timestamp;
        public Rate rates { get; set; }
    }
}

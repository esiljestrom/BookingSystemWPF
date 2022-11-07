using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3WPF
{
    public class Booking
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int TableNumber { get; set; }

        public Booking(string name, string date, string time, int tableNumber)
        {
            Name = name;
            Date = date;
            Time = time;
            TableNumber = tableNumber;
        }
    }
}

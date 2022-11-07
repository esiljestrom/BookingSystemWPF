using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb3WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var startBokningar = new List<Booking>() // De bokningar som alltid ska finnas med
            { 
                new Booking("Per", "2022-11-5", "18:00", 3),
                new Booking("Ulla", "2022-11-7", "17:00", 5),
                new Booking("Charles", "2022-11-13", "20:00", 2)
            };
            foreach(var booking in startBokningar)
            {
                if(!FileHandler.IsBooked(booking)) // om inte startBokningar finns så bokar programmet dom
                    FileHandler.SaveBooking(booking, false);
            }
        }

        private void Boka_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Sparar all input i tydligare variabler
                string name = txtNamn.Text;
                string date = dtpDatum.SelectedDate.Value.ToShortDateString();
                string time = cbxTid.Text;
                int tablenr = Convert.ToInt32(cbxBordsnummer.Text);

                // '#' används för att separera i textdokumentet så namnet får inte ha den symbolen i sig
                // kontrollerar även så att input inte är tom på alla fälten där fel kan uppstå
                if(!name.Contains('#') && name != "" && time != "")
                {
                    var booking = new Booking(name, date, time, tablenr); // Skapar ett boknings-objekt

                    FileHandler.SaveBooking(booking, true); // Sparar boknings-objektet i txt-filen med alla bokningar
                }
                else
                {
                    MessageBox.Show("Felaktig inmatning. Försök igen!");
                }
            }
            catch
            {
                MessageBox.Show("Felaktig inmatning. Försök igen!");
            }
        }

        private void VisaBokningar_Click(object sender, RoutedEventArgs e) // Hämtar alla bokningar från txt-filen och skriver ut dom
        {
            lbxBokningar.Items.Clear();
            var bookings = new List<Booking>();
            bookings = FileHandler.LoadBookings();

            foreach(var booking in bookings)
            {
                lbxBokningar.Items.Add($"{booking.Date} kl. {booking.Time}   Bord #{booking.TableNumber}     {booking.Name}");
            }
        }

        private void Avboka_Click(object sender, RoutedEventArgs e)
        {
            if(lbxBokningar.SelectedIndex >= 0)
            {
            int selectedBooking = lbxBokningar.SelectedIndex;
            FileHandler.RemoveBooking(selectedBooking);
            VisaBokningar_Click(null, null); // Uppdaterar listan efter en avbokning
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace Labb3WPF
{
    public class FileHandler
    {
        private static string Path { get; } = "bokningar.txt";


        public static List<Booking> LoadBookings() // Returnerar en lista med alla bokningar
        {
            var bookings = new List<Booking>(); // Skapar en lista där alla bokningar sparas

            string[] properties;

            using(StreamReader sr = new StreamReader(Path))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    properties = line.Split('#');

                    var booking = new Booking(properties[0], properties[1], properties[2], Convert.ToInt32(properties[3]));
                    bookings.Add(booking);
                }
            }
            bookings = SortBookings(bookings);
            return bookings;
        }

        public static void SaveBooking(Booking booking, bool bookingMessage) // Skriver ner en bokning i textfilen
        { 
            if (!IsBooked(booking)) // Om bokningen inte finns så sparas den, men om det redan finns en det datumet så bokas det
            {
                using (StreamWriter sw = new StreamWriter(Path, true))
                {
                    sw.WriteLine($"{booking.Name}#{booking.Date}#{booking.Time}#{booking.TableNumber}");
                }
                if(bookingMessage == true)
                    MessageBox.Show("Bord är nu bokat i namnet: " + booking.Name);
            }
            else
                MessageBox.Show("Den bokningen är upptagen.");
        }

        public static void RemoveBooking(int bookingIndex) // Skriver om textfilen utan den valda bokningen(bookingIndex)
        {
            // Skapar en lista med alla existerande bokningar
            // Tar bort den valda bokningen (bookingIndex) i listan 
            // Skriver sedan över textfilen med nya listan med bokningar
            var bookings = new List<Booking>();
            bookings = LoadBookings();
            bookings.Remove(bookings[bookingIndex]);

            OverwriteAllBookings(bookings);
        }

        private static void OverwriteAllBookings(List<Booking> bookings) // Skriver över alla bokningar med ny lista
        {
            using (StreamWriter streamwriter = new StreamWriter(Path, false)) // false för då skriver den över alla gamla bokningar
            {
                for (int i = 0; i < bookings.Count; i++)
                {
                    streamwriter.WriteLine($"{bookings[i].Name}#{bookings[i].Date}#{bookings[i].Time}#{bookings[i].TableNumber}");
                }
            }
        }

        public static bool IsBooked(Booking booking) // Kontrollerar ifall det redan finns en sådan bokning
        {
            var bookings = new List<Booking>();
            bookings = LoadBookings(); // Hämtar listan med alla bokningar för att jämföra med

            var query = bookings
                .Where(x => x.Date == booking.Date)
                .Where(x => x.Time == booking.Time)
                .Where(x => x.TableNumber == booking.TableNumber)
                .Select(x => x);

            if (query.Count() > 0) // Om det finns en eller flera likadana bokningar returneras True
                return true;
            else
                return false;
        }

        private static List<Booking> SortBookings(List<Booking> bookings) // Sorterar alla bokningar efter deras datum med LINQ
        {
            return bookings.OrderBy(x => x.Date).ToList();
        }
    }

}

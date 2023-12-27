using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp
{
    class Program
    {
        // Ganti dengan API key Anda dari WeatherAPI
        private const string API_KEY = "masukkan API anda";

        // Buat objek HttpClient untuk mengirim permintaan ke API
        private static readonly HttpClient client = new HttpClient();

        // Buat metode async untuk mendapatkan data cuaca dari API
        private static async Task GetWeatherDataAsync(string location)
        {
            // Buat URL permintaan dengan lokasi dan API key
            string requestUrl = $"http://api.weatherapi.com/v1/current.json?key={API_KEY}&q={location}";

            // Kirim permintaan GET ke API dan dapatkan respons
            HttpResponseMessage response = await client.GetAsync(requestUrl);

            // Periksa apakah respons berhasil
            if (response.IsSuccessStatusCode)
            {
                // Baca isi respons sebagai string JSON
                string json = await response.Content.ReadAsStringAsync();

                // Ubah string JSON menjadi objek C# dengan Newtonsoft.Json
                WeatherData data = JsonConvert.DeserializeObject<WeatherData>(json);

                // Tampilkan data cuaca di konsol
                Console.WriteLine($"Lokasi: {data.location.name}, {data.location.country}");
                Console.WriteLine($"Waktu: {data.location.localtime}");
                Console.WriteLine($"Suhu: {data.current.temp_c} C");
                Console.WriteLine($"Kelembaban: {data.current.humidity} %");
                Console.WriteLine($"Angin: {data.current.wind_kph} km/jam");
                Console.WriteLine($"Kondisi: {data.current.condition.text}");
            }
            else
            {
                // Tampilkan pesan kesalahan jika respons gagal
                Console.WriteLine($"Permintaan gagal dengan status {response.StatusCode}");
            }
        }

        // Buat metode main untuk menjalankan program
        static void Main(string[] args)
        {
            // Baca input lokasi dari pengguna
            Console.Write("Masukkan lokasi untuk melacak cuaca misalnya (jakarta): ");
            string location = Console.ReadLine();

            // Panggil metode async untuk mendapatkan data cuaca
            GetWeatherDataAsync(location).Wait();

            // Tunggu input dari pengguna untuk keluar
            Console.WriteLine("Tekan enter untuk keluar");
            Console.ReadLine();
        }
    }

    // Buat kelas untuk menyimpan data cuaca dari API
    public class WeatherData
    {
        public Location location { get; set; }
        public Current current { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string country { get; set; }
        public string localtime { get; set; }
    }

    public class Current
    {
        public double temp_c { get; set; }
        public int humidity { get; set; }
        public double wind_kph { get; set; }
        public Condition condition { get; set; }
    }

    public class Condition
    {
        public string text { get; set; }
    }
}

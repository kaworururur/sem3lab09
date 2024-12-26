using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace lab9_2
{
    public partial class Form1 : Form
    {
        private string File_Path = "C:/Users/stepa/Documents/code/lab9/lab9_2/city.txt";

        public Form1()
        {
            InitializeComponent();
            LoadCities();
        }

        private void LoadCities()
        {
            try
            {
                string[] lines = File.ReadAllLines(File_Path);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 1)
                    {
                        listBox1.Items.Add(parts[0].Trim()); // Добавляем только названия городов
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка городов: {ex.Message}");
            }
        }

        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите город из списка.");
                return;
            }

            string selectedCity = listBox1.SelectedItem.ToString();

            try
            {
                Weather currentWeather = await GetWeatherByCity(selectedCity);
                DisplayInfo(currentWeather);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения погоды: {ex.Message}");
            }
        }

        private async Task<Weather> GetWeatherByCity(string cityName)
        {
            string apiKey = "b8b395f38e3311a84b4e5f34eae8ccd4"; //my
            //string apiKey = "542a3f18aadff0a091857dbe1416ec83";

            // Поиск координат города
            double[] coordinates = GetCityCoordinates(cityName);

            if (coordinates == null || coordinates.Length != 2)
                throw new Exception("Координаты города не найдены или неверный формат данных.");

            double latitude = coordinates[0];
            double longitude = coordinates[1];

            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var dataString = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(dataString);
                    return new Weather()
                    {
                        Country = (string)data["sys"]["country"],
                        Name = (string)data["name"],
                        Temp = (double)data["main"]["temp"],
                        Description = (string)data["weather"][0]["description"]
                    };
                }
                else
                {
                    throw new Exception("Ошибка при запросе API.");
                }
            }
        }

        private double[] GetCityCoordinates(string cityName)
        {
            try
            {
                string[] lines = File.ReadAllLines(File_Path);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3 && parts[0].Trim().Equals(cityName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Пытаемся преобразовать широту и долготу
                        if (double.TryParse(parts[1].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double latitude) &&
                            double.TryParse(parts[2].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double longitude))
                        {
                            return new double[] { latitude, longitude };
                        }
                        else
                        {
                            throw new FormatException($"Координаты для города '{cityName}' имеют неверный формат: {parts[1]}, {parts[2]}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения координат: {ex.Message}");
            }

            // Если координаты не найдены
            throw new Exception($"Координаты для города '{cityName}' не найдены.");
        }

        private void DisplayInfo(Weather weather)
        {
            MessageBox.Show($"Город: {weather.Name}, {weather.Country}\n" +
                            $"Температура: {weather.Temp:F1}°C\n" +
                            $"Описание: {weather.Description}",
                            "Прогноз погоды", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public struct Weather
        {
            public string Country { get; set; }
            public string Name { get; set; }
            public double Temp { get; set; }
            public string Description { get; set; }
        }
    }
}
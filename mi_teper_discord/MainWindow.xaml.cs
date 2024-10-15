using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace mi_teper_discord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Discord1135Context context;

        HttpClient httpClient = new HttpClient();
        private DispatcherTimer timer = null;

        public Chat Chat { get; set; }
        public List<Chat> Chats { get; set; } = new List<Chat>();

        public MainWindow()
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri("http://localhost:5132/api/");
            DataContext = this;
        }

        public void timerStart()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) //к таймеру относится 
        {
            Thread thread = new Thread(GetMessage);
            thread.Start();
        }

        public async void GetMessage()
        {

            var responce = await httpClient.PostAsync($"Messages/GetMessages", context);          
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                return;
            }
            else
            {
                var answer = await responce.Content.ReadFromJsonAsync<Search>();
                MessageBox.Show($"Нашелся человек по паспорту - {answer.ToString()}");
                Close();
            }
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {

        }
    }
}
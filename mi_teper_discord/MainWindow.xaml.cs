using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Discord1135Context context;

        HttpClient httpClient = new HttpClient();
        private DispatcherTimer timer = null;
 

        public event PropertyChangedEventHandler? PropertyChanged;

        public Chat Chat { get; set; } = new();
        private List<Chat> chats;
        public List<Chat> Chats 
        { 
            get=> chats;
            set 
            {
                chats = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Chats)));
            } }

        public MainWindow()
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri("http://localhost:5132/api/");
            DataContext = this;
            timerStart();
            
        }

        public void timerStart()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) //к таймеру относится 
        {
            Thread thread = new Thread(GetMessage);
            thread.Start();
        }

        public async void GetMessage()
        {

            var responce = await httpClient.PostAsync($"Messages/GetMessages", new StringContent("", Encoding.UTF8, "application/json"));          
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                MessageBox.Show("все плохо!");
                return;
            }
            else
            {
                var messages = await responce.Content.ReadFromJsonAsync<List<Chat>>();
                Chats = new List<Chat>(messages);
                

            }
        }

        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            string arg = JsonSerializer.Serialize(Chat);
            var responce = await httpClient.PostAsync($"Messages/SendMessages",
                new StringContent(arg, Encoding.UTF8, "application/json"));

            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                return;
            }
            else
            {
                MessageBox.Show("Сообщение отправлено!");
                
            }

        }

        private void EnterIsPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage(null, null);
            }
        }
    }
}
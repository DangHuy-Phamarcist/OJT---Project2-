using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetAllIPs_Click(object sender, RoutedEventArgs e)
        {
            lstIPs.Items.Clear();
            string hostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            foreach (IPAddress ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    lstIPs.Items.Add(ip.ToString());
                }
            }
        }

        private async void TestCamera_Click(object sender, RoutedEventArgs e)
        {
            string cameraIP = txtCameraIP.Text;

            if (string.IsNullOrWhiteSpace(cameraIP))
            {
                txtStatus.Text = "Please enter a valid IP address.";
                return;
            }

            bool isReachable = await TestCameraAsync(cameraIP);

            txtStatus.Text = isReachable ? "Camera is reachable!" : "Camera is not reachable.";
        }

        private Task<bool> TestCameraAsync(string ip)
        {
            return Task.Run(() =>
            {
                try
                {
                    Ping ping = new Ping();
                    PingReply reply = ping.Send(ip, 1000); // Timeout of 1000ms (1 second)

                    return reply.Status == IPStatus.Success;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}

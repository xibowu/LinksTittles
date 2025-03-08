using HttpLinkTittles.Client;
using System;
using System.Collections.Generic;
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

namespace HttpLinkTittles
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //this.Title = "HttpLinkTittles";
            return;

        }

        HttpLinkTittles.Client.HttpClientHeader HttpClientHeader = new HttpLinkTittles.Client.HttpClientHeader();
        public async void Button_Click(object sender, RoutedEventArgs e)
        {

            txtStatus.Text = "doing";
            string urls = txtInputLinks.Text;
            string result = await HttpClientHeader.HttpClientReques(urls);

            txtOutputLinks.Clear();
            txtOutputLinks.Text = result;
            txtStatus.Text = "done";

            return;
        }
    }
}

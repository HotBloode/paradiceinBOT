using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace paradiceinBOT
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {


            if (cb1.SelectedIndex == -1)
            {
                MessageBox.Show("Не тупи и выбери валюту");
            }
            else
            {
                ClassBet bet1 = new ClassBet(tb1, Convert.ToDouble(textBet1.Text), textBet1.Text.Replace(",", "."), cb1.Text);
                (sender as Button).IsEnabled = false;


                Thread ThreadForParser = new Thread(new ThreadStart(bet1.Start));
                ThreadForParser.Start();
            }
        }

        private void TextBet1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }

    }
}
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
using RestSharp;

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

        private void f(int flag)
        {
            Controller  contrl = new Controller(tb1,
                Convert.ToDouble(textBet1.Text),
                textBet1.Text.Replace(",", "."),
                flag,
                cb1.Text,
                Convert.ToDouble(tbChance.Text), Convert.ToDouble(tbMulty.Text));
            contrl.Start();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {           

         if (cb1.SelectedIndex == -1)
         {
             MessageBox.Show("Не тупи и выбери валюту");
         }
         else
         {
             string checkedValue = sp.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.Value).Content.ToString();

             int a = 0;
                 if (checkedValue == "ABOVE")
                 {
                     f(1);
                 }
                 else if (checkedValue == "BELOW")
                 {
                     f(2);
                 }
                 else
                 {
                     f(3);
                 }

                 (sender as Button).IsEnabled = false;

         }
        }

        private void TextFormat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }
        
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            tbChance.IsEnabled = true;
            tbMulty.IsEnabled = false;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            tbChance.IsEnabled = false;
            tbMulty.IsEnabled = true;
        }
        
        private void TbChance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if ((sender as TextBox).IsEnabled)
                {
                    if (Convert.ToDouble((sender as TextBox).Text)<2 || Convert.ToDouble((sender as TextBox).Text)>98)
                    {
                        MessageBox.Show("Шанс должен быть в пределах [2;98]");
                        (sender as TextBox).Text = "";
                    }
                    else
                    {
                        double s = 99.0 / Convert.ToDouble((sender as TextBox).Text);
                        tbMulty.Text = Convert.ToString(s);
                    }
                }
            }
        }

        private void TbMulty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Convert.ToDouble((sender as TextBox).Text) < 1.01 || Convert.ToDouble((sender as TextBox).Text) > 95.5)
                {
                    MessageBox.Show("The multiplier shall be within [1,01;49,5]");
                    (sender as TextBox).Text = "";
                }
                else
                {
                    if ((sender as TextBox).IsEnabled)
                    {
                        double s = 99.0 / Convert.ToDouble((sender as TextBox).Text);
                        tbChance.Text = Convert.ToString(s);
                    }
                }
            }
        }

        private void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            if (multipliedByWin != null)
            {
                multipliedByWin.IsEnabled = true;
            }
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            if (multipliedByWin != null)
            {
                multipliedByWin.IsEnabled = false;
            }
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            if (multipliedByLose != null)
            {
                multipliedByLose.IsEnabled = false;
            }
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            if (multipliedByLose != null)
            {
                multipliedByLose.IsEnabled = true;
            }
        }

        private void Multiplied(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void MultipliedByWin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Convert.ToDouble((sender as TextBox).Text) <= 1.01 || Convert.ToDouble((sender as TextBox).Text) > 49.5)
                {
                    MessageBox.Show("The multiplier shall be within [1,01;49,5]");
                    (sender as TextBox).Text = "2";
                }
            }
        }
    }
}
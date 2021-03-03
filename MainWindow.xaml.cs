﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace paradiceinBOT
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int flagSide;
        private bool flagMultyOnWin;
        private bool flagMultyOnLose;
        private Controller controller;
       
        public MainWindow()
        {
            InitializeComponent();
            Microsoft.Win32.SystemEvents.SessionSwitch += OnIn;
            Microsoft.Win32.SystemEvents.SessionEnding += OnOut;
        }

        void OnIn(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            controller.OnIn();

        }

        void OnOut(object sender, Microsoft.Win32.SessionEndingEventArgs e)
        {
            controller.OnOut();
        }

        private void AddInfAndStart()
        {

            controller = new Controller(tb1,
                Convert.ToDouble(textBet1.Text),
                textBet1.Text.Replace(",", "."),
                flagSide,
                cb1.Text,
                Convert.ToDouble(tbChance.Text),
                Convert.ToDouble(tbMulty.Text),
                flagMultyOnWin,
                Convert.ToDouble(multipliedByWin.Text),
                flagMultyOnLose,
                Convert.ToDouble(multipliedByLose.Text),
                rbBets.IsChecked.Value,
                rbMins.IsChecked.Value,
                Convert.ToInt32(mins.Text),
                Convert.ToInt32(bets.Text)
                );

            controller.Start();
        }

        private void SiarchFlagSide()
        {
            string checkedValue = sp.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.Value).Content.ToString();
            if (checkedValue == "ABOVE")
            {
                flagSide = 1;
            }
            else if (checkedValue == "BELOW")
            {
                flagSide = 2;
            }
            else
            {
                flagSide = 3;
            }
        }

        private void SiarchFlagMultOnWin()
        {
            string checkedValue = onWin.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.Value).Content.ToString();
            if (checkedValue == "multiplied by")
            {
                flagMultyOnWin = true;
            }
            else
            {
                flagMultyOnWin = false;
            }
        }

        private void SiarchFlagMultOnLose()
        {
            string checkedValue = onLose.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.Value).Content.ToString();
            if (checkedValue == "multiplied by")
            {
                flagMultyOnLose = true;
            }
            else
            {
                flagMultyOnLose = false;
            }
        }

        #region CheckFunk

        private bool CheckChance()
        {
            if (tbChance == null || tbChance.Text == "" || Convert.ToDouble(tbChance.Text) < 2 || Convert.ToDouble(tbChance.Text) > 98)
            {
                MessageBox.Show("Chance must be within [2;98]");
                tbChance.Text = "45";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckmMultiply()
        {
            if (tbMulty == null || tbMulty.Text == ""||Convert.ToDouble(tbMulty.Text) < 1.01 || Convert.ToDouble(tbMulty.Text) > 95.5)
            {
                MessageBox.Show("The multiplier shall be within [1,01;49,5]");
                tbMulty.Text = "2,2";
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool CheckMultOnWinLose(TextBox box)
        {
            if (box == null || box.Text == ""||Convert.ToDouble(box.Text) <= 1.01 || Convert.ToDouble(box.Text) > 49.5)
            {
                MessageBox.Show("The multiplier shall be within [1,01;50]");
               box.Text = "2";
               return false;

            }
            else
            {
                return true;
            }
        }

        private bool CheckMins(TextBox box)
        {
            if (box == null || box.Text == "" || Convert.ToDouble(box.Text) < 5 || Convert.ToDouble(box.Text) > 24)
            {
                MessageBox.Show("The mins shall be within [5;3600]");
                box.Text = "10";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckBets(TextBox box)
        {
            if (box == null || box.Text == "" || Convert.ToDouble(box.Text) < 100 || Convert.ToDouble(box.Text) > 100000)
            {
                MessageBox.Show("The number of bets shall be within [100;100000]");
                box.Text = "100000";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckBet()
        {
            if (textBet1 == null || textBet1.Text == "")
            {
                MessageBox.Show("The bet line must be filled!");
                textBet1.Text = "0,00000001";
                return false;

            }
            else
            {
                return true;
            }
        }
        #endregion

        private List<double> deserializedProduct = new List<double>();
        
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (cb1.SelectedIndex == -1)
            {
                MessageBox.Show("Не тупи и выбери валюту");
            }
            else
            {
                if (CheckChance() && CheckmMultiply() && CheckMultOnWinLose(multipliedByWin) && CheckMultOnWinLose(multipliedByLose) && CheckBet()&&CheckMins(mins) && CheckBets(bets))
                {
                    SiarchFlagSide();
                    SiarchFlagMultOnLose();
                    SiarchFlagMultOnWin();
                    AddInfAndStart();

                    (sender as Button).IsEnabled = false;
                    btStop.IsEnabled = true;
                    SpumStatistic.IsEnabled = true;
                }
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

        #region KeyDown
        private void TbChance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                if ((sender as TextBox).IsEnabled)
                {
                    if (CheckChance())
                    {
                        double s = 99.0 / Convert.ToDouble((sender as TextBox).Text);
                        tbMulty.Text = Convert.ToString(s);
                    }
                }
            }
        }

        private void TbMulty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                if (CheckmMultiply())
                {
                    if ((sender as TextBox).IsEnabled)
                    {
                        double s = 99.0 / Convert.ToDouble((sender as TextBox).Text);
                        tbChance.Text = Convert.ToString(s);
                    }
                }
            }
        }

        private void MultipliedByWin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                CheckMultOnWinLose(sender as TextBox);
            }
        }

        private void Mins_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                CheckMins(mins);
            }
        }

        private void Bets_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                CheckBets(bets);
            }
        }

        private void TextBet1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                CheckBet();
            }
        }
        #endregion

        #region multipliedByWinSettings
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
                multipliedByWin.Text = "2";
                multipliedByWin.IsEnabled = false;
            }
        }
        #endregion

        #region multipliedByLoseSettings
        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            if (multipliedByLose != null)
            {
                multipliedByLose.IsEnabled = false;
                multipliedByLose.Text = "2";
            }
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            if (multipliedByLose != null)
            {
                multipliedByLose.IsEnabled = true;
            }
        }
        #endregion

        #region SpamSettings
        private void RadioButton_Checked_6(object sender, RoutedEventArgs e)
        {
            if (mins != null || bets != null)
            {
                mins.IsEnabled = false;
                bets.IsEnabled = false;
                bets.Text = "10000";
                mins.Text = "10";
            }
        }

        private void RadioButton_Checked_7(object sender, RoutedEventArgs e)
        {
            if (mins != null || bets != null)
            {
                mins.IsEnabled = true;
                bets.IsEnabled = false;
                bets.Text = "10000";
            }
        }

        private void RadioButton_Checked_8(object sender, RoutedEventArgs e)
        {
            if (mins != null || bets != null)
            {
                mins.IsEnabled = false;
                mins.Text = "10";
                bets.IsEnabled = true;
            }
        }
        #endregion

        private void SpumStatistic_Click(object sender, RoutedEventArgs e)
        {
            controller.OnceSpam();
        }

        private void BtStop_Click(object sender, RoutedEventArgs e)
        {
            controller.Stop();
            btStop.IsEnabled = false;
            SpumStatistic.IsEnabled = false;
            bt1.IsEnabled = true;
        }

    }
}
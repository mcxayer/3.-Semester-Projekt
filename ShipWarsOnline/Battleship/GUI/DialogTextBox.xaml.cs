﻿using System;
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
using System.Windows.Shapes;

namespace Battleship.GUI
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogTextBox : Window
    {
        public DialogTextBox()
        {
            InitializeComponent();
        }

        public string ResponseValue { get { return tbValue.Text; } set { tbValue.Text = value; } }

        public string Information { get { return lblInfo.Content as string; } set { lblInfo.Content = value; } }

        private void OnOkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
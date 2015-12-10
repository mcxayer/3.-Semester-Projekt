﻿using Battleship.Game;
using Battleship.GUI;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginWindow : UserControl
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void OnBackButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.GotoMainMenu();
        }

        private void OnLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                GUIFacade.Instance.Login(tbUsername.Text, tbPassword.Password);
                GUIFacade.Instance.Connect();
            }
            catch (FaultException ex)
            {
                lblInfo.Content = ex.Message;
                return;
            }
        }

        private void OnCreateUserButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.GotoAccountCreation();
        }
    }
}

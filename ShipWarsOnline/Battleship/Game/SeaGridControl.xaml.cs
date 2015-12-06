﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Battleship.Game;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Battleship.Game
{
    /// <summary>
    /// Interaction logic for ComputerGrid.xaml
    /// </summary>
    public partial class SeaGrid : UserControl
    {
        public List<List<SeaSquare>> GridCells
        {
            get { return GameContextFacade.Instance.; }
        }

        public SeaGrid()
        {
            InitializeComponent();
        }

        private void OnCellClicked(object sender, MouseButtonEventArgs e)
        {
            //Grid vm = this.DataContext as Grid;
            //ListBoxItem item = sender as ListBoxItem;
            //SeaSquare content = item.Content as SeaSquare;

            ////XXX sometimes if you click really fast you can end up clicking on what the debugger says is a "ListBoxItem {DisconnectedItem}
            ////hunting down the exact cause would take ages, and might even be a bug in WPF or something
            //if (content == null)
            //    return;

            //vm.Clicked(content);            

            GameContextFacade.Instance.TakeTurn(0, 0);
        }
    }
}

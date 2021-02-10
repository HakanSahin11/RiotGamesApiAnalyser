using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Lol_Decay_Reminder.MainWindow;
using Lol_Decay_Reminder.Models;

namespace Lol_Decay_Reminder
{
    public partial class AddNew : Window 
    {
        MainWindow _mainWindow;
        public AddNew(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            List<string> ListOfRegions = new List<string> { "EUW", "EUNE", "NA", "BR", "LAN", "LAS", "OCE", "RU", "TR", "JP", "KR" };
            ListOfRegions.ForEach(x => cbAddNewRegion.Items.Add(x));
            cbAddNewRegion.SelectedIndex = 0;
        }

        private void btnAddConfirm_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.BtnAddNewConfirmed(new SavedUsersModel(txtAddNew.Text, cbAddNewRegion.Text));
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

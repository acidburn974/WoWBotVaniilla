using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace WoWBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Tableau des processus wow
        /// </summary>
        Process[] _wowProcesses;

        /// <summary>
        /// Rafraichit les données
        /// </summary>
        private Thread _refreshDataThread;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Rafraichit la liste des processus WoW disponible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            _wowProcesses = Process.GetProcessesByName("Wow");
            foreach (Process t in _wowProcesses)
            {
                processesComboBox.Items.Add(t.Id);
            }
        }


        /// <summary>
        /// Attache le bot au processus demandé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void attachButton_Click(object sender, RoutedEventArgs e)
        {
            if (processesComboBox.SelectedValue != null)
            {
                try
                {
                    int processId = Int16.Parse(processesComboBox.SelectedValue.ToString());
                    Common.Memory.AttachToProcess(processId);
                    RefreshUiData();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Lance le thread qui rafraichit l'UI
        /// </summary>
        private void RefreshUiData()
        {
            _refreshDataThread = new Thread(RefreshUiDataWork);
            _refreshDataThread.Start();
        }

        /// <summary>
        /// Rafraichit les données
        /// </summary>
        private void RefreshUiDataWork()
        {
            // IsAttached boolean
            if (IsAttachedLabel.Dispatcher.CheckAccess())
            {
                IsAttachedLabel.Content = Common.Memory.IsAttached ? "Is Attached: Yes" : "Is Attached: No";
            }
            else
            {
                IsAttachedLabel.Dispatcher.Invoke(delegate () { IsAttachedLabel.Content = Common.Memory.IsAttached ? "Is Attached: Yes" : "Is Attached: No"; });
            }
            // Adresse de l'ObjManager
            if (InGameObjectManagerAddress.CheckAccess())
            {
                InGameObjectManagerAddress.Content = "Object Manager: 0x" + Manager.ObjectManager.InGameObjectManager.ToString("X");
            }
            else
            {
                InGameObjectManagerAddress.Dispatcher.Invoke(delegate() { InGameObjectManagerAddress.Content = "Object Manager: 0x" + Manager.ObjectManager.InGameObjectManager.ToString("X"); });
            }
            // Player GUID
            if (PlayerGuidLabel.CheckAccess())
            {
                PlayerGuidLabel.Content = "Player GUID: " + Manager.ObjectManager.LocalGUID;
            }
            else
            {
                PlayerGuidLabel.Dispatcher.Invoke(delegate () { PlayerGuidLabel.Content = "Player GUID: " + Manager.ObjectManager.LocalGUID; });
            }
            // First Object Location
            if (FirstObjectLocationLabel.CheckAccess())
            {
                FirstObjectLocationLabel.Content = "First Object Location: " + Manager.ObjectManager.FirstObject;
            }
            else
            {
                FirstObjectLocationLabel.Dispatcher.Invoke(delegate () { FirstObjectLocationLabel.Content = "First Object Location: " + Manager.ObjectManager.FirstObject; });
            }


            Thread.Sleep(250);
        }

        private void fishButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

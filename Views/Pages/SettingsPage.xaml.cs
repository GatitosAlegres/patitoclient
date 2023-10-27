using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using System.IO.Ports;
using System;
using PatitoClient.Lib;

namespace PatitoClient.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INavigableView<ViewModels.SettingsViewModel>
    {
        public ViewModels.SettingsViewModel ViewModel { get; }

        public SettingsPage(ViewModels.SettingsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var remoteServerIp = TextBox_RemoteServerIP.Text.Trim();
            var port = Constants.SERVER_PORT;
            var nickName = TextBox_Nickname.Text.Trim();
            
            try
            {
                if (nickName is "")
                    throw new ArgumentException("Nickname can't be empty");
                
                ViewModel.OnConnect(
                remoteServerIp: remoteServerIp,
                port: port,
                nickName: nickName
                );

                SnackBar_OnPortSettingsSave.Show(
                    title: "Connection",
                    message: $"Connection successful with server {remoteServerIp}:{port}",
                    SymbolRegular.Save24,
                    ControlAppearance.Success);
            }catch(Exception ex)
            {
                SnackBar_OnPortSettingsSave.Show(
                     title: "Connection",
                     message: ex.Message,
                     SymbolRegular.ErrorCircle24,
                     ControlAppearance.Danger);
            }
        }
    }
}
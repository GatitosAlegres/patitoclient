using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Wpf.Ui.Common.Interfaces;

namespace PatitoClient.ViewModels
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private IPAddress _serverIp;
        private IPEndPoint _serverEndPoint;

        [ObservableProperty]
        private Wpf.Ui.Appearance.ThemeType _currentTheme = Wpf.Ui.Appearance.ThemeType.Unknown;

        private ChatViewModel _chatViewModel;

        public SettingsViewModel(ChatViewModel chatViewModel)
        {
            _chatViewModel = chatViewModel;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
            
            _isInitialized = true;
        }

       public void OnConnect(
           string remoteServerIp,
           int port,
           string nickName)
        {
            var serverIpLiteral = remoteServerIp;
        
            _serverIp = IPAddress.Parse(serverIpLiteral);

            _serverEndPoint = new IPEndPoint(_serverIp, port);

            _chatViewModel._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
            _chatViewModel._clientSocket.Connect(_serverEndPoint);

            _chatViewModel.SuccessfulConnect(nickName);
        } 

        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Light)
                        break;

                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
                    CurrentTheme = Wpf.Ui.Appearance.ThemeType.Light;

                    break;

                default:
                    if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Dark)
                        break;

                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
                    CurrentTheme = Wpf.Ui.Appearance.ThemeType.Dark;

                    break;
            }
        }
    }
}

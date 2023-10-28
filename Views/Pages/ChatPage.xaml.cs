using PatitoClient.Framework.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Newtonsoft.Json;
using PatitoClient.Core.Domain;
using Wpf.Ui.Common.Interfaces;

namespace PatitoClient.Views.Pages
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : INavigableView<ViewModels.ChatViewModel>
    {
        private StackPanel previousPanel;

        public ViewModels.ChatViewModel ViewModel
        {
            get;
        }

        public ChatPage(ViewModels.ChatViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            ViewModel.ReceivedTextMessageEvent += OnReceivedTextMessage;
            ViewModel.SendTextMessageEvent += OnSendTextMessage;
            ViewModel.ClientsListOnlineEvent += OnClientsListOnlineEvent;
            
            ViewModel.Error += OnError;
        }

        private void OnClientsListOnlineEvent(object oo, List<Client> clients)
        {

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ListViewUsers.Items.Clear();
                
                clients.ForEach(client =>
                {
                    var textBlock = new TextBlock
                    {
                        Text = client.Nickname
                    };
                
                    var listViewItem = new ListViewItem
                    {
                        Content = textBlock
                    };

                    ListViewUsers.Items.Add(listViewItem);
                });
            });
        }

        private void OnError(object oo, string data)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                MessageUI.create(
                    data,
                    HorizontalAlignment.Center,
                    System.Windows.Media.Brushes.DarkRed, (StackPanel panel) => {

                        StackPanel_ChatLayout.Children.Add(panel);
                    });
            });
        }

        private void OnReceivedTextMessage(object o, Message message)
        {

            Application.Current.Dispatcher.Invoke((Action)delegate {
                MessageUI.create(
                    message.Body,
                    HorizontalAlignment.Left,
                    System.Windows.Media.Brushes.DarkBlue, (StackPanel panel) => {
                        
                        StackPanel_ChatLayout.Children.Add(panel);
                    });
            }); 
            ScrollViewer.ScrollToBottom();
        }

        private void OnSendTextMessage(object o, Message message)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                MessageUI.create(
                 message.Body,
                 HorizontalAlignment.Right,
                 System.Windows.Media.Brushes.DarkGreen, (StackPanel panel) => {

                     TextBox_InputMessage.Clear();
                     StackPanel_ChatLayout.Children.Add(panel);
                 });
            });
            ScrollViewer.ScrollToBottom();
        }
        
        private void TextBox_InputMessage_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ViewModel.OnInputChanged(TextBox_InputMessage.Text);
        }

        private void ListViewUsers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewUsers.SelectedIndex == -1 ) return;

            ViewModel.listViewSelectedIndex = ListViewUsers.SelectedIndex;
            
            var listViewItem = ListViewUsers.SelectedItem as ListViewItem;
            var textBlock = listViewItem?.Content as TextBlock;
            var nickName = textBlock?.Text;
            
            if (nickName is null or "") return;
            
            StackPanel_ChatLayout.Children.Clear();
            
            ViewModel.SetClientReceiver(nickName);

            LabelClientReceiver.Text = nickName;
        }
    }
}
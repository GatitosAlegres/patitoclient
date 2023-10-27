using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace PatitoClient.Framework.Components;

public class MessageUI
{
    public static void create(string phrase,
        HorizontalAlignment alignment, 
        SolidColorBrush colorBrush, 
        Action<StackPanel> callback)
    {
        StackPanel messageContainer = new StackPanel();
        messageContainer.Margin = new Thickness(0, 20, 0, 0);

        Border borderContainer = new Border();
        borderContainer.HorizontalAlignment = alignment;
        borderContainer.Margin = new Thickness(5);
        borderContainer.BorderThickness = new Thickness(1);
        borderContainer.BorderBrush = Brushes.Transparent;
        borderContainer.Background = colorBrush;
        borderContainer.CornerRadius = new CornerRadius(10);

        TextBlock messageTextBlock = new TextBlock();
        messageTextBlock.Text = phrase;
        messageTextBlock.FontSize = 14;
        messageTextBlock.Padding = new Thickness(10);
        messageTextBlock.TextWrapping = TextWrapping.Wrap;

        borderContainer.Child = messageTextBlock;
        messageContainer.Children.Add(borderContainer);

        callback(messageContainer);
    }
}
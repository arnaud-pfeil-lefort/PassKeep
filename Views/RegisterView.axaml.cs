using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PassKeep.ClassesGenerales;
using PassKeep.modeles;
using System;
using System.Linq;

namespace PassKeep.Views;

public partial class RegisterView : Window
{
    public RegisterView()
    {
        InitializeComponent();
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        => BeginMoveDrag(e);

    private void MinimizeWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void CloseWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => Close();


    private void OnLoginClicked(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var app = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var mainWindow = new LoginView();
        app.MainWindow = mainWindow;
        mainWindow.Show();
        Close();
    }

    private void OnRegisterClicked(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
            string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
            string.IsNullOrWhiteSpace(NomTextBox.Text))
        {
            ErrorTextBlock.Text = "Tous les champs sont obligatoires.";
            ErrorTextBlock.IsVisible = true;
            return;
        }

        using DataContext db = new DataContext();

        bool emailExiste = db.PKUser.Any(u => u.Email == EmailTextBox.Text);
        if (emailExiste)
        {
            ErrorTextBlock.Text = "Cet email est déjà utilisé.";
            ErrorTextBlock.IsVisible = true;
            return;
        }

        var user = new PKUser
        {
            Id = Guid.NewGuid(),
            Nom = NomTextBox.Text,
            Email = EmailTextBox.Text,
            Login = EmailTextBox.Text,
            Password = ClassFonctionsGenerales.hacherChaine(PasswordTextBox.Text)
        };

        db.PKUser.Add(user);
        db.SaveChanges();

        var app = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var mainWindow = new MainWindow(user);
        app.MainWindow = mainWindow;
        mainWindow.Show();

        Close();
    }

}
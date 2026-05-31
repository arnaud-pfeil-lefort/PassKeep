using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace PassKeep.Views;

public enum HelpPage
{
    Login,
    Register,
    Main,
    AddProfil,
    Settings,
    TypeProfil
}

public partial class HelpView : Window
{
    public HelpView(HelpPage page)
    {
        InitializeComponent();
        BuildContent(page);
    }

    private void BuildContent(HelpPage page)
    {
        switch (page)
        {
            case HelpPage.Login:
                HelpTitle.Text = "Aide - Connexion";
                AddSection("Se connecter",
                    "Saisis ton adresse e-mail et ton mot de passe, puis clique sur « Se connecter ».");
                AddBullets("Étapes :", new[]
                {
                    "Entre ton adresse e-mail dans le premier champ.",
                    "Entre ton mot de passe dans le second champ.",
                    "Coche « Rester connecté » pour ne pas devoir te reconnecter à chaque fois.",
                    "Clique sur « Se connecter »."
                });
                AddSection("En cas d'erreur",
                    "Le message « Email ou mot de passe incorrect » s'affiche si les informations sont mauvaises. Vérifie la casse (majuscules/minuscules) et l'adresse e-mail utilisée.");
                AddSection("Pas encore de compte ?",
                    "Clique sur « S'inscrire » en bas de la fenêtre pour créer ton coffre-fort.");
                break;

            case HelpPage.Register:
                HelpTitle.Text = "Aide - Inscription";
                AddSection("Créer un compte",
                    "Remplis tous les champs pour créer ton coffre-fort personnel PassKeep.");
                AddBullets("Champs requis :", new[]
                {
                    "Nom complet : ton prénom et nom.",
                    "Adresse e-mail : doit être unique, tu t'en serviras pour te connecter.",
                    "Mot de passe : choisis un mot de passe fort.",
                    "Confirmer le mot de passe : doit être identique au mot de passe saisi."
                });
                AddSection("Règles de validation",
                    "Tous les champs sont obligatoires. L'e-mail doit être unique : impossible de créer deux comptes avec la même adresse.");
                AddSection("Après l'inscription",
                    "Tu es automatiquement redirigé vers l'écran de connexion. Utilise tes identifiants pour accéder à ton coffre-fort.");
                break;

            case HelpPage.Main:
                HelpTitle.Text = "Aide - Tableau de bord";
                AddSection("Vue d'ensemble",
                    "Le tableau de bord liste tous tes profils de connexion (Google, GitHub, banque…). Chaque entrée affiche le service et le login associés.");
                AddBullets("Actions disponibles :", new[]
                {
                    "Afficher/masquer le mot de passe : clique sur l'icône oeil.",
                    "Modifier une entrée : clique sur l'icône crayon.",
                    "Supprimer une entrée : clique sur l'icône corbeille.",
                    "Ajouter un profil : bouton « + Ajouter un mot de passe » en bas."
                });
                AddSection("Recherche et filtres",
                    "Utilise la barre de recherche pour filtrer par nom de service. Le menu déroulant filtre par type de profil. Le bouton ↺ réinitialise les filtres.");
                AddSection("Paramètres et aide",
                    "L'icône engrenage ouvre les paramètres. L'icône ? ouvre cette aide.");
                break;

            case HelpPage.AddProfil:
                HelpTitle.Text = "Aide - Ajouter un profil";
                AddSection("Remplir le formulaire",
                    "Chaque profil correspond à un compte externe que tu veux sauvegarder.");
                AddBullets("Champs du formulaire :", new[]
                {
                    "Service : nom du site ou de l'application (ex : Google, GitHub).",
                    "URL (facultatif) : adresse web du service.",
                    "Type : catégorie du profil (ex : Réseaux sociaux, Banque).",
                    "Login : ton identifiant ou adresse e-mail pour ce service.",
                    "Mot de passe : le mot de passe associé à ce compte."
                });
                AddSection("Vérification d'URL",
                    "Clique sur « Vérifier » à côté du champ URL pour analyser si le site est sûr via Google Safe Browsing.");
                AddSection("Générer un mot de passe",
                    "Clique sur « Générer » pour créer automatiquement un mot de passe à partir des mots de ton dictionnaire (configurable dans les Paramètres).");
                break;

            case HelpPage.Settings:
                HelpTitle.Text = "Aide - Paramètres";
                AddSection("Apparence",
                    "Bascule entre le mode sombre et le mode clair en cliquant sur le bouton « Mode clair » ou « Mode sombre ».");
                AddSection("Types de profil",
                    "Clique sur « Gérer » pour ouvrir le gestionnaire de types. Tu peux y ajouter ou supprimer des catégories utilisées pour classer tes profils.");
                AddSection("Dictionnaire",
                    "Le dictionnaire sert à la génération automatique de mots de passe.");
                AddBullets("Actions :", new[]
                {
                    "Importer : charge un fichier .txt contenant une liste de mots (un mot par ligne).",
                    "Supprimer : vide entièrement le dictionnaire."
                });
                AddSection("Se déconnecter",
                    "Le bouton rouge « Se déconnecter » en bas ferme ta session et te ramène à l'écran de connexion.");
                break;

            case HelpPage.TypeProfil:
                HelpTitle.Text = "Aide - Types de profil";
                AddSection("À quoi ça sert ?",
                    "Les types de profil sont des catégories qui permettent de classer tes profils de connexion (ex : Réseaux sociaux, Banque, Email…).");
                AddSection("Ajouter un type",
                    "Saisis le nom du nouveau type dans le champ texte, puis clique sur « Ajouter ». Le nom doit être unique.");
                AddSection("Supprimer un type",
                    "Clique sur le bouton ✕ à droite du type que tu souhaites supprimer. Cette action est définitive.");
                AddSection("Bon à savoir",
                    "Les types sont ensuite disponibles dans le menu déroulant lors de l'ajout ou de la modification d'un profil.");
                break;
        }
    }

    private void AddSection(string title, string body)
    {
        var panel = new StackPanel { Spacing = 6 };

        var titleBlock = new TextBlock
        {
            Text = title,
            FontSize = 14,
            FontWeight = FontWeight.SemiBold,
            Foreground = new SolidColorBrush(Color.Parse("#e2e8f0")),
            TextWrapping = TextWrapping.Wrap
        };

        var bodyBlock = new TextBlock
        {
            Text = body,
            FontSize = 13,
            Foreground = new SolidColorBrush(Color.Parse("#94a3b8")),
            TextWrapping = TextWrapping.Wrap,
            LineHeight = 20
        };

        panel.Children.Add(titleBlock);
        panel.Children.Add(bodyBlock);

        var border = new Border
        {
            Padding = new Avalonia.Thickness(14, 12),
            CornerRadius = new Avalonia.CornerRadius(10),
            Child = panel
        };
        border.Classes.Add("Card");

        ContentArea.Children.Add(border);
    }

    private void AddBullets(string title, string[] items)
    {
        var panel = new StackPanel { Spacing = 6 };

        var titleBlock = new TextBlock
        {
            Text = title,
            FontSize = 14,
            FontWeight = FontWeight.SemiBold,
            Foreground = new SolidColorBrush(Color.Parse("#e2e8f0")),
            TextWrapping = TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(0, 0, 0, 4)
        };
        panel.Children.Add(titleBlock);

        foreach (var item in items)
        {
            var row = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8
            };

            var bullet = new TextBlock
            {
                Text = "•",
                FontSize = 13,
                Foreground = new SolidColorBrush(Color.Parse("#3b82f6")),
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Avalonia.Thickness(0, 2, 0, 0)
            };

            var text = new TextBlock
            {
                Text = item,
                FontSize = 13,
                Foreground = new SolidColorBrush(Color.Parse("#94a3b8")),
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20
            };

            row.Children.Add(bullet);
            row.Children.Add(text);
            panel.Children.Add(row);
        }

        var border = new Border
        {
            Padding = new Avalonia.Thickness(14, 12),
            CornerRadius = new Avalonia.CornerRadius(10),
            Child = panel
        };
        border.Classes.Add("Card");

        ContentArea.Children.Add(border);
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        => BeginMoveDrag(e);

    private void MinimizeWindow(object? sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void CloseWindow(object? sender, RoutedEventArgs e)
        => Close();
}

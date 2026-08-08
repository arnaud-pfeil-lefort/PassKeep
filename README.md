<div align="center">

<img src="PassKeep/assets/passkeepIcon.png" width="80"/>

# PassKeep

**Gestionnaire de mots de passe sécurisé - Windows & Linux**

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square)
![Avalonia](https://img.shields.io/badge/Avalonia-12.0-8B5CF6?style=flat-square)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=flat-square&logo=sqlite&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)

</div>

---

## Présentation

PassKeep est un projet conçu comme **tour d'horizon complet du développement logiciel moderne**, allant de la conception d'interface à la distribution d'un installeur natif.

L'application est un gestionnaire de mots de passe de bureau, entièrement local et sans serveur externe. Elle couvre un large spectre de concepts techniques :

- **Interface graphique cross-platform** avec Avalonia UI (XAML, thèmes dynamiques, styles personnalisés)
- **Architecture en couches** avec séparation UI / logique métier / accès données (DLL dédiée)
- **Base de données locale** SQLite gérée via **Entity Framework Core** et un système de **migrations** automatiques au démarrage
- **Chiffrement AES-256** des mots de passe stockés
- **Intégration d'API REST externe** (Google Safe Browsing) avec gestion de clé via fichier `.env`
- **Gestion de session persistante** (GUID de session stocké localement)
- **Packaging et distribution** - installeur Windows via Inno Setup, archive Linux, releases GitHub

## Fonctionnalités

- **Gestion de profils** - Ajout, modification et suppression de comptes (site, identifiant, mot de passe)
- **Génération de mots de passe** - Aléatoire ou basé sur un dictionnaire personnalisable
- **Vérification d'URL** - Contrôle des sites via Google Safe Browsing
- **Types de profil** - Classement des comptes par catégories personnalisables
- **Thème clair / sombre** - Interface adaptée à vos préférences
- **Connexion persistante** - Option "Rester connecté" au démarrage

## Comptes par défaut

Au premier lancement, deux comptes sont créés automatiquement via les migrations :

| Rôle | Email | Mot de passe |
|---|---|---|
| Administrateur | `admin@passkeep.com` | `admin` |
| Utilisateur | `user@passkeep.com` | `user` |

> Le compte **Admin** a accès à l'ensemble des profils de tous les utilisateurs. Le compte **User** ne voit que ses propres profils.

## Téléchargement

| Plateforme | Fichier |
|---|---|
| Windows | `PassKeepSetup.exe` |
| Linux x64 | `PassKeep-linux-x64.zip` |

Voir les [Releases](../../releases) pour télécharger la dernière version.

## Installation

### Windows

1. Télécharge `PassKeepSetup.exe`
2. Lance l'installeur et suis les étapes
3. PassKeep est prêt à l'emploi

### Linux

```bash
sudo apt install -y libx11-6 libxext6 libfontconfig1
unzip PassKeep-linux-x64.zip -d PassKeep
chmod +x PassKeep/PassKeep
./PassKeep/PassKeep
```

## Vérification Google Safe Browsing (optionnel)

Pour activer la vérification des URLs, crée un fichier `.env` dans le dossier d'installation :

```
GOOGLE_SAFE_BROWSING_API_KEY=ta_clé_api
```

> Une clé API gratuite est disponible sur [Google Cloud Console](https://console.cloud.google.com/).

## Stack technique

- **UI** - [Avalonia UI](https://avaloniaui.net/) 12.0
- **ORM** - Entity Framework Core 10 + SQLite
- **Chiffrement** - AES-256
- **Environnement** - DotNetEnv

## Développement

```bash
git clone https://github.com/arnaud-pfeil-lefort/PassKeep
cd passkeep
dotnet restore
dotnet run --project PassKeep
```
--

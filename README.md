# CineTrack

Projet d'application entreprise — Gestionnaire et découvreur d'animes.

### Versions

- Visual Studio 2022+
- .NET 8.0 SDK

---

## Structure du projet

```
CineTrack/
├── CineTrack/                                      ← Projet WPF (UI)
│   ├── App.xaml / App.xaml.cs                      ← Point d'entrée, injection de dépendances (DI)
│   ├── Converters/
│   │   ├── BoolToVisibilityConverter.cs            → bool → Visible/Collapsed
│   │   ├── BoolToVisibilityInverseConverter.cs     → bool inversé → Visible/Collapsed
│   │   ├── FavoriteColorConverter.cs               → bool favori → couleur bouton
│   │   ├── IntToVisibilityConverter.cs             → int → Visible/Collapsed
│   │   ├── LoadingTextConverter.cs                 → IsLoading → "Chargement..."
│   │   ├── StringNotEmptyToVisibilityConverter.cs  → string non vide → Visible
│   │   └── StringToVisibilityConverter.cs          → string vide → Collapsed
│   ├── Session/
│   │   └── SessionManager.cs                       ← Singleton : utilisateur connecté + token GUID
│   ├── Services/
│   │   ├── Interfaces/
│   │   │   ├── INavigationService.cs               ← Contrat navigation MVVM
│   │   │   └── ITransferParameter.cs               ← Contrat passage de paramètre entre ViewModels
│   │   ├── Jikan/
│   │   │   ├── IJikanService.cs                    ← Contrat API Jikan (MyAnimeList)
│   │   │   └── JikanService.cs                     ← Appels API + cache en mémoire + filtre client-side
│   │   └── NavigationService.cs                    ← Navigation MVVM via ContentControl + ITransferParameter
│   ├── ViewModels/
│   │   ├── AnimeCard/
│   │   │   ├── AnimeCardViewModel.cs               ← Carte anime (titre, image, score, SelectCommand)
│   │   │   ├── FavoriteCardViewModel.cs            ← Carte favori
│   │   │   └── SearchItemViewModel.cs              ← Résultat de recherche
│   │   ├── AnimeDetails/
│   │   │   ├── AnimeDetailsViewModel.cs            ← Détails anime : Jikan + notes + favoris + commentaires
│   │   │   └── CommentaireViewModel.cs             ← Wrapper commentaire (Username, IsCurrentUserAuthor, RatingDisplay)
│   │   ├── Auth/
│   │   │   ├── PasswordReset/
│   │   │   │   ├── ForgottenPasswordViewModel.cs   ← Saisie email pour réinitialisation
│   │   │   │   ├── ResetCodeVerificationViewModel.cs ← Vérification code envoyé par email
│   │   │   │   └── ResetPasswordViewModel.cs       ← Saisie nouveau mot de passe
│   │   │   ├── SignInViewModel.cs                  ← Connexion utilisateur
│   │   │   └── SignUpViewModel.cs                  ← Création de compte avec validation complète
│   │   ├── Carousel/
│   │   │   └── CarouselViewModel.cs                ← Carrousel par genre : navigation infinie + PageSize dynamique
│   │   ├── Favoris/
│   │   │   └── FavorisViewModel.cs                 ← Liste des favoris de l'utilisateur
│   │   ├── Search/
│   │   │   └── SearchViewModel.cs                  ← Recherche d'animes via Jikan
│   │   └── MainViewModel.cs                        ← Page principale : chargement des carrousels par genre
│   └── Views/
│       ├── AnimeDetails/
│       │   ├── AnimeDetailsView.xaml               ← Page détail : image, synopsis, scores, notes, commentaires
│       │   └── AnimeDetailsView.xaml.cs
│       ├── Auth/
│       │   ├── PasswordReset/
│       │   │   ├── ForgottenPasswordView.xaml
│       │   │   ├── ResetCodeVerificationView.xaml
│       │   │   └── ResetPasswordView.xaml
│       │   ├── SignInView.xaml
│       │   └── SignUpView.xaml
│       ├── Favoris/
│       │   └── FavorisView.xaml
│       ├── Search/
│       │   └── SearchView.xaml
│       ├── MainView.xaml                           ← Page principale avec carrousels + scroll persistant
│       ├── MainWindow.xaml                         ← Fenêtre principale (ContentControl MVVM)
│       └── TermsAndConditions.xaml
│
└── CineTrack.Data/                                 ← Projet données (base de données)
    ├── Context/
    │   ├── CineTrackDbContext.cs                   ← EF Core : SQLite, unicité email/username
    │   └── CineTrackDbContextFactory.cs            ← Design-time factory pour les migrations EF
    ├── Models/
    │   ├── Commentaire.cs                          ← Id, MalId, UtilisateurId, Texte, DateCreation
    │   ├── Favoris.cs                              ← Id, UtilisateurId, MalId (long)
    │   ├── Note.cs                                 ← Id, UtilisateurId, MalId, Rating (1-5), DateAdded
    │   └── Utilisateur.cs                          ← Id, FullName, Username, Email, Password, DateCreation
    ├── Repositories/
    │   ├── Interfaces/
    │   │   ├── ICommentaireRepository.cs
    │   │   ├── IFavorisRepository.cs
    │   │   ├── INoteRepository.cs
    │   │   └── IUtilisateurRepository.cs
    │   ├── CommentaireRepository.cs                ← GetByAnime (avec Include Utilisateur), Add, Remove
    │   ├── FavorisRepository.cs                    ← GetByUser, Add, Remove
    │   ├── NoteRepository.cs                       ← GetByUserAndAnime, GetByAnime, Add, Update
    │   └── UtilisateurRepository.cs                ← GetByUsername, GetByEmail, AddUser...
    └── Services/
        ├── EmailServ/
        │   ├── IEmailService.cs
        │   └── EmailService.cs                     ← Envoi email SMTP (réinitialisation mot de passe)
        ├── HashServ/
        │   └── HashService.cs                      ← BCrypt : hashage et vérification mot de passe
        ├── NoteServ/
        │   ├── INoteService.cs
        │   └── NoteService.cs                      ← RateAsync, GetCommunityScore, GetNote
        ├── PasswordResetStoreServ/
        │   └── PasswordResetStore.cs               ← Stockage temporaire des codes de réinitialisation
        └── UtilisateurServ/
            ├── IUtilisateurService.cs
            └── UtilisateurService.cs               ← SignUp, SignIn, validation métier
```

---

## Architecture

L'application suit le patron **MVVM** (Model-View-ViewModel) avec injection de dépendances via `Microsoft.Extensions.DependencyInjection`.

### Navigation

La navigation est gérée par `NavigationService` — les ViewModels sont résolus via le conteneur DI et affichés dans un `ContentControl` via `DataTemplate`. Pour passer des données entre pages (ex: `MalId` vers `AnimeDetailsViewModel`), le ViewModel implémente `ITransferParameter` et reçoit le paramètre via `TransferParameter(object param)`.

### Données anime

L'application suit le principe **Option C** — Jikan (MyAnimeList) est la source de vérité pour les données d'affichage (titre, image, synopsis, score MAL). La base de données locale ne stocke que ce que l'application possède : notes des utilisateurs, favoris, et commentaires. Les animes sont référencés par leur `MalId` (long) sans FK vers une table Anime locale.

### Carrousels

Chaque genre charge jusqu'à 20 animes via Jikan. Comme le filtre `Genres` de `AnimeSearchConfig` est ignoré par la librairie, un filtre client-side est appliqué après chaque page. Le nombre de cartes visibles (`PageSize`) est calculé dynamiquement selon la largeur disponible. La navigation est infinie dans les deux sens.

---

## Fonctionnalités implémentées

| Fonctionnalité                                      | Statut |
| --------------------------------------------------- | ------ |
| Création de compte (validation complète)            | ✅     |
| Connexion / Déconnexion                             | ✅     |
| Réinitialisation mot de passe par email             | ✅     |
| Carrousels d'animes par genre (navigation infinie)  | ✅     |
| Page détail anime (synopsis, épisodes, âge)         | ✅     |
| Score MAL affiché (Jikan)                           | ✅     |
| Score communauté CineTrack (notre DB)               | ✅     |
| Système de notation 1–5 par utilisateur             | ✅     |
| Favoris                                             | ✅     |
| Commentaires (ajout, suppression, affichage auteur) | ✅     |
| Note affichée sur chaque commentaire                | ✅     |
| Recherche d'animes                                  | ✅     |
| Persistance de la position de scroll                | ✅     |

---

## Commandes utiles

### Migrations EF Core

```
Add-Migration NomMigration -Project CineTrack.Data -StartupProject CineTrack.Data
Update-Database -Project CineTrack.Data -StartupProject CineTrack.Data
```

### Lancer l'application

Définir `CineTrack` comme projet de démarrage et lancer avec F5.

---

## Journal d'avancement

### 21/03/2026 — Mise en place de la base de données

- Modèle `Utilisateur` avec validation (regex mot de passe, email, contraintes d'unicité)
- Patron Repository avec `IUtilisateurRepository`
- `UtilisateurService` avec inscription (hachage BCrypt, vérification unicité)
- Squelette de connexion (token manquant)
- SQLite avec migrations EF Core
- Conteneur IoC dans `App.xaml.cs` avec support `appsettings.json`

_Pour ma propre souffrance personnelle (William) — Heures passées ce soir : 8h_

---

### 22/03/2026 — Création de compte (CU01)

- `SignUpViewModel` : validation complète, `IsLoading`, `TogglePasswordVisibilityCommand`
- `SignUpView.xaml` : formulaire complet, thème sombre, bannière d'erreur
- `SessionManager` : Singleton thread-safe, token GUID, `OpenSession` / `CloseSession`
- `App.xaml.cs` : DI complet, migration automatique au démarrage

_Pour ma propre souffrance personnelle (Angel) — Heures passées ce soir : 6h_

---

### Depuis — Fonctionnalités principales

- Navigation MVVM avec `NavigationService` + `ContentControl` + `DataTemplate`
- `ITransferParameter` pour passer le `MalId` vers `AnimeDetailsViewModel`
- Intégration Jikan (JikanDotNet) : carrousels par genre, recherche, détail anime
- Système de notes (1–5) via `Note`, `NoteRepository`, `NoteService`
- Commentaires avec affichage de la note de l'auteur via `CommentaireViewModel`
- Favoris via `Favoris`, `FavorisRepository`
- Refactorisation : suppression de `Anime` et `UtilisateurAnime`, remplacement par `MalId` direct
- UX : scroll persistant, carrousels centrés, taille des cartes dynamique selon la largeur

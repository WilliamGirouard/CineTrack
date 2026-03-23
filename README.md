# CineTrack
Projet d'application entreprise.

### Versions

  - Visual Studio 2022+
  - .NET 8.0 SDK

## Structure du projet

```
CineTrack/
├── CineTrack/                        ← Projet WPF (UI)
│   ├── App.xaml / App.xaml.cs        ← Point d'entrée, injection de dépendances (DI)
│   ├── Converters/                   ← Outils de conversion pour les bindings XAML
│   │   ├── BoolToVisibilityConverter.cs       → bool → Visible/Collapsed
│   │   ├── BoolToVisibilityInverseConverter   → bool inversé → Visible/Collapsed
│   │   ├── StringToVisibilityConverter.cs     → string vide → Collapsed
│   │   └── LoadingTextConverter.cs            → IsLoading=true → "Chargement..."
│   ├── Session/
│   │   └── SessionManager.cs         ← Singleton : garde l'utilisateur connecté + token GUID
│   ├── Services/
│   │   ├── INavigationService.cs     ← Contrat de navigation entre pages
│   │   └── NavigationService.cs      ← Navigation MVVM via ContentControl
│   ├── ViewModels/Auth/
│   │   ├── SignUpViewModel.cs         ← FAIT — logique création de compte
│   │   └── SignInViewModel.cs         ← VIDE — à compléter (SCRUM-37)
│   └── Views/Auth/
│       ├── SignUpView.xaml            ← FAIT — formulaire création de compte
│       ├── SignUpView.xaml.cs         ← FAIT — sync PasswordBox → ViewModel
│       ├── SignInView.xaml            ← VIDE — à faire (SCRUM-37)
│       └── SignInView.xaml.cs         ← VIDE — à faire (SCRUM-37)
│
└── CineTrack.Data/                   ← Projet données (base de données)
    ├── Models/
    │   └── Utilisateur.cs            ← Modèle BD : Id, FullName, Username, Email, Password, DateCreation
    ├── Context/
    │   └── CineTrackDbContext.cs      ← EF Core : connexion SQLite, unicité email/username
    ├── Repositories/
    │   ├── Interfaces/IUtilisateurRepository.cs
    │   └── UtilisateurRepository.cs  ← CRUD utilisateurs (GetByUsername, GetByEmail, AddUser...)
    └── Services/
        ├── HashService.cs            ← BCrypt : hashage et vérification mot de passe
        ├── Interfaces/IUtilisateurService.cs
        └── UtilisateurService.cs     ← Logique métier : SignUp, SignIn (token TODO), IsEmailUsed...
```

---

### AVANCEMENT 3/21/2026

	Ce qui a été fait : Mise en place de la DataBase
	
	- Modèle Utilisateur avec validation (regex mot de passe a.k.a securisation genre carac. minimal, caractere speciaux..., email, contraintes d'unicité)
	- Patron Repository avec IUtilisateurRepository
	- UtilisateurService avec inscription (hachage BCrypt avec HashService, vérification unicité email/username)
	- Squelette de connexion (Manque Token aka Garder la session ouverte 30min)
	- Base de données SQLite avec migrations EF Core
	- Conteneur IoC configuré dans App.xaml.cs avec support appsettings.json; Voir : https://cegepmv.github.io/420-413/injection_dependance/index.html 


À faire: SessionManager (Token), logique de connexion (Manque le Token justement), logique de deconnexion, vues (SignIn/SignUp/MainWindow), ModelView (SignIn/SignUp/MainWindow)

À titre informatif, l'application se lance, mais il y a juste un textBlock avec Test écrit. 
Aussi, les View autre que MainWindow sont des pages et non des windows 
(Pour que ça reste sur la même fenetre mais autre page)

Pour ma propre souffrance personnel (William) - Heure passée ce soir : 8h

---

## AVANCEMENT 3/22/2026

### SCRUM — Création de compte (CU01)

**`SignUpViewModel.cs`**
- Champs bindés : `FullName`, `Username`, `Email`, `Password`, `ConfirmPassword`
- Bouton S'inscrire désactivé tant que les champs sont vides (`CanExecute`)
- Validation complète à la soumission :
  - Username min. 3 caractères
  - Format courriel valide
  - Mot de passe min. 8 caractères + 1 chiffre + 1 caractère spécial
  - Confirmation mot de passe identique
- Messages d'erreur en français
- `IsLoading` pendant l'appel async (bouton désactivé + texte "Chargement...")
- `TogglePasswordVisibilityCommand` pour afficher/masquer le mot de passe (bouton 👁)
- Appelle `IUtilisateurService.SignUp()` qui hash le mot de passe (BCrypt) et sauvegarde en BD

**`SignUpView.xaml`**
- Formulaire complet avec tous les champs
- Design sombre (thème cohérent avec CineTrack)
- Styles réutilisables : `InputStyle`, `PasswordStyle`, `PrimaryButton`, `FieldLabel`
- Bouton désactivé visuellement quand `CanExecute = false`
- Bannière d'erreur rouge qui apparaît seulement s'il y a une erreur
- Lien "Se connecter" en bas

**`SessionManager.cs`**
- Singleton thread-safe
- `OpenSession(utilisateur)` → génère un token GUID en mémoire
- `CloseSession()` → efface tout
- `IsLoggedIn`, `CurrentUser`, `Token`

**`App.xaml.cs`**
- Injection de dépendances complète (DI)
- Migration BD automatique au démarrage
- Enregistrement : DbContext, Repository, Service, ViewModels, Views

**`MainWindow`**
- Utilise un `Frame` pour naviguer entre les `Page` (SignIn, SignUp, etc.)
- `NavigateTo(page)` appelable depuis le code-behind

Pour ma propre souffrance personnel (Angel) - Heure passée ce soir : 6h

---

## Comment ça fonctionne — Flux SignUp

```
Utilisateur remplit le formulaire
        ↓
SignUpView.xaml  →  binding  →  SignUpViewModel
        ↓
Clic "S'inscrire"  →  SignUpCommand
        ↓
ValiderChamps() — vérifie tout localement
        ↓
IUtilisateurService.SignUp()
        ↓
HashService.PasswordHasher()  →  BCrypt hash
        ↓
IUtilisateurRepository.AddUser()  →  SQLite
        ↓
TODO : naviguer vers SignIn
```

---

## Pourquoi y'a un Repository ET un Service — c'est pas redondant ?

Non, ils font pas la même chose.

Le **Repository** il sait juste parler à la base de données. Il fait du CRUD bête : `Add`, `Find`, `Remove`. Il sait pas ce qu'est un signup, il sait pas c'est quoi un mot de passe hashé, il s'en fout. 

Le **Service** lui c'est là que la vraie logique est. C'est lui qui vérifie si l'email est déjà pris, qui hash le mot de passe avec BCrypt, qui throw les bonnes exceptions si quelque chose va pas.

L'avantage de séparer les deux : si un jour on change de base de données (genre SQLite → PostgreSQL), on change juste le Repository, le Service touche à rien. Pareil pour les tests unitaires.

---

## Points importants à savoir

**PasswordBox et le binding**
WPF interdit de binder `PasswordBox.Password` directement pour des raisons de sécurité.
La solution utilisée : l'événement `PasswordChanged` dans le code-behind synchronise manuellement
la valeur vers le ViewModel. C'est le workaround standard WPF.

**Converters**

Un converter c'est juste un traducteur entre le ViewModel et le XAML.
Le problème c'est que le XAML comprend pas les `bool` ou les `string` pour certaines propriétés.
Par exemple `Visibility` veut `Visible` ou `Collapsed`, pas `true` ou `false`.
Donc le converter fait la traduction à chaque fois que la valeur change.

```
ViewModel          Converter                    XAML
──────────────────────────────────────────────────────────
bool true    →  BoolToVisibility        →  Visibility.Visible
bool false   →  BoolToVisibility        →  Visibility.Collapsed
bool true    →  BoolToVisibilityInverse →  Visibility.Collapsed  ← inversé
bool false   →  BoolToVisibilityInverse →  Visibility.Visible    ← inversé
"" vide      →  StringToVisibility      →  Visibility.Collapsed
"une erreur" →  StringToVisibility      →  Visibility.Visible
bool false   →  LoadingText             →  "S'inscrire"
bool true    →  LoadingText             →  "Chargement..."
```

**`BoolToVisibilityConverter`** — `true` → Visible, `false` → Collapsed
Utilisé pour afficher le TextBox (mot de passe en clair) quand `IsPasswordVisible = true` :
```xml
<TextBox Visibility="{Binding IsPasswordVisible, Converter={StaticResource BoolToVisConverter}}"/>
```

**`BoolToVisibilityInverseConverter`** — `true` → Collapsed, `false` → Visible (l'inverse)
Utilisé pour afficher le PasswordBox (masqué) quand `IsPasswordVisible = false`.
Les deux ensemble forment le toggle 👁 — quand l'un est visible, l'autre est caché :
```xml
<PasswordBox Visibility="{Binding IsPasswordVisible, Converter={StaticResource BoolToVisInverseConverter}}"/>
```

**`StringToVisibilityConverter`** — string non vide → Visible, vide/null → Collapsed
Utilisé pour la bannière d'erreur rouge : elle est invisible par défaut, et apparaît seulement quand `ErrorMessage` contient quelque chose :
```xml
<Border Visibility="{Binding ErrorMessage, Converter={StaticResource StringToVisConverter}}">
    <TextBlock Text="{Binding ErrorMessage}"/>
</Border>
```

**`LoadingTextConverter`** — change le texte du bouton pendant un appel à la BD
```xml
<!-- IsLoading=false → "S'inscrire"  |  IsLoading=true → "Chargement..." -->
<Button Content="{Binding IsLoading, Converter={StaticResource LoadingTextConverter},
                           ConverterParameter=S'inscrire}"/>
```

**NavigationService — c'est quoi et pourquoi c'est pas encore utilisé**

Le `NavigationService` c'est pour faire de la navigation MVVM-pur, c'est-à-dire qu'un ViewModel peut changer de page sans jamais connaître les Views. En ce moment la `MainWindow` navigue encore manuellement avec un `Frame`. Le service est prêt mais pas encore branché, ça va être fait dans SCRUM-37.

Pour l'utiliser éventuellement :
```csharp
_navigationService.NavigateTo<SignInViewModel>();
```

**Styles XAML**
Les styles dans `<Page.Resources>` fonctionnent comme des classes CSS :
on définit les propriétés une fois et on les applique avec `Style="{StaticResource NomDuStyle}"`.

---

## TODO — Backlog Jira restant

### Authentification
| Ticket | Tâche | Fichiers à créer/modifier |
|--------|-------|--------------------------|
| SCRUM-37 | Vérification des identifiants (SignIn) | `SignInViewModel.cs`, `SignInView.xaml`, `UtilisateurService.SignIn()` + token `SessionManager` |

### Recherche d'animé
| Ticket | Tâche |
|--------|-------|
| SCRUM-68 | Barre de recherche |
| SCRUM-69 | Filtrage (genre, note, nb épisodes) |
| SCRUM-70 | Page information animé (fiche détaillée) |

### Gestion des favoris
| Ticket | Tâche |
|--------|-------|
| SCRUM-71 | Ajout au favoris |
| SCRUM-72 | Suppression des favoris |
| SCRUM-73 | Accès à la page favoris |

### Commentaires
| Ticket | Tâche |
|--------|-------|
| SCRUM-74 | Lire les commentaires |
| SCRUM-75 | Ajout d'un commentaire |
| SCRUM-76 | Supprimer son commentaire |

### Expérience utilisateur
| Ticket | Tâche |
|--------|-------|
| SCRUM-77 | Page principale (accueil) |
| SCRUM-78 | Affordance des boutons |
| SCRUM-79 | Uniformité du style des pages |
| SCRUM-80 | Vitesse de navigation |
| SCRUM-81 | Beauté de la page d'accueil |

---

## Prochaine étape recommandée

**SCRUM-37 — SignIn** est le plus urgent car sans connexion, rien d'autre n'est accessible.

Fichiers à compléter dans l'ordre :
1. `UtilisateurService.SignIn()` → appeler `SessionManager.OpenSession()` après validation BCrypt
2. `SignInViewModel.cs` → même pattern que `SignUpViewModel`
3. `SignInView.xaml` → même structure que `SignUpView`
4. Brancher la navigation : après SignUp réussi → SignIn, après SignIn réussi → MainWindow (accueil)
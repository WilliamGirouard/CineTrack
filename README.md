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


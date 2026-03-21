# CineTrack
Projet d'application entreprise.

### Versions

  - Visual Studio 2022+
  - .NET 8.0 SDK

### AVANCEMENT 3/21/2026

	feat: Mise en place de la DataBase

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

Pour ma propre souffrance personnel - Heure passée ce soir : 8h
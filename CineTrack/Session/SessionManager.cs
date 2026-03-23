using CineTrack.Data.Models;

namespace CineTrack.Session
{
    /// <summary>
    /// Singleton qui représente la session active de l'utilisateur connecté.
    /// Contient le token GUID en mémoire et l'utilisateur courant.
    /// </summary>
    public class SessionManager
    {
        // ── Singleton ──────────────────────────────────────────────────────────
        private static SessionManager? _instance;
        private static readonly object _lock = new();

        public static SessionManager Instance
        {
            get
            {
                lock (_lock)
                {
                    _instance ??= new SessionManager();
                    return _instance;
                }
            }
        }

        private SessionManager() { }

        // ── État de session ────────────────────────────────────────────────────
        public string? Token { get; private set; }
        public Utilisateur? CurrentUser { get; private set; }
        public bool IsLoggedIn => Token != null && CurrentUser != null;

        // ── Méthodes publiques ─────────────────────────────────────────────────

        /// <summary>Ouvre une session pour l'utilisateur donné et génère un token GUID.</summary>
        public void OpenSession(Utilisateur utilisateur)
        {
            CurrentUser = utilisateur;
            Token = Guid.NewGuid().ToString();
        }

        /// <summary>Ferme la session et efface toutes les données en mémoire.</summary>
        public void CloseSession()
        {
            CurrentUser = null;
            Token = null;
        }
    }
}

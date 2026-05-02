using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services.NoteServ
{
    public interface INoteService
    {
        Task RateAsync(int utilisateurId, long malId, int note);
        Task<double?> GetCommunityScoreAsync(long malId);
        Task<int?> GetNoteAsync(int utilisateurId, long malId);
    }
}

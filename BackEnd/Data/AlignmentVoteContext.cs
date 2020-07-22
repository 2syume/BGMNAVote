using BGMNANotebookGrab.Models;
using Microsoft.EntityFrameworkCore;

namespace BGMNANotebookGrab.Data
{
    public class AlignmentVoteContext : DbContext
    {
        public AlignmentVoteContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Nominate> Nominates { get; set; }

        public DbSet<Vote> Votes { get; set; }
    }
}
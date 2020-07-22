using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BGMNANotebookGrab.Models
{
    [Table("Vote")]
    public class Vote
    {
        [Key] public int VoteId { get; set; }

        [ForeignKey("Nominate")] [Required] public int NominateId { get; set; }

        public Nominate Nominate { get; set; }

        public DateTime Date { get; set; }
    }
}
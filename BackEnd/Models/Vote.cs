using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BGMNANotebookGrab.Models
{
    [Table("Vote")]
    public class Vote
    {
        [Key] public int VoteId { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Saying { get; set; }

        [Required] public string Alignment { get; set; }
    }
}
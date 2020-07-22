using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BGMNANotebookGrab.Models
{
    [Table("Nominate")]
    public class Nominate
    {
        [Key] public int NominateId { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Saying { get; set; }

        [Required] public string Alignment { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Prime.Models
{
    [Table("AccessAgreementNotes")]
    public class AccessAgreementNote : IEnrolleeNote
    {
        [Key]
        public int? Id { get; set; }

        [JsonIgnore]
        public int EnrolleeId { get; set; }

        [JsonIgnore]
        public Enrollee Enrollee { get; set; }

        public string Note { get; set; }

        public DateTime NoteDate { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelData.Model
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public DateTime DataDeCriacao { get; set; }
        public DateTime? DataDeEdicao { get; set; }
        public DateTime? DataDeRemocao { get; set; }

        [Required]
        public int Status { get; set; }
    }
}

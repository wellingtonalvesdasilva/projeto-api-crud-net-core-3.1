using System.ComponentModel.DataAnnotations;

namespace ModelData.Model
{
    public class Usuario : BaseEntity
    {
        [StringLength(400)]
        [Required]
        public string NomeCompleto { get; set; }

        [StringLength(500)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        [Required]
        public string Senha { get; set; }
    }
}

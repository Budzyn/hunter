using System.ComponentModel.DataAnnotations;

namespace Hunter.Services.Dto
{
    public class UserRoleDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}

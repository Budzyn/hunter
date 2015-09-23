using System;
using System.ComponentModel.DataAnnotations;
using Hunter.DataAccess.Entities;

namespace Hunter.Services.Dto
{
    public class ActivityDto
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string UserLogin { get; set; }

        [StringLength(100)]
        public string UserAlias { get; set; }

        [StringLength(150)]
        public ActivityType Tag { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }

        [Required]
        [StringLength(2000)]
        public string Url { get; set; }

        public DateTime Time { get; set; }
    }
}

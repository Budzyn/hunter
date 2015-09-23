using System.ComponentModel.DataAnnotations;
using Hunter.DataAccess.Entities.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hunter.Services
{
    public class PoolViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        // [CustomValidation(typeof(HunterValidation), "ValidateIsPoolNameExist")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "poolbackground")]
        public ICollection<PoolBackground> PoolBackground { get; set; }
    }
}
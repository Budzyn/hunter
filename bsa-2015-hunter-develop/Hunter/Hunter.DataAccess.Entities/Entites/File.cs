using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.DataAccess.Entities
{
    public class File : IEntity
    {
        public File()
        {
            Added = DateTime.Now;
        }

        public int Id { get; set; }

        [StringLength(2000)]
        public string FileName { get; set; }
        public DateTime Added { get; set; }

        [StringLength(2000)]
        public string Path { get; set; }
    }
}

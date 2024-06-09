using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.Models
{
    [Table("file_proof")]
    public class FileProof : BaseModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public double Size { get; set; }
        public Guid MonitorSessionId { get; set; }

        [ForeignKey("Student")]
        public Guid StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Computer")]
        public Guid ComputerId { get; set; }
        public virtual Computer Computer { get; set; }

    }
}

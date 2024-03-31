using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.Models
{
    [Table("refresh_token")]
    public class RefreshToken : BaseModel
    {
        public Guid UserId { get; set; }
        public string ReToken { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}

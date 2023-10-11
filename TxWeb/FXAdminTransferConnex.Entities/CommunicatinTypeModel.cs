using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class CommunicatinTypeModel
    {

        public byte CommunicationTypeId { get; set; }

        [MaxLength(50)]
        public string TypeName { get; set; }
    }

    public class TaskTypeModel
    {
        public int TaskTypeId { get; set; }
        public string TaskType { get; set; }
    }
}

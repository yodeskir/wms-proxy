using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WMSDataAccess.UserManagement.Entities
{
    public class WMSLayers
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id", Order = 1)]
        public long id { get; set; }
        public string username { get; set; }
        public string layername { get; set; }
        public string datasourcename { get; set; }
        public string description { get; set; }
        public string extent { get; set; }
        public string projection { get; set; }
        public bool ispublic { get; set; }
        public string layertype { get; set; }

    }
}


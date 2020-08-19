using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace wmsDataAccess.UserManagement.Entities
{
    public class WMSMapsLog
    {
        public WMSMapsLog()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long id { get; set; }
        public string mapname { get; set; }
        public string username { get; set; }

        public string mapcontent { get; set; }
        public DateTime logdate { get; set; }
    }
}

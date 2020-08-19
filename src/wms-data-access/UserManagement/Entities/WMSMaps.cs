using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMSDataAccess.UserManagement.Entities
{
    public class WMSMaps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long id { get; set; }
        public string mapname { get; set; }
        public string username { get; set; }
        public string center { get; set; }
        public short zoom { get; set; }
        public string mapprojection { get; set; }

        public string mapfile { get; set; }

        public WMSMaps() { }
    }
}

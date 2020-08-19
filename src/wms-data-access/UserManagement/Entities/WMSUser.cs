using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMSDataAccess.UserManagement.Entities
{
    public class WMSUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [DisplayName("User name")]
        [Required]
        public string username { get; set; }

        [DisplayName("Email")]
        [Required]
        public string useremail { get; set; }

        [DisplayName("Full name")]
        public string userfullname { get; set; }

        [DisplayName("Password")]
        [Required]
        [NotMapped]
        public string password { get; set; }

        public string hashedpassword { get; set; }
        public string salt { get; set; }
        public DateTime creationdate { get; set; }

        [ForeignKey(nameof(username))]
        public ICollection<WMSMaps> maps { get; set; }
    }
}

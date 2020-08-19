using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WMSDataAccess.UserManagement.DBContexts;
using WMSDataAccess.UserManagement.Entities;
using WMSTools.Interfaces;

namespace WMSDataAccess.UserManagement
{
    public class UserDBSeeder : IUserDBSeeder
    {
        private UserDBContext _context;
        private IHashHelper _hashHelper;
        private IUserManager _userManager;

        public UserDBSeeder(UserDBContext context, IHashHelper hashHelper, IUserManager userManager)
        {
            _context = context;
            _hashHelper = hashHelper;
            _userManager = userManager;
        }

        public void Seed()
        {
            if (_context.Users.Any(u=>u.username == "yodeski"))
                return;

            var salt = _hashHelper.GetSalt();
            var user = new WMSUser
            {
                username = "yodeski",
                useremail = "yodeski@gmail.com",
                userfullname = "Yodeski Rodriguez Alvarez",
                salt = salt,
                creationdate = DateTime.Now,
                hashedpassword = _hashHelper.GetHash("lolo" + salt)
            };
            _context.Users.Add(user);
            _userManager.CreateUserShema("yodeski");



            if (_context.Maps.Any(u => u.mapname == "canada"))
                return;

            var map = new WMSMaps
            {
                mapname = "canada",
                mapprojection = "EPSG:3857",
                username = "yodeski",
                mapfile = "canada.map",
                center = "0, 0",
                zoom = 2
            };
            _context.Maps.Add(map);
            _context.SaveChanges();
        }
    }
}
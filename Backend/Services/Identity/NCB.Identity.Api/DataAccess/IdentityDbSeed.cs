using NCB.Identity.Api.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.DataAccess
{
    public class IdentityDbSeed
    {
        private IdentityDbContext _context;

        public IdentityDbSeed(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Roles.Any())
            {
                await _context.AddRangeAsync(RoleData());
                await _context.SaveChangesAsync();
            }

            if (!_context.Accounts.Any())
            {
                await _context.AddRangeAsync(AccountData());
                await _context.SaveChangesAsync();
            }
        }

        private List<Role> RoleData()
        {
            return new List<Role>()
            {
                new Role(){ Id = new Guid("5d752d9a-ee65-414f-935a-e9f4c2d26f40"), Name = "Admin" },
                new Role(){ Id = new Guid("da9c7599-05c4-4ec3-a5bd-c18e1009b43c"), Name = "User" }
            };
        }

        private List<Account> AccountData()
        {
            return new List<Account>()
            {
                new Account(password: "admin"){
                    Id =  new Guid("982c55f9-bb39-473f-a335-9cc458e2fd15"),
                    Username = "admin",
                    AccountInRoles = new List<AccountInRole>(){
                        new AccountInRole()
                        {
                            RoleId= new Guid("5d752d9a-ee65-414f-935a-e9f4c2d26f40"),
                        }
                    }
                },
                new Account(password: "user"){
                    Id =  new Guid("e4cf1403-2e49-4f29-ae2a-442a607aa970"),
                    Username = "user",
                    AccountInRoles = new List<AccountInRole>(){
                        new AccountInRole()
                        {
                            RoleId= new Guid("da9c7599-05c4-4ec3-a5bd-c18e1009b43c"),
                        }
                    }
                }
            };
        }
    }
}

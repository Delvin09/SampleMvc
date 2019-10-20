using System.Collections.Generic;

namespace SampleMvc.Controllers
{
    class DatabaseContext
    {
        public List<User> Users { get; set; }
        public DatabaseContext()
        {
            Users = new List<User>() {
                new User() { Id = 1, Name = "Sam", Age = 33, Email = "sam@gmai.com" },
                new User() { Id = 2, Name = "Jhon", Age = 25, Email = "jhon@microsoft.com" }
            };
        }
    }
}

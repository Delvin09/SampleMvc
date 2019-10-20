using System.Linq;

namespace SampleMvc.Controllers
{

    class UsersController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public UsersController()
        {
            _db = new DatabaseContext();
        }

        // /Users/
        // /Users/Index
        public ViewResult Index(WebContext context)
        {
            return new ViewResult(_db.Users);
        }

        // /Users/GetUser/int
        // /Users?GetUser=int
        public ViewResult GetUser(WebContext context)
        {
            var param = context.Request.Parameters.Skip(1).FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(param))
                param = context.Request.Parameters.Skip(2).FirstOrDefault()?.Key;
            if (param != null && int.TryParse(param, out int id))
            {
                var user = _db.Users.FirstOrDefault(x => x.Id == id);
                return new ViewResult(user);
            }
            return null;
        }
    }
}

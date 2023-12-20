
using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        public DataContext _context;

        public UsersController(DataContext context)
        {
            this._context = context;
        }
    }
}
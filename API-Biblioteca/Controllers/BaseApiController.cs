using Microsoft.AspNetCore.Mvc;

namespace API_Biblioteca.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {      
    }
}

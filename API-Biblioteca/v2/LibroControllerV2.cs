using API_Biblioteca.Controllers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace API_Biblioteca.v2
{
    [ApiVersion("2.0")]
    public class LibroControllerV2 : BaseApiController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

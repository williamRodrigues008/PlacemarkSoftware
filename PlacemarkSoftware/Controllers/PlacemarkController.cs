using Microsoft.AspNetCore.Mvc;
using PlacemarkSoftware.API.Interfaces;
using PlacemarkSoftware.API.Models;

namespace PlacemarkSoftware.API.Controllers
{
    [Route("api/[controller]")]
    public class PlacemarkController : Controller
    {

        private readonly IReadPlacemarks _placemarks;

        public PlacemarkController(IReadPlacemarks placemarks)
        {
            _placemarks = placemarks;
        }

        [HttpGet("GetAllPlacemarks")]
        public IActionResult GetAllPlacemarks()
        {
            var getAll = _placemarks.GetAllPlacemarks();
            if (getAll == null)
                return NotFound();

            return Ok(getAll);
        }

        [HttpPost("ExportPlacemarks")]
        public IActionResult ExportPlacemarks([FromBody] Placemark placemark, string typeSearch)
        {
            var placemarksList = _placemarks.GetPlacemark(typeSearch, placemark);
            if (placemarksList == null)
                return NotFound("Não encontramos nenhuma informação com o(s) parâmetro(s) de busca. Por favor, use o campo de [Buscas] e copie os dados que deseja exportar!");
            _placemarks.ExportPlacemarks(placemarksList);

            return Ok(placemarksList);
        }

        [HttpPost("GetPlacemarks")]
        public IActionResult GetPlacemarks([FromBody] Placemark placemark, string typeSearch)
        {
            var placemarksList = _placemarks.GetPlacemark(typeSearch, placemark);
            if (placemarksList == null)
                return NotFound("Nenhuma informação encontrada, redefina os parametro para realizar uma nova busca!");

            return Ok(placemarksList);
        }
    }
}

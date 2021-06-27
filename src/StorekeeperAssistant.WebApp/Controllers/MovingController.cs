using Microsoft.AspNetCore.Mvc;
using StorekeeperAssistant.Api.Models.Movings;
using StorekeeperAssistant.Api.Services;
using System.Threading.Tasks;

namespace StorekeeperAssistant.WebApp.Controllers
{
    public class MovingController : Controller
    {
        private readonly IMovingRemoteCallService _movingRemoteCallService;

        public MovingController(IMovingRemoteCallService movingRemoteCallService)
        {
            _movingRemoteCallService = movingRemoteCallService;
        }

        /// <summary>
        /// Форма спискок всех перемещений
        /// </summary>
        [Route("movings")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Форма спискок всех перемещений
        /// </summary>
        [Route("movings/create")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Получить список всех перемещений и ТМЦ
        /// </summary>
        [HttpGet("movings/get")]
        public async Task<IActionResult> GetAsync(GetMovingRequest request)
        {
            var response = await _movingRemoteCallService.GetAsync(request);
            return Json(response);
        }

        /// <summary>
        /// Создать перемещение
        /// </summary>
        [HttpPost("movings/create")]
        public async Task<IActionResult> CreateAsync([FromBody]CreateMovingRequest request)
        {
            var response = await _movingRemoteCallService.CreateAsync(request);
            return Json(response);
        }

        /// <summary>
        /// Создать перемещение
        /// </summary>
        [HttpPost("movings/delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteMovingRequest request)
        {
            var response = await _movingRemoteCallService.DeleteAsync(request);
            return Json(response);
        }
    }
}

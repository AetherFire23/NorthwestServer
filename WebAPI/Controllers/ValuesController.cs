using Microsoft.AspNetCore.Mvc;
using WebAPI.Temp_settomgs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("Sylvain/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly PlayerContext _playerContext;
        public ValuesController(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        [HttpGet]
        [Route("runcs2")]
        public async Task<ActionResult> RunConstructor2() { return Ok(); }

        [HttpGet]
        [Route("GetTaskSettings")]
        public async Task<ActionResult<List<TaskSetting>>> GetTaskSettings()
        {
            return _playerContext.TaskSettings.ToList();
        }

        [HttpPut]
        [Route("SaveTaskSettings")]
        public async Task<ActionResult> SaveTaskSettings([FromBody]List<TaskSetting> settings)
        {
            foreach (var setting in settings)
            {
                var sett = _playerContext.TaskSettings.First(x => x.Id == setting.Id);
                sett.SerializedProperties = setting.SerializedProperties;
            }
            _playerContext.SaveChanges();
            return Ok();
        }

    }
}

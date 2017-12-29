using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBoardAuth.Api.Models;
using WebBoardAuth.Logic.Service.Interface;

using WebBoardAuth.Logic.Models;
using Microsoft.AspNetCore.Cors;
using WebBoardAuth.Logic;

namespace WebBoardAuth.Api.Controllers.v1
{
   // [Produces("application/json")]
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class AudienceController : Controller
    {

        private ILogicUnitOfWork LogicUnitOfWork;
        public AudienceController(ILogicUnitOfWork logicUnitOfWork)
           // : base(redisConnectionMultiplexer)
        {
            LogicUnitOfWork = logicUnitOfWork;
        }

        [HttpGet("GetAll")]
        public async Task<object> GetAll()
        {
            return await LogicUnitOfWork.AudienceService.GetAllAudience();
        }

        [HttpGet("Get")]
        public async Task<object> Get(string clientId)
        {
            var newAudience = await LogicUnitOfWork.AudienceService.FindAudience(clientId);
            return newAudience;
        }

        private AudienceViewModel dmmy() {
            return new AudienceViewModel()
            {
                Issuer = "mywww.www.com",
                Name ="WebBoard"
            };
        }
        [HttpGet("Post")]
        // api/Audience/Post
        public async Task<object> Post(AudienceViewModel audienceModel)
        {
            audienceModel = dmmy();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var audienceDto = new AudienceDto();
            audienceDto.Name = audienceModel.Name;
            audienceDto.Issuer = audienceModel.Issuer;
            var newAudience = await LogicUnitOfWork.AudienceService.AddAudience(audienceDto);
            return Ok(newAudience);
        }
    }
}
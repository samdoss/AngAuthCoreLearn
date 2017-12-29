using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebBoardAuth.Logic.Models;
using System.Threading.Tasks;
using WebBoardAuth.Api.Models;
using WebBoardAuth.Logic;

namespace WebBoardAuth.Api.Controllers.v1
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
    public class ClientController : Controller
    {

        private ILogicUnitOfWork LogicUnitOfWork;
        public ClientController(ILogicUnitOfWork logicUnitOfWork)
           // : base(redisConnectionMultiplexer)
        {
            LogicUnitOfWork = logicUnitOfWork;
        }

        // POST api/Client/Register
        [AllowAnonymous]
        [HttpGet("Register")]
        public async Task Register(ClientViewModel model)
        {
            var clientDto = new ClientDto();
            clientDto.Id = model.Id;
            clientDto.Name = model.Name;
            await LogicUnitOfWork.ClientService.AddClient(clientDto);
        }
        // POST api/Client/Find
        [AllowAnonymous]
        [HttpGet("Find")]
        public async Task<object> Find(string client_id)
        {
           return await LogicUnitOfWork.ClientService.FindClient(client_id);
        }
    }
}
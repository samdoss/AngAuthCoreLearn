//using WebBoardAuth.DataAccess.Redis;
using WebBoardAuth.DataAccess.Sql;
using WebBoardAuth.Logic.Service.Implement;
using WebBoardAuth.Logic.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebBoardAuth.Logic
{
    public interface ILogicUnitOfWork
    {
        IAudienceService AudienceService { get; set; }
        IClientService ClientService { get; set; }
        //IEmailService EmailService { get; set; }
        IAccessTokenService AccessTokenService { get; set; }
        //IManageAmphurService ManageAmphurService { get; set; }
    }

    public class LogicUnitOfWork : ILogicUnitOfWork
    {
        private AuthDbConnection _sqlConnection { get; set; }
        //private RedisUnitOfWork _redisUnitOfWork { get; set; }
        //private IRedisConnectionMultiplexer _redisConnectionMultiplexer { get; set; }

        private IAudienceService _audienceService;
        private IClientService _clientService;
       
        private IAccessTokenService _accessTokenService;
        
        //public LogicUnitOfWork()
        //{
        //    _sqlConnection = new AuthDbConnection();
        //}

        public LogicUnitOfWork(
            //IRedisConnectionMultiplexer redisConnectionMultiplexer,
            AuthDbConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
            //_redisConnectionMultiplexer = redisConnectionMultiplexer;
            // _redisUnitOfWork = new RedisUnitOfWork(_redisConnectionMultiplexer);
        }

        public IAudienceService AudienceService
        {
            get { return _audienceService ?? (_audienceService = new AudienceService(_sqlConnection)); }
            set { _audienceService = value; }
        }

        public IClientService ClientService
        {
            get { return _clientService ?? (_clientService = new ClientService(_sqlConnection)); }
            set { _clientService = value; }
        }

        //public IEmailService EmailService
        //{
        //    get { return _emailService ?? (_emailService = new EmailService()); }
        //    set { _emailService = value; }
        //}

        public IAccessTokenService AccessTokenService
        {
            get { return _accessTokenService ?? (_accessTokenService = new AccessTokenService(_sqlConnection)); }
            set { _accessTokenService = value; }
        }

        //public IManageAmphurService ManageAmphurService
        //{
        //    get { return _manageAmphurService ?? (_manageAmphurService = new ManageAmphurService(_sqlConnection)); }
        //    set { _manageAmphurService = value; }
        //}
    }
}

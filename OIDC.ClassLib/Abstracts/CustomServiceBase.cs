using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OIDC.ClassLib.Abstracts
{
    public abstract class CustomServiceBase<T> : ControllerBase
    {
        #region DataMembers

        protected ILogger<CustomServiceBase<T>> _logger;
        protected IMapper _mapper;
        protected CustomClientBase? _client;

        #endregion

        #region Ctor

        public CustomServiceBase(ILogger<CustomServiceBase<T>> logger, IMapper mapper, CustomClientBase customClient)
        {
            _logger = logger;
            _mapper = mapper;
            _client = customClient;
        }

        public CustomServiceBase(ILogger<CustomServiceBase<T>> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        #region TypeFunctions

        public async Task<T?> FuncInvoker(Func<Task<T>?> clientAction)
        {
            try
            {
                _logger.LogInformation("Invoking client request to microservice");
                T res = await clientAction.Invoke();
                _logger.LogInformation("Client request has been invoked successfully");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> FuncInvoker(Func<Task<IEnumerable<T>>?> clientAction)
        {
            try
            {
                _logger.LogInformation("Invoking client request to microservice");
                IEnumerable<T> res = await clientAction.Invoke();
                _logger.LogInformation("Client request has been invoked successfully");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            return null;
        }

        public async Task<IEnumerable<T>> FuncInvoker(Func<T, Task<IEnumerable<T>>?> clientAction, T param1)
        {
            try
            {
                _logger.LogInformation("Invoking client request to microservice");
                IEnumerable<T> res = await clientAction.Invoke(param1);
                _logger.LogInformation("Client request has been invoked successfully");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            return null;
        }

        public async Task<IEnumerable<T>> FuncInvoker(Func<IEnumerable<T>, Task<IEnumerable<T>>?> clientAction, IEnumerable<T> enumerableParam1)
        {
            try
            {
                _logger.LogInformation("Invoking client request to microservice");
                IEnumerable<T> res = await clientAction.Invoke(enumerableParam1);
                _logger.LogInformation("Client request has been invoked successfully");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            return null;
        }

        #endregion

        #region DTOFunctions

        public async Task<CustomDtoBase> FuncInvoker(Func<Task<CustomDtoBase>?> clientAction)
        {
            try
            {
                _logger.LogInformation("Invoking client request to microservice");
                CustomDtoBase res = await clientAction.Invoke();
                _logger.LogInformation("Client request has been invoked successfully");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<CustomDtoBase>> FuncInvoker(Func<Task<IEnumerable<CustomDtoBase>>?> clientAction)
        {
            try
            {
                _logger.LogInformation("Invoking client request to microservice");
                IEnumerable<CustomDtoBase> res = await clientAction.Invoke();
                _logger.LogInformation("Client request has been invoked successfully");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            return null;
        }

        public async Task<IEnumerable<CustomDtoBase>> FuncInvoker(Func<CustomDtoBase, Task<IEnumerable<T>>?> clientAction, CustomDtoBase param1)
        {
            try
            {
                _logger.LogInformation("Invoking client request to microservice");
                IEnumerable<CustomDtoBase> res = (IEnumerable<CustomDtoBase>)await clientAction.Invoke(param1);
                _logger.LogInformation("Client request has been invoked successfully");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<CustomDtoBase>> FuncInvoker(Func<IEnumerable<CustomDtoBase>, Task<IEnumerable<CustomDtoBase>>?> clientAction, IEnumerable<CustomDtoBase> enumerableParam1)
        {
            try
            {
                _logger.LogInformation("Invoking client request to microservice");
                IEnumerable<CustomDtoBase> res = (IEnumerable<CustomDtoBase>)await clientAction.Invoke(enumerableParam1);
                _logger.LogInformation("Client request has been invoked successfully");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        #endregion


    }
}

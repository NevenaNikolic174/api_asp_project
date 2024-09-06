using MyProject.Application.Actor;
using MyProject.Application.UseCases;
using MyProject.Implementation.Exceptions;
using System;
using System.Linq;

namespace ProjekatImplementation
{
    public class UseCaseHandler
    {
        private readonly IApplicationActor _actor;
        private readonly IUseCaseLogger _logger;

        public UseCaseHandler(IApplicationActor actor, IUseCaseLogger logger)
        {
            _actor = actor;
            _logger = logger;
        }
        public void HandleCommand<TData>(ICommand<TData> command, TData data)
        {
            _logger.Log(command, _actor, data);
            if (!_actor.AllowedUseCases.Contains(command.Id))
            {
                throw new UnauthorizedException(command, _actor);
            }

            command.Execute(data);
        }

        public TResult HandleQuery<TResult, TSearch>(IQuery<TResult, TSearch> query, TSearch search)
            where TResult : class
        {
            _logger.Log(query, _actor, search);
            if (!_actor.AllowedUseCases.Contains(query.Id))
            {
                throw new UnauthorizedException(query, _actor);
            }

            var result = query.Exectue(search);

            return result;
        }
    }
}

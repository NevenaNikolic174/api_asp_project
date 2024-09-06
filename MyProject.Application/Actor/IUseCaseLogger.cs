using MyProject.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Actor
{
    public interface IUseCaseLogger
    {
        void Log(IUseCase useCase, IApplicationActor actor, object useCaseData);
    }
}

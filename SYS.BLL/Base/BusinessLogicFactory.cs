using Ninject;
using Ninject.Parameters;
using SYS.BLL.Domain;
using SYS.BLL.Domain.GAIA;
using SYS.BLL.Domain.Line;
using SYS.DAL.Base;
using System;

namespace SYS.BLL.Base
{
    public interface IBusinessLogicFactory : IDisposable
    {
        TLogic GetLogic<TLogic>();
    }
    public class BusinessLogicFactory : IBusinessLogicFactory
    {
        private IKernel _kernel;
        private readonly string RepositoryFactory = nameof(RepositoryFactory);

        public BusinessLogicFactory()
        {
            if (this._kernel == null)
            {
                this._kernel = new StandardKernel();
            }

            _kernel
                .Bind<IBusinessLogicFactory>().ToConstant(this).InSingletonScope();
            // example
            _kernel
                .Bind<IRegionLogic>().To<RegionLogic>().InSingletonScope().WithConstructorArgument(RepositoryFactory, context => null);
            // Domain
            _kernel
                .Bind<IDateTimeLogic>().To<DateTimeLogic>().InSingletonScope().WithConstructorArgument(RepositoryFactory, context => null);
            _kernel
                .Bind<IHttpContextStateLogic>().To<HttpContextStateLogic>().InSingletonScope().WithConstructorArgument(RepositoryFactory, context => null);
            _kernel
                .Bind<IMailLogic>().To<MailLogic>().InSingletonScope().WithConstructorArgument(RepositoryFactory, context => null);
            _kernel
                .Bind<IWebServiceLogic>().To<WebServiceLogic>().InSingletonScope().WithConstructorArgument(RepositoryFactory, context => null);
            // GAIA
            _kernel
                .Bind<IGAIALogic>().To<GAIALogic>().InSingletonScope().WithConstructorArgument(RepositoryFactory, context => null);            
            // Line
            _kernel
                .Bind<ILineBotLogic>().To<LineBotLogic>().InSingletonScope().WithConstructorArgument(RepositoryFactory, context => null);
            _kernel
                .Bind<ILineBotReplyLogic>().To<LineBotReplyLogic>().InSingletonScope().WithConstructorArgument(RepositoryFactory, context => null);
        }

        public void Dispose()
        {
            if (this._kernel != null)
            {
                this._kernel.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public TLogic GetLogic<TLogic>()
        {
            return _kernel.Get<TLogic>();
        }

        public TLogic GetLogic<TLogic>(IRepositoryFactory RepositoryFactory = null)
        {
            var inject = new IParameter[]
            {
                new ConstructorArgument(nameof(RepositoryFactory), RepositoryFactory),
            };

            return _kernel.Get<TLogic>(inject);
        }
    }
}

using System;
using System.Configuration;
using System.Web;
using Autofac;
using Autofac.Integration.Wcf;
using Bioinformatics.Buissness.Contracts;
using Bioinformatics.Buissness.Contracts.Ants;
using Bioinformatics.Buissness.Contracts.Graph;
using Bioinformatics.Buissness.Implementations;
using Bioinformatics.Buissness.Implementations.Ants;
using Bioinformatics.Buissness.Implementations.AntsStateManager;
using Bioinformatics.Buissness.Implementations.Graph;
using Bioinformatics.Buissness.Implementations.Resolver;
using Bioinformatics.Persistence.Interfaces;
using Bioinformatics.Persistence.Services;

namespace Bioinformatics.Buissness.Host
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // build and set container in application start
            var builder = new ContainerBuilder();
            //Persistence
            builder
                .RegisterType<ProteinFileRepository>()
                .As<IProteinRepository>()
                .UsingConstructor(() => new ProteinFileRepository(ConfigurationManager.AppSettings["ProteinFileRepositoryPath"]));

            builder
                .RegisterType<ProteinGraphFileRepository>()
                .As<IProteinGraphRepository>()
                .UsingConstructor(() => new ProteinGraphFileRepository(ConfigurationManager.AppSettings["ProteinGraphFileRepositoryPath"]));

            //Buisness
            builder.RegisterType<AntsCliqueResolver>().As<ICliqueResolver>();
            builder.RegisterType<AntsFeromonNodesInitializer>().As<IAntsFeromonNodesInitializer>();
            builder.RegisterType<EvaporatorFeromon>().As<IEvaporatorFeromon>();
            builder.RegisterType<ProteinGraphGenerator>().As<IProteinGraphGenerator>();
            builder.RegisterType<ProteinNodeGenerator>().As<IProteinNodeGenerator>();
            builder.RegisterType<RegexGenerator>().As<IRegexGenerator>();
            builder.RegisterType<AntsManager>().As<IAntsManager>();
            var container = builder.Build();
            AutofacHostFactory.Container = container;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
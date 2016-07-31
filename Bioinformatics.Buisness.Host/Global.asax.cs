using System;
using System.Configuration;
using System.Web;
using Autofac;
using Autofac.Integration.Wcf;
using Bioinformatics.Buisness.Contracts;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Contracts.Graph;
using Bioinformatics.Buisness.Implementations;
using Bioinformatics.Buisness.Implementations.Ants;
using Bioinformatics.Buisness.Implementations.AntsStateManager;
using Bioinformatics.Buisness.Implementations.Graph;
using Bioinformatics.Buisness.Implementations.Resolver;
using Bioinformatics.Persistence.Interfaces;
using Bioinformatics.Persistence.Services;

namespace Bioinformatics.Buisness.Host
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // build and set container in application start
            var builder = new ContainerBuilder();
            //Persistence
            builder
                .Register(c => new ProteinFileRepository(ConfigurationManager.AppSettings["ProteinFileRepositoryPath"]))
                .As<IProteinRepository>();
            builder
                .Register(
                    c =>
                        new ProteinGraphFileRepository(
                            ConfigurationManager.AppSettings["ProteinGraphFileRepositoryPath"]))
                .As<IProteinGraphRepository>();
            builder
                .Register(
                    c =>
                        new VerificationResultFileRepository(
                            ConfigurationManager.AppSettings["VerificationResultFileRepositoryPath"]))
                .As<IVerificationResultRepository>();

            //Buisness
            builder.RegisterType<AntsCliqueResolver>().As<ICliqueResolver>();
            builder.Register(a => new AntsFeromonNodesInitializer(1.0)).As<IAntsFeromonNodesInitializer>();
            builder.Register(a => new EvaporatorFeromon(0.999)).As<IEvaporatorFeromon>();
            builder.RegisterType<ProteinGraphGenerator>().As<IProteinGraphGenerator>();
            builder.RegisterType<ProteinNodeGenerator>().As<IProteinNodeGenerator>();
            builder.RegisterType<RegexGenerator>().As<IRegexGenerator>();
            builder.RegisterType<AntsManager>().As<IAntsManager>().SingleInstance();
            builder.RegisterType<ResultChecker>().As<IResultChecker>();
            AutofacHostFactory.Container = builder.Build();
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
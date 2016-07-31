using Autofac;
using Bioinformatics.Buisness.Contracts;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Contracts.Graph;
using Bioinformatics.Buisness.Implementations;
using Bioinformatics.Buisness.Implementations.Graph;
using Bioinformatics.Buisness.Implementations.Resolver;
using Bioinformatics.Persistence.Interfaces;
using Bioinformatics.Persistence.Services;

namespace Bioinformatics.Client.ConsoleApplication.DependencyResolver
{
    public class DependencyConfiguration
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            //Persistence
            builder.RegisterType<ProteinFileRepository>().As<IProteinGraphRepository>();
            builder.RegisterType<ProteinGraphFileRepository>().As<IProteinRepository>();

            //Buisness
            builder.RegisterType<AntsCliqueResolver>().As<ICliqueResolver>();
            builder.RegisterType<ProteinGraphFileRepository>().As<IAntsFeromonNodesInitializer>();
            builder.RegisterType<ProteinFileRepository>().As<IEvaporatorFeromon>();
            builder.RegisterType<ProteinGraphGenerator>().As<IProteinGraphGenerator>();
            builder.RegisterType<ProteinNodeGenerator>().As<IProteinNodeGenerator>();
            builder.RegisterType<RegexGenerator>().As<IRegexGenerator>();
            return builder.Build();
        }
    }
}
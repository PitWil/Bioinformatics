/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Bioinformatics.Client.BioinformaticsManagement"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/


using System.Configuration;
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
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Bioinformatics.Client.BioinformaticsManagement.ViewModel
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}


            //  var client =new  Services.AntsManagerClient().;
            if (ConfigurationManager.AppSettings["Test"].Equals("true"))
            {
                SimpleIoc.Default.Register<IProteinRepository>(
                    () => new ProteinFileRepository(ConfigurationManager.AppSettings["ProteinFileRepositoryPath"]));
                SimpleIoc.Default.Register<IProteinGraphRepository>(
                    () =>
                        new ProteinGraphFileRepository(
                            ConfigurationManager.AppSettings["ProteinGraphFileRepositoryPath"]));
                SimpleIoc.Default.Register<IVerificationResultRepository>(
                    () =>
                        new VerificationResultFileRepository(
                            ConfigurationManager.AppSettings["VerificationResultFileRepositoryPath"]));

                //Buisness
                SimpleIoc.Default.Register<ICliqueResolver, AntsCliqueResolver>();
                SimpleIoc.Default.Register<IAntsFeromonNodesInitializer>(() => new AntsFeromonNodesInitializer(1.0));
                SimpleIoc.Default.Register<IEvaporatorFeromon>(() => new EvaporatorFeromon(0.999));
                SimpleIoc.Default.Register<IProteinGraphGenerator, ProteinGraphGenerator>();
                SimpleIoc.Default.Register<IProteinNodeGenerator, ProteinNodeGenerator>();
                SimpleIoc.Default.Register<IRegexGenerator, RegexGenerator>();
                SimpleIoc.Default.Register<IAntsManager, AntsManager>();
                SimpleIoc.Default.Register<IResultChecker, ResultChecker>();
            }
            else
            {
                SimpleIoc.Default.Register<IAntsManager>(() => new Buisness.Proxies.AntsManager());
            }

            //      try
            //      {

            //     ((ICommunicationObject)client).Close();
            //    }
            //   catch
            //  {
            //      if (client != null)
            // //     {
            //        ((ICommunicationObject)client).Abort();
            //     }
            //  }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
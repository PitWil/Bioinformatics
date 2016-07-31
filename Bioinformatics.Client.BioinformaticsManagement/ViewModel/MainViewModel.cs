using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Bioinformatics.Buisness.Contracts.Ants;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Buisness.Models.Ants;
using Bioinformatics.Client.BioinformaticsManagement.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Bioinformatics.Client.BioinformaticsManagement.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private static readonly Random Ran = new Random((int) (DateTime.Now.Ticks*DateTime.Now.Ticks));
        private readonly IAntsManager _antsManager;
        private readonly BackgroundWorker _worker = new BackgroundWorker();

        private int _antsCount;
        private int _antsExplorerCount;

        private List<ColonyOfAnts> _colonies;
        private List<ProteinSequence> _counterExamples;
        private int _diversificationTime;
        private double _feromonAnts;
        private double _feromonAntsExplorer;

        private string _graphBuildPercentage;
        private double _graphBuildStat;
        private int _interationCount;
        private List<ProteinSequence> _positiveExamples;
        private List<ProteinSequence> _selectedCounterExamples;
        private List<ProteinSequence> _selectedPositiveExamples;
        private int _spanAnthills;

        public MainViewModel(IAntsManager antsManager)
        {
            _antsManager = antsManager;

            var a = _antsManager.GetPositiveNodes();

            var list = new List<ProteinSequence>();
            a.Data.ForEach(Z => list.Add(new ProteinSequence(Z.Value)));
            PositiveExamples = list;

            var b = _antsManager.GetNegativeNodes();

            var list2 = new List<ProteinSequence>();
            b.Data.ForEach(Z => list2.Add(new ProteinSequence(Z.Value)));
            CounterExamples = list2;
            InitCommand();
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            GraphBuildStat = 0.0;
        }

        public string GraphBuildPercentage
        {
            get { return _graphBuildPercentage; }
            set
            {
                _graphBuildPercentage = value;
                RaisePropertyChanged("GraphBuildPercentage");
            }
        }

        public List<ProteinSequence> PositiveExamples
        {
            get { return _positiveExamples; }
            set
            {
                _positiveExamples = value;
                RaisePropertyChanged("PositiveExamples");
            }
        }

        public List<ProteinSequence> CounterExamples
        {
            get { return _counterExamples; }
            set
            {
                _counterExamples = value;
                RaisePropertyChanged("CounterExamples");
            }
        }

        public List<ProteinSequence> SelectedPositiveExamples
        {
            get { return _selectedPositiveExamples; }
            set
            {
                _selectedPositiveExamples = value;
                RaisePropertyChanged("SelectedPositiveExamples");
            }
        }

        public List<ProteinSequence> SelectedCounterExamples
        {
            get { return _selectedCounterExamples; }
            set
            {
                _selectedCounterExamples = value;
                RaisePropertyChanged("SelectedCounterExamples");
            }
        }

        public List<ColonyOfAnts> Colonies
        {
            get { return _colonies; }
            set
            {
                _colonies = value;
                RaisePropertyChanged("Colonies");
            }
        }

        public int AntsCount
        {
            get { return _antsCount; }
            set
            {
                _antsCount = value;
                RaisePropertyChanged("AntsCount");
            }
        }

        public int AntsExplorerCount
        {
            get { return _antsExplorerCount; }
            set
            {
                _antsExplorerCount = value;
                RaisePropertyChanged("AntsExplorerCount");
            }
        }

        public int SpanAnthills
        {
            get { return _spanAnthills; }
            set
            {
                _spanAnthills = value;
                RaisePropertyChanged("SpanAnthills");
            }
        }

        public double FeromonAnts
        {
            get { return _feromonAnts; }
            set
            {
                _feromonAnts = value;
                RaisePropertyChanged("FeromonAnts");
            }
        }

        public double FeromonAntsExplorer
        {
            get { return _feromonAntsExplorer; }
            set
            {
                _feromonAntsExplorer = value;
                RaisePropertyChanged("FeromonAntsExplorer");
            }
        }

        public int DiversificationTime
        {
            get { return _diversificationTime; }
            set
            {
                _diversificationTime = value;
                RaisePropertyChanged("DiversificationTime");
            }
        }

        public int InterationCount
        {
            get { return _interationCount; }
            set
            {
                _interationCount = value;
                RaisePropertyChanged("InterationCount");
            }
        }

        public double GraphBuildStat
        {
            get { return _graphBuildStat; }
            set
            {
                _graphBuildStat = value;
                GraphBuildPercentage = $"{_graphBuildStat,14:P1}";
                RaisePropertyChanged("GraphBuildStat");
            }
        }

        public bool PositiveExamplesButton { get; private set; }

        public bool CounterExamplesButton { get; private set; }

        public RelayCommand SelectCounterExamples { get; private set; }

        public RelayCommand SelectPositiveExamples { get; private set; }

        public RelayCommand ChooseProteins { get; private set; }

        public RelayCommand RemoveProteins { get; private set; }

        public RelayCommand AddColony { get; private set; }

        public RelayCommand SetSettings { get; private set; }

        public RelayCommand Start { get; private set; }

        public RelayCommand Pause { get; private set; }

        public RelayCommand Stop { get; private set; }

        public RelayCommand GetGraphStatus { get; private set; }

        public RelayCommand SelectAllPositiveExamples { get; private set; }

        public RelayCommand DeselectAllPositiveExamples { get; private set; }

        public RelayCommand SelectAllCounterExamples { get; private set; }

        public RelayCommand DeselectAllCounterExamples { get; private set; }

        public RelayCommand SelectAllSelectedPositiveExamples { get; private set; }

        public RelayCommand DeselectAllSelectedPositiveExamples { get; private set; }

        public RelayCommand SelectAllSelectedCounterExamples { get; private set; }

        public RelayCommand DeselectAllSelectedCounterExamples { get; private set; }

        public double GraphSearchStat
        {
            get { return _graphBuildStat; }
            set
            {
                _graphBuildStat = value;
                //  GraphBuildPercentage = $"{_graphBuildStat,14:P1}";
                RaisePropertyChanged("GraphSearchStat");
            }
        }

        private void InitCommand()
        {
            if (_antsManager.GetManagerState() != ManagerState.Ready)
            {
                CounterExamplesButton = false;
                PositiveExamplesButton = false;
            }
            else
            {
                CounterExamplesButton = true;
                PositiveExamplesButton = true;
            }

            SelectCounterExamples = new RelayCommand(SelectCounterExamplesExecute, () => CounterExamplesButton);
            SelectPositiveExamples = new RelayCommand(SelectPositiveExamplesExecute, () => PositiveExamplesButton);
            ChooseProteins = new RelayCommand(ChooseProteinExecute, () => PositiveExamplesButton);
            RemoveProteins = new RelayCommand(RemoveProteinExecute, () => PositiveExamplesButton);
            AddColony = new RelayCommand(AddColonyExecute, () => PositiveExamplesButton);
            SetSettings = new RelayCommand(SetSettingsExecute, () => PositiveExamplesButton);
            Start = new RelayCommand(StartExecute, () => PositiveExamplesButton);
            Stop = new RelayCommand(StopExecute, () => PositiveExamplesButton);
            Pause = new RelayCommand(PauseExecute, () => PositiveExamplesButton);
            SelectAllPositiveExamples = new RelayCommand(SelectAllPositiveExamplesExecute, () => PositiveExamplesButton);
            DeselectAllPositiveExamples = new RelayCommand(DeselectAllPositiveExamplesExecute,
                () => PositiveExamplesButton);
            SelectAllCounterExamples = new RelayCommand(SelectAllCounterExamplesExecute, () => PositiveExamplesButton);
            DeselectAllCounterExamples = new RelayCommand(DeselectAllCounterExamplesExecute,
                () => PositiveExamplesButton);
            SelectAllSelectedPositiveExamples = new RelayCommand(SelectAllSelectedPositiveExamplesExecute,
                () => PositiveExamplesButton);
            DeselectAllSelectedPositiveExamples = new RelayCommand(DeselectAllSelectedPositiveExamplesExecute,
                () => PositiveExamplesButton);
            SelectAllSelectedCounterExamples = new RelayCommand(SelectAllSelectedCounterExamplesExecute,
                () => PositiveExamplesButton);
            DeselectAllSelectedCounterExamples = new RelayCommand(DeselectAllSelectedCounterExamplesExecute,
                () => PositiveExamplesButton);

            GetGraphStatus = new RelayCommand(GetGraphStatusExecute);
        }

        public void GetGraphStatusExecute()
        {
            GraphBuildStat = _antsManager.GraphBuildStat().Percentage;
            var z = _antsManager.GraphSearchStat();
            GraphSearchStat = z.Percentage;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GetGraphStatusExecute();
            _worker.RunWorkerAsync();
        }

        public void StartExecute()
        {
            if (_antsManager.Start().Successed)
                _worker.RunWorkerAsync();
        }

        public void StopExecute()
        {
            _antsManager.Finish();
        }

        public void PauseExecute()
        {
            _antsManager.Pause();
        }

        public void SetSettingsExecute()
        {
            var settings = new AntsConfigurationModel
            {
                Colonies = Colonies,
                CounterNodes = new List<ProteinNode>()
            };
            CounterExamples.ForEach(z =>
            {
                if (z.IsChecked)
                {
                    settings.CounterNodes.Add(new ProteinNode(z.Sequence));
                }
            });
            settings.PositiveNodes = new List<ProteinNode>();
            PositiveExamples.ForEach(z =>
            {
                if (z.IsChecked)
                {
                    settings.PositiveNodes.Add(new ProteinNode(z.Sequence));
                }
            }
                );
            settings.PositiveNodesToVerif = new List<ProteinNode>();
            SelectedPositiveExamples.ForEach(z =>
            {
                if (z.IsChecked)
                {
                    settings.PositiveNodesToVerif.Add(new ProteinNode(z.Sequence));
                }
            });
            settings.NegativeNodesToVerif = new List<ProteinNode>();
            SelectedCounterExamples.ForEach(z =>
            {
                if (z.IsChecked)
                {
                    settings.NegativeNodesToVerif.Add(new ProteinNode(z.Sequence));
                }
            });
            _antsManager.SetAntsSettings(settings);
        }

        public void AddColonyExecute()
        {
            if (Colonies == null)
            {
                Colonies = new List<ColonyOfAnts>();
            }
            var ants = new List<Ant>();
            for (var i = 0; i < AntsCount; i++)
            {
                ants.Add(new Ant(FeromonAnts));
            }
            for (var i = 0; i < AntsExplorerCount; i++)
            {
                ants.Add(new AntFinder(FeromonAntsExplorer));
            }

            Colonies.Add(new ColonyOfAnts
            {
                Ants = ants,
                AntsHillSize = SpanAnthills,
                DiversificationTime = DiversificationTime,
                InterationCount = InterationCount
            });
            Colonies = Colonies.ToList();
        }

        public void RemoveProteinExecute()
        {
            if (CounterExamples == null)
            {
                CounterExamples = new List<ProteinSequence>();
            }
            if (PositiveExamples == null)
            {
                PositiveExamples = new List<ProteinSequence>();
            }
            SelectedPositiveExamples.ForEach(z =>
            {
                if (z.IsChecked)
                {
                    PositiveExamples.Add(z);
                }
            });
            PositiveExamples = PositiveExamples.ToList();

            SelectedPositiveExamples = SelectedPositiveExamples.Where(z => !z.IsChecked).ToList();

            SelectedCounterExamples.ForEach(z =>
            {
                if (z.IsChecked)
                {
                    CounterExamples.Add(z);
                }
            });
            CounterExamples = CounterExamples.ToList();

            SelectedCounterExamples = SelectedCounterExamples.Where(z => !z.IsChecked).ToList();
        }

        public void ChooseProteinExecute()
        {
            if (SelectedCounterExamples == null)
            {
                SelectedCounterExamples = new List<ProteinSequence>();
            }
            if (SelectedPositiveExamples == null)
            {
                SelectedPositiveExamples = new List<ProteinSequence>();
            }
            PositiveExamples.ForEach(z =>
            {
                if (z.IsChecked)
                {
                    SelectedPositiveExamples.Add(z);
                }
            });
            SelectedPositiveExamples = SelectedPositiveExamples.ToList();

            PositiveExamples = PositiveExamples.Where(z => !z.IsChecked).ToList();

            CounterExamples.ForEach(z =>
            {
                if (z.IsChecked)
                {
                    SelectedCounterExamples.Add(z);
                }
            });
            SelectedCounterExamples = SelectedCounterExamples.ToList();

            CounterExamples = CounterExamples.Where(z => !z.IsChecked).ToList();
        }

        public void SelectCounterExamplesExecute()
        {
            CounterExamples.ForEach(a => a.IsChecked = false);
            var count = CounterExamples.Count/3;
            for (var i = 0; i < count; i++)
            {
                int index;
                do
                {
                    index = Ran.Next(0, CounterExamples.Count - 1);
                } while (CounterExamples[index].IsChecked);
                CounterExamples[index].IsChecked = true;
            }
        }

        public void SelectPositiveExamplesExecute()
        {
            PositiveExamples.ForEach(a => a.IsChecked = false);
            var count = PositiveExamples.Count/3;
            for (var i = 0; i < count; i++)
            {
                int index;
                do
                {
                    index = Ran.Next(0, PositiveExamples.Count - 1);
                } while (PositiveExamples[index].IsChecked);
                PositiveExamples[index].IsChecked = true;
            }
        }

        public void SelectAllPositiveExamplesExecute()
        {
            PositiveExamples.ForEach(a => a.IsChecked = true);
        }

        public void DeselectAllPositiveExamplesExecute()
        {
            PositiveExamples.ForEach(a => a.IsChecked = false);
        }

        public void SelectAllCounterExamplesExecute()
        {
            CounterExamples.ForEach(a => a.IsChecked = true);
        }

        public void DeselectAllCounterExamplesExecute()
        {
            CounterExamples.ForEach(a => a.IsChecked = false);
        }

        public void SelectAllSelectedPositiveExamplesExecute()
        {
            SelectedPositiveExamples.ForEach(a => a.IsChecked = true);
        }

        public void DeselectAllSelectedPositiveExamplesExecute()
        {
            SelectedPositiveExamples.ForEach(a => a.IsChecked = false);
        }

        public void SelectAllSelectedCounterExamplesExecute()
        {
            SelectedCounterExamples.ForEach(a => a.IsChecked = true);
        }

        public void DeselectAllSelectedCounterExamplesExecute()
        {
            SelectedCounterExamples.ForEach(a => a.IsChecked = false);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Caliburn.Micro.Contrib.Demo;
using Caliburn.Micro.Contrib.Demo.ViewModels;

namespace Caliburn.Micro.Contrib.Demo
{
    public class CategorySamples : PropertyChangedBase
    {
        public SampleCategory Category { get; private set; }
        public IObservableCollection<ISample> Samples { get; private set; }

        public CategorySamples(SampleCategory category, IEnumerable<ISample> samples)
        {
            Category = category;
            Samples = new BindableCollection<ISample>(samples.OrderBy(x => x.DisplayName));
        }
    }

    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<ISample>, IShell
    {
        public IObservableCollection<CategorySamples> SamplesByCategory { get; private set; }

        public IObservableCollection<string> LogItems { get; private set; }

        public ShellViewModel()
            :this(new ISample[0])
        {
            
        }

        [ImportingConstructor]
        public ShellViewModel([ImportMany]IEnumerable<ISample> samples)
        {
            DisplayName = "CMContrib WPF Demo";
            LogItems = new BindableCollection<string>();
            SamplesByCategory = new BindableCollection<CategorySamples>();
            foreach (var samplesByCategory in samples
                .GroupBy(x => x.Category)
                .OrderBy(x => x.Key.ToString()))
            {
                SamplesByCategory.Add(new CategorySamples(samplesByCategory.Key,
                                                          new BindableCollection<ISample>(samplesByCategory)));
            }
        }

        public void Log(string message)
        {
            LogItems.Insert(0,string.Format("[{0}] {1}", DateTime.Now.ToLongTimeString(), message));
        }

        public void ClearLog()
        {
            LogItems.Clear();
        }
    }
}
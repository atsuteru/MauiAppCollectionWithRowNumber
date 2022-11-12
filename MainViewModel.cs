using System.Collections.ObjectModel;

namespace MauiAppCollectionWithRowNumber
{
    public class MainViewModel
    {
        public ObservableCollection<Person> Persons { get; }

        public MainViewModel()
        {
            Persons = new ObservableCollection<Person>();
            // sample data
            new string[] {
                "James", "Robert", "John", "Michael", "David", "William", "Richard", "Joseph", "Thomas", "Charles",
                "Christopher", "Daniel", "Matthew", "Anthony", "Mark", "Donald", "Steven", "Paul", "Andrew", "Joshua",
            }
                .Select(x => new Person() { Name = x })
                .ToList()
                .ForEach(Persons.Add);
        }
    }
}

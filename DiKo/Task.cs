using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DiKo
{
    public class Tasks : ObservableCollection<Task>
    {
        // Creating the Tasks collection in this way enables data binding from XAML.
    }
}
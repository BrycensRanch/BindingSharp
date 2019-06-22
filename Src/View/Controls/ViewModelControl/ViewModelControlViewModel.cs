using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MVVM
{
    public class ViewModelControlViewModel : IViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Type View => typeof(ViewModelControl);

        private Command myCommand;
        public ICommand MyCommand
        {
            get { return myCommand; }    
        }

        private string label = "MEIN LABEL NEU";
        public string Label
        {
            get { return label; }
            set
            {
                if(label != value)
                {
                    label = value;
                    Console.WriteLine("VIEWMODEL: " + value);

                    label = value + value;
                    Console.WriteLine("CHanged " + label);
                    OnPropertyChanged();
                }

            }
        }

        public ViewModelControlViewModel()
        {
            myCommand = new Command((o) => ButtonAction());
        }

        private void ButtonAction()
        {
            Console.WriteLine("ViewModel: Button clicked");
            Label = "Test";
            myCommand.SetCanExecute(false);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
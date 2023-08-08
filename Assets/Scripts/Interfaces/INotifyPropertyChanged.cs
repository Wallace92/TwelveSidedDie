using System.ComponentModel;

public interface INotifyPropertyChanged
{
    event PropertyChangedEventHandler PropertyChange;
}
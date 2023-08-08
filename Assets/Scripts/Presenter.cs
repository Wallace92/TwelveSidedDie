using System.ComponentModel;
using UnityEngine;

public abstract class Presenter<T> : MonoBehaviour where T : Model
{
    protected T Model;

    protected void Awake()
    {
        Model = GetComponent<T>();
        Model.PropertyChange += OnPropertyChange;
    }

    protected abstract void OnPropertyChange(object sender, PropertyChangedEventArgs e);
    
    private void OnDestroy() => Model.PropertyChange -= OnPropertyChange;
}
using System.ComponentModel;

namespace WebpWhack
{
    public abstract class PropertyChangedNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged( string propertyName )
        {
            var propertyChanged = PropertyChanged;
            if( propertyChanged == null ) return;

            propertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}

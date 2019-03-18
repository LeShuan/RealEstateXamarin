using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RealEstateXamarin.Models
{
   public class IsBusyObject : INotifyPropertyChanged
        {


            public IsBusyObject()
            {
            }

            private bool busy = false;

            public bool IsBusy
            {
                get { return busy; }
                set
                {
                    if (busy == value)
                        return;

                    busy = value;
      
                    OnPropertyChanged("IsBusy");
                    
                }
            }

        

            #region INotifyPropertyChanged implementation

            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

            }

            #endregion
        
    }
}

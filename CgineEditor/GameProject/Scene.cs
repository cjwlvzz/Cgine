using System;
using System.Collections.Generic;
using System.Text;

namespace CgineEditor.GameProject
{
    class Scene :ViewModelBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(Name);
                }
            }
        }
    }
}

using System;

namespace SkipUnsentProperties
{
    public class DataTransferObject : BaseDataTransferObject
    {
        private string _one;
        public string One
        {
            get => _one;
            set => SetValue(nameof(One), ref _one, value);
        }

        private string _two;
        public string Two
        {
            get => _two;
            set => SetValue(nameof(Two), ref _two, value);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Models
{
    public class ValueModel<T>
    {
        private T _value;

        public T GetValue()
        {
            return _value;
        }

        public void SetValue(T value)
        {
            _value = value;
        }
    }
}

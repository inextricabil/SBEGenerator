using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Models
{
    public class ValueModel<T>
    {
        private T _value;
        private string _op;

        public ValueModel(T v, string op)
        {
            _value = v;
            _op = op;
        }

        public ValueModel(T v)
        {
            _value = v;
        }

        public ValueModel()
        {

        }

        public T GetValue()
        {
            return _value;
        }

        public string GetOperator()
        {
            return _op;
        }

        public void SetOperator(string value)
        {
            _op = value;
        }

        public void SetValue(T value)
        {
            _value = value;
        }
    }
}

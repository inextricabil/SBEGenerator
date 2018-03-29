using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Models
{
    public class RangedModel
    {
        private double _minValue;
        private double _maxValue;

        public double GetMinValue()
        {
            return _minValue;
        }

        public void SetMinValue(double minValue)
        {
            _minValue = minValue;
        }

        public double GetMaxValue()
        {
            return _maxValue;
        }

        public void SetMaxValue(double maxValue)
        {
            _maxValue = maxValue;
        }
    }
}

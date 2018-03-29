using Generator.Models;
using System;

namespace Generator
{
    public class Subscription
    {
        private Guid _subId;

        private ValueModel<string> _companies;
        private ValueModel<string> _dates;
        private ValueModel<double> _drop;
        private ValueModel<int> _value;
        private ValueModel<double> _variation;

        public Subscription(Guid subId)
        {
            _subId = subId;
        }

        public void SetCompany(SubscriptionTouple<string> companyTouple)
        {
            _companies = new ValueModel<string>(companyTouple.GetValue());
        }

        public void SetDrop(SubscriptionTouple<double> dropTouple)
        {
            _drop = new ValueModel<double>(dropTouple.GetValue());
        }

        public void SetValue(SubscriptionTouple<int> valueTouple)
        {
            _value = new ValueModel<int>(valueTouple.GetValue());
        }

        public void SetVariation(SubscriptionTouple<double> variationTouple)
        {
            _variation = new ValueModel<double>(variationTouple.GetValue());
        }

        public void SetDate(SubscriptionTouple<string> dateTouple)
        {
            _dates = new ValueModel<string>(dateTouple.GetValue());
        }

        public ValueModel<string> GetCompany()
        {
            return _companies;
        }

        public string GetCompanyValue()
        {
            return _companies.GetValue();
        }

        public ValueModel<string> GetDate()
        {
            return _dates;
        }

        public string GetDateValue()
        {
            return _dates.GetValue();
        }

        public ValueModel<double> GetDrop()
        {
            return _drop;
        }

        public double GetDropValue()
        {
            return _drop.GetValue();
        }

        public ValueModel<int> GetValues()
        {
            return _value;
        }

        public int GetValuesValue()
        {
            return _value.GetValue();
        }

        public ValueModel<double> GetVariation()
        {
            return _variation;
        }

        public double GetVariationValue()
        {
            return _variation.GetValue();
        }

        public int CheckNullValues()
        {
            int count = 0;
            if (string.IsNullOrWhiteSpace(_companies.GetValue()))
                count++;
            if (string.IsNullOrWhiteSpace(_dates.GetValue()))
                count++;
            if (_drop.GetValue() == 0)
                count++;
            if (_variation.GetValue() == 0)
                count++;
            if (_value.GetValue() == 0)
                count++;

            return count;
        }
    }
}
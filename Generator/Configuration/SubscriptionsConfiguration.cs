using Generator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Generator.Configuration
{
    public class SubscriptionsConfiguration
    {
        private string _outputFileName;

        private ValueModel<int> _messageNumber;
        private ValueModel<int> _value;
        private ValueModel<int> _drop;
        private ValueModel<double> _variation;
        private ValueModel<int> _companies;
        private ValueModel<int> _dates;
        private List<string> _operators;
        private ValueModel<string> _minField;
        private ValueModel<string> _minOperator;
        private ValueModel<int> _minOperatorWeight;

        public SubscriptionsConfiguration()
        {
            _outputFileName = "subscriptions.txt";
            _messageNumber = new ValueModel<int>();
            _value = new ValueModel<int>();
            _drop = new ValueModel<int>();
            _variation = new ValueModel<double>();
            _companies = new ValueModel<int>();
            _dates = new ValueModel<int>();
            SetOperators(new List<string>());
            _minField = new ValueModel<string>();
            _minOperator = new ValueModel<string>();
            _minOperatorWeight = new ValueModel<int>();
        }

        public String GetOutputFileName()
        {
            return _outputFileName;
        }

        public ValueModel<int> GetValue()
        {
            return _value;
        }

        public void SetValue(ValueModel<int> value)
        {
            _value = value;
        }

        public ValueModel<int> GetMessageNumber()
        {
            return _messageNumber;
        }

        public void SetMessageNumber(ValueModel<int> messageNumber)
        {
            _messageNumber = messageNumber;
        }

        public ValueModel<int> GetDrop()
        {
            return _drop;
        }

        public void SetDrop(ValueModel<int> drop)
        {
            _drop = drop;
        }

        public ValueModel<double> GetVariation()
        {
            return _variation;
        }

        public void SetVariation(ValueModel<double> variation)
        {
            _variation = variation;
        }

        public ValueModel<int> GetCompanies()
        {
            return _companies;
        }

        public void SetCompanies(ValueModel<int> companies)
        {
            _companies = companies;
        }

        public ValueModel<int> GetDates()
        {
            return _dates;
        }

        public void SetDates(ValueModel<int> dates)
        {
            _dates = dates;
        }

        public List<String> GetOperators()
        {
            return _operators;
        }

        public void SetOperators(List<string> operators)
        {
            _operators = operators;
        }

        public ValueModel<String> GetMinField()
        {
            return _minField;
        }

        public void SetMinField(ValueModel<string> minField)
        {
            _minField = minField;
        }

        public ValueModel<String> GetMinOperator()
        {
            return _minOperator;
        }

        public void SetMinOperator(ValueModel<string> minOperator)
        {
            _minOperator = minOperator;
        }

        public ValueModel<int> GetMinOperatorWeight()
        {
            return _minOperatorWeight;
        }

        public void SetMinOperatorWeight(ValueModel<int> minOperatorWeight)
        {
            _minOperatorWeight = minOperatorWeight;
        }

        public double GetFieldWeight(Field field)
        {
            if (field == Field.company)
            {
                return _companies.GetValue();
            }

            if (field == Field.drop)
            {
                return _drop.GetValue();
            }

            if (field == Field.value)
            {
                return _value.GetValue();
            }

            if (field == Field.variation)
            {
                return _variation.GetValue();
            }

            if (field == Field.date)
            {
                return _dates.GetValue();
            }

            return 0;
        }

        public Field MinField()
        {
            string field = _minField.GetValue();

            if (field.Equals("company"))
            {
                return Field.company;
            }

            if (field.Equals("drop"))
            {
                return Field.drop;
            }

            if (field.Equals("value"))
            {
                return Field.value;
            }

            if (field.Equals("variation"))
            {
                return Field.variation;
            }

            if (field.Equals("date"))
            {
                return Field.date;
            }

            return Field.company;
        }

        public void LoadConfiguration()
        {
            LoadMessageNumber(_messageNumber);
 
             LoadIntegerValue( "value", _value);
             LoadIntegerValue( "drop", _drop);
             LoadDoubleValue( "variation", _variation);
             LoadIntegerValue( "companies", _companies);
             LoadIntegerValue( "dates", _dates);
             LoadStringValue("min-field", _minField);
             LoadStringValue("min-op", _minOperator);
             LoadIntegerValue( "min-freq-op", _minOperatorWeight);
             LoadOperators();
        }

        private void LoadDoubleValue(string key, ValueModel<double> model)
        {
            var value = ConfigurationManager.AppSettings[key];
            model.SetValue(double.Parse(value));
        }

        private static void LoadMessageNumber(ValueModel<int> messageNumber)
        {
            var configurationManagerMessageNumber = ConfigurationManager.AppSettings["_messageNumber"];
            messageNumber.SetValue(int.Parse(configurationManagerMessageNumber));
        }

        private static void LoadIntegerValue(string key, ValueModel<int> model)
        {
            var value = ConfigurationManager.AppSettings[key];
            model.SetValue(int.Parse(value));
        }

        private void LoadStringValue(string key, ValueModel<string> model)
        {
            var value = ConfigurationManager.AppSettings[key];
            model.SetValue(value);
        }

        private void LoadOperators()
        {
            var value = ConfigurationManager.AppSettings["operators"];

            var operators = value.Split(",");

            foreach (var op in operators)
            {
                _operators.Add(op);
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Generator.Models;

namespace Generator
{
    public class PublicationsConfiguration
    {
        private readonly string _outputFileName;
        private readonly string _dateStringFormat;

        private ValueModel<int> _messageNumber;

        private RangedModel _drop;
        private RangedModel _value;
        private RangedModel _variation;

        private List<string> _companies;
        private List<string> _stringDates;
        private List<DateTime> _dates;

        public PublicationsConfiguration()
        {
            _outputFileName = "publications.txt";
            _dateStringFormat = "yyyy-MM-dd";

            _messageNumber = new ValueModel<int>();

            _drop = new RangedModel();
            _value = new RangedModel();
            _variation = new RangedModel();

            _companies = new List<string>();
            _stringDates = new List<string>();
            _dates = new List<DateTime>();
        }

        public string GetOutputFileName()
        {
            return _outputFileName;
        }

        public ValueModel<int> GetMessageNumber()
        {
            return _messageNumber;
        }

        public void SetNumberOfMessages(ValueModel<int> messageNumber)
        {
            _messageNumber = messageNumber;
        }

        public RangedModel GetDrop()
        {
            return _drop;
        }

        public void SetDrop(RangedModel drop)
        {
            _drop = drop;
        }

        public RangedModel GetValue()
        {
            return _value;
        }

        public void SetValue(RangedModel value)
        {
            _value = value;
        }

        public RangedModel GetVariation()
        {
            return _variation;
        }

        public void SetVariation(RangedModel variation)
        {
            _variation = variation;
        }

        public List<string> GetCompanies()
        {
            return _companies;
        }

        public void SetCompanies(List<string> companies)
        {
            _companies = companies;
        }

        public List<DateTime> GetDates()
        {
            return _dates;
        }

        public void SetDates(List<DateTime> dates)
        {
            _dates = dates;
        }

        public List<string> GetStringDates()
        {
            return _stringDates;
        }

        public void SetStringDates(List<string> stringDates)
        {
            _stringDates = stringDates;
        }

        public void LoadConfiguration()
        {
            LoadMessageNumber(_messageNumber);
            LoadRangedValue("Publications_min-drop", "Publications_max-drop", _drop);
            LoadRangedValue("Publications_min-value", "Publications_max-value", _value);
            LoadRangedValue("Publications_min-variation", "Publications_max-variation", _variation);
            LoadCompanies();
            LoadDates();
        }

        private static void LoadMessageNumber(ValueModel<int> messageNumber)
        {
            var configurationManagerMessageNumber = ConfigurationManager.AppSettings["Publications_messageNumber"];
            messageNumber.SetValue(int.Parse(configurationManagerMessageNumber));
        }

        private static void LoadRangedValue(string minKey, string maxKey, RangedModel model)
        {
            var minValue = ConfigurationManager.AppSettings[minKey];
            var maxValue = ConfigurationManager.AppSettings[maxKey];

            model.SetMinValue(double.Parse(minValue));
            model.SetMaxValue(double.Parse(maxValue));
        }

        private void LoadCompanies()
        {
            var companies = ConfigurationManager.AppSettings["Publications_companies"].Split(",");

            foreach (var company in companies)
            {
                _companies.Add(company);
            }
        }

        private void LoadDates()
        {
            var configurationManagerDates = ConfigurationManager.AppSettings["Publications_dates"].Split(",");
            foreach (var date in configurationManagerDates)
            {
                _stringDates.Add(date);
                _dates.Add(DateTime.ParseExact(date, _dateStringFormat, CultureInfo.InvariantCulture));
            }
        }
    }
}


using Generator.Models;
using System;

namespace Generator
{
    public class Publication
    {
        private ValueModel<string> _companies;
        private ValueModel<string> _dates;
        private ValueModel<double> _drop;
        private ValueModel<double> _value;
        private ValueModel<double> _variation;

        public void SetCompany(PublicationTouple<string> companyTouple)
        {
            _companies = new ValueModel<string>(companyTouple.GetValue());
        }

        public void SetDrop(PublicationTouple<double> dropTouple)
        {
            _drop = new ValueModel<double>(dropTouple.GetValue());
        }

        public void SetValue(PublicationTouple<double> valueTouple)
        {
            _value = new ValueModel<double>(valueTouple.GetValue());
        }

        public void SetVariation(PublicationTouple<double> variationTouple)
        {
            _variation = new ValueModel<double>(variationTouple.GetValue());
        }

        public void SetDate(PublicationTouple<string> dateTouple)
        {
            _dates = new ValueModel<string>(dateTouple.GetValue());
        }
    }
}
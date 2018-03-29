using Generator.Configuration;
using Generator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generator
{

    public class GeneratorPublisher
    {
        private string _file;
        private int _messagesNumber;
        private PublicationsConfiguration _publicationsConfig;
        private List<Publication> _publications;

        public GeneratorPublisher(string file)
        {
            _file = file;
            _publicationsConfig = new PublicationsConfiguration();
            _publications = new List<Publication>();
        }

        public void Generate()
        {
            Initialize();

            Console.WriteLine("[Publisher-{0}] Generating publications...\n", _file);

            GeneratePublications();

            Console.WriteLine("[Publisher-{0}] Generation finished!\n", _file);
        }

        private void Initialize()
        {
            try
            {
                _publicationsConfig.LoadConfiguration();
                _messagesNumber = _publicationsConfig.GetMessageNumber().GetValue();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

        private void GeneratePublications()
        {
            for (int i = 0; i < _messagesNumber; ++i)
            {
                Publication publication = CreatePublication();

                _publications.Add(publication);
            }

            WritePublicationsToFile();
        }
        
        private Publication CreatePublication()
        {
            Publication publication = new Publication();

            // company
            PublicationTouple<String> companyTouple = (PublicationTouple<String>)GetPublicationTouple(Field.company, false);
            publication.SetCompany(companyTouple);

            // drop
            PublicationTouple<Double> dropTouple = (PublicationTouple<Double>)GetPublicationTouple(Field.drop, true);
            publication.SetDrop(dropTouple);

            // value
            PublicationTouple<Double> valueTouple = (PublicationTouple<Double>)GetPublicationTouple(Field.value, true);
            publication.SetValue(valueTouple);

            // variation
            PublicationTouple<Double> variationTouple = (PublicationTouple<Double>)GetPublicationTouple(Field.variation, true);
            publication.SetVariation(variationTouple);

            // date
            PublicationTouple<String> dateTouple = (PublicationTouple<String>)GetPublicationTouple(Field.date, false);
            publication.SetDate(dateTouple);

            return publication;
        }

        private Object GetPublicationTouple(Field field, bool isRanged)
        {
            if (!isRanged)
            {
                return GetPublicationToupleFromSet(field);
            }

            return GetPublicationToupleFromRange(field);
        }

        private Object GetPublicationToupleFromSet(Field field)
        {
            List<string> setValues = new List<string>();

            if (field == Field.company)
            {
                setValues = _publicationsConfig.GetCompanies();
            }

            if (field == Field.date)
            {
                setValues = _publicationsConfig.GetStringDates();
            }

            String value = GetRandom(setValues);

            return new PublicationTouple<string>(field, value);
        }

        private Object GetPublicationToupleFromRange(Field field)
        {
            RangedModel model = new RangedModel();

            if (field == Field.drop)
            {
                model = _publicationsConfig.GetDrop();
            }

            if (field == Field.value)
            {
                model = _publicationsConfig.GetValue();
            }

            if (field == Field.variation)
            {
                model = _publicationsConfig.GetVariation();
            }

            double value = GetRandom(model.GetMinValue(), model.GetMaxValue());

            return new PublicationTouple<Double>(field, value);
        }

        private void WritePublicationsToFile()
        {

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(_file))
            {
                foreach (var publication in _publications)
                {
                    file.WriteLine(publication.ToString());
                }
            }
        }

        private String GetRandom(List<string> list)
        {
            return list.PickRandom();
        }

        private double GetRandom(double rangeMin, double rangeMax)
        {
            Random random = new Random();

            double value = rangeMin + (rangeMax - rangeMin) * random.NextDouble();

            String result = String.Format("%.2f", value);

            return double.Parse(result);
        }
    }
}

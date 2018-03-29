using Generator.Configuration;
using Generator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generator
{
    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }

    public class GeneratorSubscriber
    {
        private string _file;
        private int _messagesNumber;
        private Guid _subId;
        private PublicationsConfiguration _publicationsConfig;
        private SubscriptionsConfiguration _subscriptionsConfig;
        private List<Subscription> _subscriptions;

        public GeneratorSubscriber(Guid id, string file)
        {
            _file = file;
            _subId = id;
            _subscriptions = new List<Subscription>();
            _publicationsConfig = new PublicationsConfiguration();
            _subscriptionsConfig = new SubscriptionsConfiguration();
        }

        public Guid GetSubId()
        {
            return _subId;
        }

        public void SetSubId(Guid subId)
        {
            _subId = subId;
        }

        public void Generate()
        {
            Initialize();
            
            Console.WriteLine("[Subscriber-{0}] Generating subscriptions...\n", _file);

            GenerateSubscriptions();

            RemoveEmptySubscriptions();

            Console.WriteLine("[Subscriber-{0}] Generation finished!\n", _file);
        }

        public List<Subscription> GetSubscriptions()
        {
            return _subscriptions;
        }

        private void Initialize()
        {
            try
            {
                _subscriptionsConfig.LoadConfiguration();
                _publicationsConfig.LoadConfiguration();
                _messagesNumber = _subscriptionsConfig.GetMessageNumber().GetValue();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

        private void GenerateSubscriptions()
        {
            Console.WriteLine("[Subscriber-{0}] Creating subscriptions...\n", _file);
            for (int i = 0; i < _messagesNumber; ++i)
            {
                Subscription subscription = CreateSubscription();

                _subscriptions.Add(subscription);
            }

            Console.WriteLine("[Subscriber-{0}] Checking weights...\n", _file);
            CheckWeights();

            Console.WriteLine("[Subscriber-{0}] Checking weights for min operator...\n", _file);
            CheckMinOperator();

            Console.WriteLine("[Subscriber-{0}] Writing subscriptions to file...\n", _file);
            WriteSubscriptonsToFile();
        }

        private Subscription CreateSubscription()
        {
            Subscription subscription = new Subscription(_subId);

            // company
            SubscriptionTouple<String> companyTouple = (SubscriptionTouple<String>)this.GetSubscriptionTouple(Field.company, false);
            subscription.SetCompany(companyTouple);

            // drop
            SubscriptionTouple<Double> dropTouple = (SubscriptionTouple<Double>)this.GetSubscriptionTouple(Field.drop, true);
            subscription.SetDrop(dropTouple);

            // value
            SubscriptionTouple<int> valueTouple = (SubscriptionTouple<int>)GetSubscriptionTouple(Field.value, true);
            subscription.SetValue(valueTouple);

            // variation
            SubscriptionTouple<Double> variationTouple = (SubscriptionTouple<Double>)this.GetSubscriptionTouple(Field.variation, true);
            subscription.SetVariation(variationTouple);

            // date
            SubscriptionTouple<String> dateTouple = (SubscriptionTouple<String>)this.GetSubscriptionTouple(Field.date, false);
            subscription.SetDate(dateTouple);

            return subscription;
        }

        private Object GetSubscriptionTouple(Field field, bool isRanged)
        {
            if (!isRanged)
            {
                return GetSubscriptionToupleFromSet(field);
            }

            return GetSubscriptionToupleFromRange(field);
        }

        private Object GetSubscriptionToupleFromSet(Field field)
        {
            List<string> setValues = new List<string>();

            string op = GetRandom(_subscriptionsConfig.GetOperators());

            if (field == Field.company)
            {
                setValues = _publicationsConfig.GetCompanies();
            }

            if (field == Field.date)
            {
                setValues = _publicationsConfig.GetStringDates();
            }

            String value = GetRandom(setValues);

            return new SubscriptionTouple<string>(field, op, value);
        }

        private Object GetSubscriptionToupleFromRange(Field field)
        {
            RangedModel model = new RangedModel();

            string op = GetRandom(_subscriptionsConfig.GetOperators());

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

            return new SubscriptionTouple<Double>(field, op, value);
        }

        private void CheckWeights()
        {
            while (!CheckFieldInSubscriptions(Field.company)) { }
            while (!CheckFieldInSubscriptions(Field.drop)) { }
            while (!CheckFieldInSubscriptions(Field.value)) { }
            while (!CheckFieldInSubscriptions(Field.variation)) { }
            while (!CheckFieldInSubscriptions(Field.date)) { }
        }

        private bool CheckFieldInSubscriptions(Field field)
        {
            double fieldWeight = _subscriptionsConfig.GetFieldWeight(field);

            double calculatedWeight = CalculateField(field, fieldWeight);

            if (calculatedWeight > fieldWeight)
            {
                Subscription subcription = GetRandomSubscription(field);

                RemoveFieldFromSubscription(subcription, field);

                return false;
            }

            return true;
        }

        private double CalculateField(Field field, double fieldWeight)
        {
            Predicate<Subscription> filter = SubsFilterByField(field);

            int nr = _subscriptions.FindAll(filter).ToList().Count;

            double nextWeight = (nr - 1) * 100;
            if (_messagesNumber != 0)
            {
                nextWeight = nextWeight / _messagesNumber;
            }

            if (nextWeight < fieldWeight)
            {
                return fieldWeight;
            }

            double weight = nr * 100;
            if (_messagesNumber != 0)
            {
                weight = weight / _messagesNumber;
            }

            string strWeight = string.Format("%.2f", weight);

            return double.Parse(strWeight);
        }

        private Subscription GetRandomSubscription(Field field)
        {
            Predicate<Subscription> subscriptionFilter = SubsFilterByField(field);

            List<Subscription> filteredSubs = _subscriptions.FindAll(subscriptionFilter).ToList();

            Random random = new Random();

            int rangeMax = filteredSubs.Count;

            int value = random.Next(rangeMax);

            return filteredSubs.First(s => s.GetValuesValue() == value);
        }

        private void RemoveFieldFromSubscription(Subscription subscription, Field field)
        {
            if (field == Field.company)
            {
                subscription.SetCompany(null);
            }

            if (field == Field.drop)
            {
                subscription.SetDrop(null);
            }

            if (field == Field.value)
            {
                subscription.SetValue(null);
            }

            if (field == Field.variation)
            {
                subscription.SetVariation(null);
            }

            if (field == Field.date)
            {
                subscription.SetDate(null);
            }
        }

        private void CheckMinOperator()
        {
            bool isOk = false;

            String minOperator = _subscriptionsConfig.GetMinOperator().GetValue();
            Field minField = _subscriptionsConfig.MinField();
            int configMinWeight = _subscriptionsConfig.GetMinOperatorWeight().GetValue();

            int nrTouples = CalculateNumberOfTouplesByField(minField);

            while (!isOk)
            {
                isOk = true;
                double minOperatorWeight = CalculateMinOperator(minField, minOperator, nrTouples);

                if (minOperatorWeight < configMinWeight)
                {
                    isOk = false;
                    SetSubscriptionWithMinOperator(minField, minOperator);
                }
            }
        }

        private int CalculateNumberOfTouplesByField(Field field)
        {
            Predicate<Subscription> filter = SubsFilterByField(field);

            return _subscriptions.FindAll(filter).ToList().Count;
        }

        private double CalculateMinOperator(Field minField, String minOperator, int nrTouples)
        {
            Predicate<Subscription> filter = SubsFilterByFieldAndEqualsOperator(minField, minOperator);

            int nr = _subscriptions.FindAll(filter).ToList().Count;

            double weight = nr * 100 / nrTouples;

            string strWeight = string.Format("%.2f", weight);

            return double.Parse(strWeight);
        }

        private void SetSubscriptionWithMinOperator(Field minField, string minOperator)
        {
            Predicate<Subscription> predicate = SubsFilterByFieldAndNotEqualsOperator(minField, minOperator);

            List<Subscription> availableSubs = _subscriptions.FindAll(predicate).ToList();

            Random random = new Random();

            int rangeMax = availableSubs.Count;

            int value = random.Next(rangeMax);

            Subscription randomSub = availableSubs.Where(v => v.GetValuesValue() == value).FirstOrDefault();

            if (minField == Field.company && randomSub.GetCompanyValue() != string.Empty
                            && randomSub.GetCompany().GetOperator() != minOperator)
            {
                randomSub.GetCompany().SetOperator(minOperator);
                return;
            }

            if (minField == Field.drop && randomSub.GetDrop() != null
                    && randomSub.GetDrop().GetOperator() != minOperator)
            {
                randomSub.GetDrop().SetOperator(minOperator);
                return;
            }

            if (minField == Field.value && randomSub.GetValuesValue() != 0
                    && randomSub.GetValues().GetOperator() != minOperator)
            {
                randomSub.GetValues().SetOperator(minOperator);
                return;
            }

            if (minField == Field.variation && randomSub.GetVariationValue() != 0
                    && randomSub.GetVariation().GetOperator() != minOperator)
            {
                randomSub.GetVariation().SetOperator(minOperator);
                return;
            }

            if (minField == Field.date && randomSub.GetDate() != null
                    && randomSub.GetDate().GetOperator() != minOperator)
            {
                randomSub.GetDate().SetOperator(minOperator);
                return;
            }
        }

        private void WriteSubscriptonsToFile()
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(_file))
            {
                foreach (var subscription in _subscriptions)
                {
                    string subs = subscription.ToString();

                    if (subs.Length > 0)
                    {
                        file.WriteLine(subs);
                    }
                }
            }
        }

        private Predicate<Subscription> SubsFilterByField(Field field)
        {
            Predicate<Subscription> subscriptionFilter = s =>
                      s.GetCompanyValue() != string.Empty && Field.company == field ||
                      s.GetDateValue() != string.Empty && Field.date == field ||
                      s.GetDropValue() != 0 && Field.drop == field ||
                      s.GetValuesValue() != 0 && Field.value == field ||
                      s.GetVariationValue() != 0 && Field.variation == field;

            return subscriptionFilter;
        }

        private Predicate<Subscription> SubsFilterByFieldAndEqualsOperator(Field field, String op)
        {
            Predicate<Subscription> subscriptionFilter = s=>
              s.GetCompanyValue() != string.Empty && Field.company == field && s.GetCompany().GetOperator().Equals(op) ||
              s.GetDateValue() != string.Empty && Field.date == field && s.GetDate().GetOperator().Equals(op) ||
              s.GetDropValue() != 0 && Field.drop == field && s.GetDrop().GetOperator().Equals(op) ||
              s.GetValuesValue() != 0 && Field.value == field && s.GetValues().GetOperator().Equals(op) ||
              s.GetVariationValue() != 0 && Field.variation == field && s.GetVariation().GetOperator().Equals(op);

            return subscriptionFilter;
        }

        private Predicate<Subscription> SubsFilterByFieldAndNotEqualsOperator(Field field, string op)
        {
            bool subscriptionFilter(Subscription s) =>
              s.GetCompanyValue() != string.Empty && Field.company == field && !s.GetCompany().GetOperator().Equals(op) ||
              s.GetDateValue() != string.Empty && Field.date == field && !s.GetDate().GetOperator().Equals(op) ||
              s.GetDropValue() != 0 && Field.drop == field && !s.GetDrop().GetOperator().Equals(op) ||
              s.GetValuesValue() != 0 && Field.value == field && !s.GetValues().GetOperator().Equals(op) ||
              s.GetVariationValue() != 0 && Field.variation == field && !s.GetVariation().GetOperator().Equals(op);

            return subscriptionFilter;
        }

        private string GetRandom(List<string> list)
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

        private void RemoveEmptySubscriptions()
        {
            List<Subscription> nonEmptySubscriptions = new List<Subscription>();

            nonEmptySubscriptions = _subscriptions.FindAll(s => s.CheckNullValues() < 5).ToList();

            _subscriptions = nonEmptySubscriptions;
        }
    }
}

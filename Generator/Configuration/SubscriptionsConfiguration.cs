using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Configuration
{
    public class SubscriptionsConfiguration
    {
        private String fileName = "subscriptions.properties";

        private String outputFileName = "subscriptions.txt";

        private String dateStringFormat = "dd.MM.yyyy";

        private ValueModel<Integer> nrMessages;
        private ValueModel<Integer> value;
        private ValueModel<Integer> drop;
        private ValueModel<Integer> variation;
        private ValueModel<Integer> companies;
        private ValueModel<Integer> dates;
        private ArrayList<String> operators;
        private ValueModel<String> minField;
        private ValueModel<String> minOperator;
        private ValueModel<Integer> minOperatorWeight;

        public SubscriptionsConfig()
        {
            this.nrMessages = new ValueModel<Integer>();
            this.value = new ValueModel<Integer>();
            this.drop = new ValueModel<Integer>();
            this.variation = new ValueModel<Integer>();
            this.companies = new ValueModel<Integer>();
            this.dates = new ValueModel<Integer>();
            this.setOperators(new ArrayList<String>());
            this.minField = new ValueModel<String>();
            this.minOperator = new ValueModel<String>();
            this.minOperatorWeight = new ValueModel<Integer>();
        }

        public String getOutputFileName()
        {
            return this.outputFileName;
        }

        public ValueModel<Integer> getValue()
        {
            return value;
        }

        public void setValue(ValueModel<Integer> value)
        {
            this.value = value;
        }

        public ValueModel<Integer> getNrMessages()
        {
            return nrMessages;
        }

        public void setNrMessages(ValueModel<Integer> nrMessages)
        {
            this.nrMessages = nrMessages;
        }

        public ValueModel<Integer> getDrop()
        {
            return drop;
        }

        public void setDrop(ValueModel<Integer> drop)
        {
            this.drop = drop;
        }

        public ValueModel<Integer> getVariation()
        {
            return variation;
        }

        public void setVariation(ValueModel<Integer> variation)
        {
            this.variation = variation;
        }

        public ValueModel<Integer> getCompanies()
        {
            return companies;
        }

        public void setCompanies(ValueModel<Integer> companies)
        {
            this.companies = companies;
        }

        public ValueModel<Integer> getDates()
        {
            return dates;
        }

        public void setDates(ValueModel<Integer> dates)
        {
            this.dates = dates;
        }

        public ArrayList<String> getOperators()
        {
            return operators;
        }

        public void setOperators(ArrayList<String> operators)
        {
            this.operators = operators;
        }

        public ValueModel<String> getMinField()
        {
            return minField;
        }

        public void setMinField(ValueModel<String> minField)
        {
            this.minField = minField;
        }

        public ValueModel<String> getMinOperator()
        {
            return minOperator;
        }

        public void setMinOperator(ValueModel<String> minOperator)
        {
            this.minOperator = minOperator;
        }

        public ValueModel<Integer> getMinOperatorWeight()
        {
            return minOperatorWeight;
        }

        public void setMinOperatorWeight(ValueModel<Integer> minOperatorWeight)
        {
            this.minOperatorWeight = minOperatorWeight;
        }

        public double getFieldWeight(Field field)
        {
            if (field == Field.company)
            {
                return this.companies.getValue();
            }

            if (field == Field.drop)
            {
                return this.drop.getValue();
            }

            if (field == Field.value)
            {
                return this.value.getValue();
            }

            if (field == Field.variation)
            {
                return this.variation.getValue();
            }

            if (field == Field.date)
            {
                return this.dates.getValue();
            }

            return 0;
        }

        public Field minField()
        {
            String field = this.minField.getValue();

            if (field.equals("company"))
            {
                return Field.company;
            }

            if (field.equals("drop"))
            {
                return Field.drop;
            }

            if (field.equals("value"))
            {
                return Field.value;
            }

            if (field.equals("variation"))
            {
                return Field.variation;
            }

            if (field.equals("date"))
            {
                return Field.date;
            }

            return Field.company;
        }

        public void loadConfig() throws IOException, ParseException {

        Properties prop = new Properties();

        InputStream inputStream = new FileInputStream(new File(this.fileName));

        prop.load(inputStream);

		this.prepareConfig(prop);
    }

    private void prepareConfig(Properties prop) throws ParseException
    {	
		this.loadNrMessages(prop, "nr-messages", this.nrMessages);
		this.loadIntegerValue(prop, "value", this.value);
		this.loadIntegerValue(prop, "drop", this.drop);
		this.loadIntegerValue(prop, "variation", this.variation);
		this.loadIntegerValue(prop, "companies", this.companies);
		this.loadIntegerValue(prop, "dates", this.dates);
		this.loadStringValue(prop, "min-field", this.minField);
		this.loadStringValue(prop, "min-op", this.minOperator);
		this.loadIntegerValue(prop, "min-freq-op", this.minOperatorWeight);
		this.loadOperators(prop);

    }

    private void loadNrMessages(Properties prop, String key, ValueModel<Integer> nrMessages)
    {
        String nrMessagesKey = prop.getProperty(key);

        nrMessages.setValue(Integer.parseInt(nrMessagesKey));
    }

    private void loadIntegerValue(Properties prop, String key, ValueModel<Integer> model)
    {
        String val = prop.getProperty(key);

        model.setValue(Integer.parseInt(val));
    }

    private void loadStringValue(Properties prop, String key, ValueModel<String> model)
    {
        String val = prop.getProperty(key);

        model.setValue(val);
    }

    private void loadOperators(Properties prop)
    {
        String operatorsValue = prop.getProperty("operators");

        String[] operators = operatorsValue.split(",");

        for (String operator : operators)
        {
            this.operators.add(operator);
        }
    }
}
}

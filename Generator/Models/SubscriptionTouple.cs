using Generator.Configuration;

namespace Generator
{
    public class SubscriptionTouple<T>
    {
        private Field field;
        private string op;
        private T value;

        public SubscriptionTouple(Field field, string op, T value)
        {
            this.field = field;
            this.op = op;
            this.value = value;
        }

        public T GetValue()
        {
            return value;
        }
    }
}
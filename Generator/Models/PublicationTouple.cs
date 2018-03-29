using Generator.Configuration;

namespace Generator
{
    public class PublicationTouple<T>
    {
        private Field field;
        private T value;

        public PublicationTouple(Field field, T value)
        {
            this.field = field;
            this.value = value;
        }

        public T GetValue()
        {
            return value;
        }
    }
}
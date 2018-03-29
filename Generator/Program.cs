using System;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generatorPub = new GeneratorPublisher("publications.txt");
            var generatorSub = new GeneratorSubscriber(Guid.NewGuid(), "subscriptions.txt");
            generatorPub.Generate();
            generatorSub.Generate();
        }
    }
}

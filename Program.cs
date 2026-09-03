namespace route_assignment_advanced1
{
    // ==================== Q2: Container<T> ====================
    public class Container<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public T Get(int index)
        {
            return items[index];
        }
    }

    // ==================== Q3: Pair<TKey, TValue> ====================
    public class Pair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public void Print()
        {
            Console.WriteLine($"{Key} : {Value}");
        }
    }

    // ==================== Q4: Swap<T> ====================
    public static class Utility
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }

    // ==================== Q5: FindMax<T> ====================
    public static class Utility2
    {
        public static T FindMax<T>(T[] items) where T : IComparable<T>
        {
            T max = items[0];
            foreach (var item in items)
            {
                if (item.CompareTo(max) > 0)
                    max = item;
            }
            return max;
        }
    }

    // ==================== Q6: IRepository<T> ====================
    public interface IRepository<T>
    {
        void Add(T item);
        T GetById(int id);
        List<T> GetAll();
        void Remove(T item);
    }

    // كلاس بسيط بنستخدمه فى امثلة الجينيريكس
    public class Shipment
    {
        public string TrackingCode { get; set; }

        public Shipment() { }

        public Shipment(string trackingCode)
        {
            TrackingCode = trackingCode;
        }

        public virtual decimal EstimatedCost
        {
            get { return 100; }
        }

        public override string ToString()
        {
            return TrackingCode;
        }
    }

    public class StandardShipment : Shipment
    {
        public override decimal EstimatedCost
        {
            get { return 95; }
        }
    }

    public class ShipmentRepository : IRepository<Shipment>
    {
        private List<Shipment> shipments = new List<Shipment>();

        public void Add(Shipment item) { shipments.Add(item); }
        public Shipment GetById(int id) { return shipments[id]; }
        public List<Shipment> GetAll() { return shipments; }
        public void Remove(Shipment item) { shipments.Remove(item); }
    }

    // ==================== Q7: struct constraint ====================
    public class ValueBox<T> where T : struct
    {
        public T Value { get; set; }
    }

    // ==================== Q8: class constraint ====================
    public class ReferenceBox<T> where T : class
    {
        public T Value { get; set; }
    }

    // ==================== Q9: new() constraint ====================
    public class Factory<T> where T : new()
    {
        public T CreateInstance()
        {
            return new T();
        }
    }

    // ==================== Q10: interface constraint ====================
    public interface ITrackable
    {
        string GetTrackingStatus();
    }

    public class TrackableShipment : ITrackable
    {
        public string GetTrackingStatus()
        {
            return "In Transit";
        }
    }

    public class TrackingLogger<T> where T : ITrackable
    {
        public void LogStatus(T item)
        {
            Console.WriteLine(item.GetTrackingStatus());
        }
    }

    // ==================== Q11: base class constraint ====================
    public class ShipmentProcessor<T> where T : Shipment
    {
        public void PrintCost(T shipment)
        {
            Console.WriteLine($"Cost : {shipment.EstimatedCost}");
        }
    }

    // ==================== Q12: multiple constraints ====================
    public class Processor<T> where T : class, ITrackable, new()
    {
        public T CreateAndLog()
        {
            T item = new T();
            Console.WriteLine(item.GetTrackingStatus());
            return item;
        }
    }

    // ==================== Q13 & Q14: default keyword, SafeList<T> ====================
    public class DefaultExample<T>
    {
        public T GetDefaultValue()
        {
            return default(T);
        }
    }

    public class SafeList<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public T GetAt(int index)
        {
            if (index < 0 || index >= items.Count)
                return default(T);

            return items[index];
        }
    }

    // ==================== Q15: covariance - out ====================
    public interface IProducer<out T>
    {
        T Produce();
    }

    public class ShipmentProducer : IProducer<Shipment>
    {
        public Shipment Produce() { return new StandardShipment(); }
    }

    // ==================== Q16: contravariance - in ====================
    public interface IConsumer<in T>
    {
        void Consume(T item);
    }

    public class GeneralConsumer : IConsumer<object>
    {
        public void Consume(object item)
        {
            Console.WriteLine("Consumed");
        }
    }

    // ==================== Q18: static members in generic types ====================
    public class Counter<T>
    {
        public static int Count;

        public Counter()
        {
            Count++;
        }
    }

    // ==================== Q19: inherit from a generic class ====================
    public class BaseRepository<T>
    {
        public virtual void Add(T item)
        {
            Console.WriteLine("Item added");
        }
    }

    public class ClosedShipmentRepository : BaseRepository<Shipment>
    {
        public override void Add(Shipment item)
        {
            Console.WriteLine($"Shipment {item} added");
        }
    }

    // ==================== Q20: Cache<TKey, TValue> ====================
    public class Cache<TKey, TValue>
    {
        // كل عنصر بيتخزن مع وقت انتهاءه
        private class CacheItem
        {
            public TValue Value { get; set; }
            public DateTime ExpiresAt { get; set; }
        }

        private Dictionary<TKey, CacheItem> items = new Dictionary<TKey, CacheItem>();

        public void Add(TKey key, TValue value, TimeSpan expiration)
        {
            var cacheItem = new CacheItem
            {
                Value = value,
                ExpiresAt = DateTime.Now.Add(expiration)
            };

            items[key] = cacheItem;
        }

        public TValue Get(TKey key)
        {
            if (!Contains(key))
                return default(TValue);

            return items[key].Value;
        }

        public bool Contains(TKey key)
        {
            if (!items.ContainsKey(key))
                return false;

            // لو الكاش انتهى، امسحه واعتبره مش موجود
            if (DateTime.Now > items[key].ExpiresAt)
            {
                items.Remove(key);
                return false;
            }

            return true;
        }

        public void Remove(TKey key)
        {
            if (items.ContainsKey(key))
                items.Remove(key);
        }
    }

    // ==================== Program (Main) ====================
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Q2: Container<T>");
            Container<string> namesContainer = new Container<string>();
            namesContainer.Add("Ahmed");
            Console.WriteLine(namesContainer.Get(0));

            Console.WriteLine();
            Console.WriteLine("Q3: Pair<TKey, TValue>");
            Pair<string, int> ageEntry = new Pair<string, int>("Ahmed", 25);
            ageEntry.Print();

            Console.WriteLine();
            Console.WriteLine("Q4: Swap<T>");
            int x = 5, y = 10;
            Utility.Swap<int>(ref x, ref y);
            Console.WriteLine($"x = {x}, y = {y}");

            Console.WriteLine();
            Console.WriteLine("Q5: FindMax<T>");
            int[] numbers = { 3, 8, 1, 9, 4 };
            Console.WriteLine(Utility2.FindMax(numbers));

            Console.WriteLine();
            Console.WriteLine("Q6: IRepository<T>");
            ShipmentRepository shipmentRepo = new ShipmentRepository();
            shipmentRepo.Add(new Shipment("SH001"));
            shipmentRepo.Add(new Shipment("SH002"));
            Console.WriteLine(shipmentRepo.GetById(0));

            Console.WriteLine();
            Console.WriteLine("Q7: struct constraint");
            ValueBox<int> valueBox = new ValueBox<int>();
            valueBox.Value = 10;
            Console.WriteLine(valueBox.Value);

            Console.WriteLine();
            Console.WriteLine("Q8: class constraint");
            ReferenceBox<string> referenceBox = new ReferenceBox<string>();
            referenceBox.Value = "hello";
            Console.WriteLine(referenceBox.Value);

            Console.WriteLine();
            Console.WriteLine("Q9: new() constraint");
            Factory<Shipment> factory = new Factory<Shipment>();
            Shipment createdShipment = factory.CreateInstance();
            Console.WriteLine(createdShipment.EstimatedCost);

            Console.WriteLine();
            Console.WriteLine("Q10: interface constraint");
            TrackingLogger<TrackableShipment> logger = new TrackingLogger<TrackableShipment>();
            logger.LogStatus(new TrackableShipment());

            Console.WriteLine();
            Console.WriteLine("Q11: base class constraint");
            ShipmentProcessor<StandardShipment> processor = new ShipmentProcessor<StandardShipment>();
            processor.PrintCost(new StandardShipment());

            Console.WriteLine();
            Console.WriteLine("Q12: multiple constraints");
            Processor<TrackableShipment> multiProcessor = new Processor<TrackableShipment>();
            multiProcessor.CreateAndLog();

            Console.WriteLine();
            Console.WriteLine("Q13: default keyword");
            DefaultExample<int> defaultExample = new DefaultExample<int>();
            Console.WriteLine(defaultExample.GetDefaultValue());

            Console.WriteLine();
            Console.WriteLine("Q14: SafeList<T>");
            SafeList<int> safeList = new SafeList<int>();
            safeList.Add(5);
            Console.WriteLine(safeList.GetAt(10));

            Console.WriteLine();
            Console.WriteLine("Q15: covariance - out");
            IProducer<object> producer = new ShipmentProducer();
            Console.WriteLine(producer.Produce());

            Console.WriteLine();
            Console.WriteLine("Q16: contravariance - in");
            IConsumer<Shipment> consumer = new GeneralConsumer();
            consumer.Consume(new Shipment("SH003"));

            Console.WriteLine();
            Console.WriteLine("Q18: static members in generic types");
            new Counter<int>();
            new Counter<int>();
            new Counter<string>();
            Console.WriteLine($"Counter<int>.Count = {Counter<int>.Count}");
            Console.WriteLine($"Counter<string>.Count = {Counter<string>.Count}");

            Console.WriteLine();
            Console.WriteLine("Q19: inherit from a generic class");
            ClosedShipmentRepository closedRepo = new ClosedShipmentRepository();
            closedRepo.Add(new Shipment("SH004"));

            Console.WriteLine();
            Console.WriteLine("Q20: Cache<TKey, TValue>");
            Cache<string, string> cache = new Cache<string, string>();
            cache.Add("SH001", "Cairo", TimeSpan.FromSeconds(5));
            Console.WriteLine(cache.Contains("SH001"));
            Console.WriteLine(cache.Get("SH001"));
        }
    }
}
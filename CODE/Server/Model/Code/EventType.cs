namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }

        
        public struct NumericChangeEvent
        {
            public Entity changeObj;
            public NumericType numericType;
            public long finalId;
            public long[] result;
        }
    }
}
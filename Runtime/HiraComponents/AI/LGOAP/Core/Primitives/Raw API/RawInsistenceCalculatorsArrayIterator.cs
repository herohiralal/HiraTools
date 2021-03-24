using HiraEngine.Components.Blackboard.Raw;

namespace HiraEngine.Components.AI.LGOAP.Raw
{
    public unsafe struct RawInsistenceCalculatorsArrayIterator
    {
        public RawInsistenceCalculatorsArrayIterator(RawInsistenceCalculatorsArray target)
        {
            _count = target.Count;
            _currentIndex = 0;
            _current = target.First;
        }
        
        private readonly byte _count;
        private byte _currentIndex;
        private byte* _current;

        public bool MoveNext => _currentIndex < _count;

        public void Get(out RawBlackboardScoreCalculatorsArray current, out byte index)
        {
            current = new RawBlackboardScoreCalculatorsArray(_current);
            _current += current.Size;
            index = _currentIndex++;
        }
    }
}
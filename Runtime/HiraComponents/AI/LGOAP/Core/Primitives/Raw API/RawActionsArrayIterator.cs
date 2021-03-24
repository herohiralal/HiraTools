using HiraEngine.Components.AI.LGOAP.Raw.Internal;
using HiraEngine.Components.Blackboard.Raw;

namespace HiraEngine.Components.AI.LGOAP.Raw
{
    public unsafe struct RawActionsArrayIterator
    {
        public RawActionsArrayIterator(RawActionsArray target)
        {
            _count = target.Count;
            _currentIndex = 0;
            _current = target.First;
        }
        
        private readonly byte _count;
        private byte _currentIndex;
        private byte* _current;

        public bool MoveNext => _currentIndex < _count;

        public void Get(out RawBlackboardDecoratorsArray precondition, out RawBlackboardScoreCalculatorsArray costCalculator, out RawBlackboardEffectorsArray effect, out byte index)
        {
            var current = new RawAction(_current);
            _current += current.Size;
            index = _currentIndex++;
            current.Break(out precondition, out costCalculator, out effect);
        }
    }
}
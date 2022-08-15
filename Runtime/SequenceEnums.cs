using UnityEngine;

namespace Theogramme.SequenceScript
{

    public enum ValueMode { Set, Add, Subtract }

    [System.Flags]
    public enum ComponentSearchMode { Object = 1 << 0, Child = 1 << 1, Parent = 1 << 2 }
    
}


using System;
using System.Collections;

namespace AdoNetCore.AseClient
{
    /// <summary>
    /// Collects all errors generated by Adaptive Server ADO.NET Data Provider.
    /// </summary>
    public sealed class AseErrorCollection : ICollection
    {
        private readonly object _syncRoot = new object();
        private readonly AseError[] _errors;
        private readonly int _indexOfMostSevereError;
        public AseError MainError { get { return _indexOfMostSevereError == -1 ? null : _errors[_indexOfMostSevereError]; } }

        internal AseErrorCollection(params AseError[] errors) 
        {
            _errors = errors ?? new AseError[0];
            _indexOfMostSevereError = GetIndexOfMostSevereError();
        }

        /// <summary>
        /// The number of errors in the collection.
        /// </summary>
        public int Count => _errors.Length;

        public bool IsSynchronized => true;

        public object SyncRoot => _syncRoot;

        /// <summary>
        /// Copies the elements of the AseErrorCollection into an array, starting at the given index within the array.
        /// </summary>
        /// <param name="array">The array into which to copy the elements.</param>
        /// <param name="index">The starting index of the array.</param>
        public void CopyTo(Array array, int index)
        {
            Array.Copy(_errors, 0, array, index, _errors.Length);
        }

        /// <summary>
        /// Enumerates the errors in this collection.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        /// <summary>
        /// The error at the specified index.
        /// </summary>
        public AseError this[int index] 
        {
            get 
            {
                return _errors[index];
            }
        }

        internal int GetIndexOfMostSevereError()
        {
            if (_errors.Length == 0) { return -1; }
            if (_errors.Length == 1) { return 0; }

            int result = 0;
            for (int i = 1; i < _errors.Length; i++)
            {
                // The '=' in '<=' means that for equal severity, we take the last error in the list. 
                if (_errors[result].Severity <= _errors[i].Severity)
                {
                    result = i;
                }
            }
            return result;
        }
    }
}

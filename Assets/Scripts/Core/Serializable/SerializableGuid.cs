using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Lix.Core
{
    /// <summary>
    /// Serializable wrapper for System.Guid.
    /// Can be implicitly converted to/from System.Guid.
    ///
    /// Author: Katie Kennedy (Searous)
    /// </summary>
    [Serializable]
    public struct SerializableGuid : ISerializationCallbackReceiver, ISerializable
    {
        private Guid _guid;
        [SerializeField] private string _serializableGuid;

        public SerializableGuid(String guid)
        {
            _guid = Guid.Parse(guid);
            _serializableGuid = _guid.ToString();
        }

        public SerializableGuid(Guid guid)
        {
            _guid = guid;
            _serializableGuid = _guid.ToString();
        }

        public SerializableGuid(SerializationInfo info, StreamingContext context)
        {
            _serializableGuid = info.GetString("guid");
            _guid = Guid.Parse(_serializableGuid);
        }

        public override bool Equals(object obj)
        {
            return obj is SerializableGuid guid &&
                    this._guid.Equals(guid._guid);
        }

        public override int GetHashCode()
        {
            return -1324198676 + _guid.GetHashCode();
        }

        public void OnAfterDeserialize()
        {
            try
            {
                _guid = Guid.Parse(_serializableGuid);
            }
            catch
            {
                _guid = Guid.Empty;
                Debug.LogWarning($"Attempted to parse invalid GUID string '{_serializableGuid}'. GUID will set to System.Guid.Empty");
            }
        }

        public void OnBeforeSerialize()
        {
            _serializableGuid = _guid.ToString();
        }

        public override string ToString() => _guid.ToString();

        public static bool operator ==(SerializableGuid a, SerializableGuid b) => a._guid == b._guid;
        public static bool operator !=(SerializableGuid a, SerializableGuid b) => a._guid != b._guid;
        public static implicit operator SerializableGuid(Guid guid) => new SerializableGuid(guid);
        public static implicit operator Guid(SerializableGuid serializable) => serializable._guid;
        public static implicit operator SerializableGuid(string serializableGuid)
        {
            if (string.IsNullOrEmpty(serializableGuid))
            {
                return new SerializableGuid(Guid.Empty);
            }

            return new SerializableGuid(Guid.Parse(serializableGuid));
        }
        public static implicit operator string(SerializableGuid serializableGuid) => serializableGuid.ToString();
        public static bool operator ==(SerializableGuid a, Guid b) => a._guid == b;
        public static bool operator !=(SerializableGuid a, Guid b) => a._guid != b;
        public bool IsNullOrEmpty()
        {
            if (_guid == null || _guid == Guid.Empty)
            {
                return true;
            }

            return false;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("guid", _guid);
        }
    }
}
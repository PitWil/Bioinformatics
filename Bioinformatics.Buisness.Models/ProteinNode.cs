using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Bioinformatics.Buisness.Models
{
    [DataContract]
    public class ProteinNode
    {
        private static Random _ran = new Random(DateTime.Now.Millisecond*333);
        private static readonly object ProteinNodeSynchRoot = new object();
        private static int _id;

        private readonly int _hashValue;
        public readonly object SynchRoot = new object();

        private readonly List<ProteinWeightNodes> _neighbors;

        private readonly Dictionary<int, ProteinWeightNode> _neighboursAll;

        [DataMember] private string[] _value;

        private readonly int _valueHashCode;

        public ProteinNode(string sequences)
        {
            lock (ProteinNodeSynchRoot)
            {
                _valueHashCode = _id;
                ++_id;
            }
            _neighbors = new List<ProteinWeightNodes> {new ProteinWeightNodes()};
            _neighboursAll = new Dictionary<int, ProteinWeightNode>();
            _value = sequences.Split(',');

            var z = _value.Aggregate("", (current, s) => current + s);
            _hashValue = z.GetHashCode();
        }

        public static int MaxCount { get; set; } = 80;

        public static float MaxFeromon { get; set; } = 20.0f;
        public static float MinFeromon { get; set; } = 1.0f;

        public string RealValue
        {
            get
            {
                var result = "";
                for (var i = 0; i < _value.Length - 1; i++)
                {
                    result += _value[i];
                }
                result += _value[_value.Length - 1];
                return result;
            }
        }

        public string Value
        {
            get
            {
                var result = "";
                for (var i = 0; i < _value.Length - 1; i++)
                {
                    result += _value[i] + ", ";
                }
                result += _value[_value.Length - 1];
                return result;
            }
        }

        public int Length => _value.Length;

        public string this[int index] => _value[index];

        public virtual Dictionary<int, ProteinWeightNode> NeighborsAll
        {
            get
            {
                lock (SynchRoot)
                {
                    return _neighboursAll;
                }
            }
        }

        [DataMember]
        public virtual List<ProteinWeightNodes> Neighbors => _neighbors;

        protected bool Equals(ProteinNode other)
        {
            return Equals(_value, other._value); //; && Equals(Neighbors, other.Neighbors);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != GetType()) return false;
            return Equals((ProteinNode) obj);
        }


        public override int GetHashCode()
        {
            return _hashValue; // ((_value != null ? _value.GetHashCode() : 0)*397);
            // ^ (Neighbors != null ? Neighbors.GetHashCode() : 0);
        }

        public static bool operator ==(ProteinNode p1, ProteinNode p2)
        {
            if (ReferenceEquals(p1, null) && ReferenceEquals(p2, null))
                return true;
            if (ReferenceEquals(p1, null) || ReferenceEquals(p2, null))
            {
                return false;
            }


            if (p1.Length != p2.Length)
                return false;
            if (p1.GetHashCode() != p2.GetHashCode())
            {
                return false;
            }

            var z1 = p1._value.Aggregate("", (current, s) => current + s);
            var z2 = p1._value.Aggregate("", (current, s) => current + s);
            return z1.Equals(z2);
        }

        public static bool operator !=(ProteinNode p1, ProteinNode p2)
        {
            return !(p1 == p2);
        }

        public int GetValueHashCode()
        {
            return _valueHashCode;
        }

        public void AddNeighbor(ProteinNode node)
        {
            if (node == null)
            {
                return;
            }
            lock (SynchRoot)
            {
                var newNode = new ProteinWeightNode {Node = node};
                if (_neighbors[_neighbors.Count - 1].Count < MaxCount)
                {
                    _neighbors[_neighbors.Count - 1].Add(newNode);
                    _neighboursAll.Add(newNode.Node.GetValueHashCode(), newNode);
                    return;
                }
                _neighbors.Add(new ProteinWeightNodes {newNode});
                _neighboursAll.Add(newNode.Node.GetValueHashCode(), newNode);
            }
        }

        [DataContract]
        public class ProteinWeightNodes : List<ProteinWeightNode>
        {
            private long _counter;
            private readonly object _synchRoot = new object();
            private double _weight = 2.0;

            [DataMember]
            public double Weight
            {
                get
                {
                    Evaporate();
                    CheckConstrain(this);
                    return _weight;
                }
            }

            protected void Evaporate()
            {
                var counter = ProteinWeightNode.Counter;
                lock (_synchRoot)
                {
                    var diff = counter - _counter;
                    if (diff == 0)
                    {
                        return;
                    }
                    var z = Math.Pow(0.99, diff);
                    _weight *= z;
                    _counter = counter;
                }
            }

            private static void CheckConstrain(ProteinWeightNodes node)
            {
                if (node._weight > MaxFeromon)
                {
                    node._weight = MaxFeromon;
                }
                else if (node._weight < MinFeromon)
                {
                    node._weight = MinFeromon;
                }
            }

            public static ProteinWeightNodes operator +(ProteinWeightNodes node, double c)
            {
                node.Evaporate();
                lock (node._synchRoot)
                {
                    node._weight += c;
                    CheckConstrain(node);
                }
                return node;
            }

            public static ProteinWeightNodes operator -(ProteinWeightNodes node, double c)
            {
                node.Evaporate();
                lock (node._synchRoot)
                {
                    node._weight -= c;
                    CheckConstrain(node);
                }
                return node;
            }

            public static ProteinWeightNodes operator *(ProteinWeightNodes node, double c)
            {
                node.Evaporate();
                lock (node._synchRoot)
                {
                    node._weight *= c;
                    CheckConstrain(node);
                }
                return node;
            }

            public static ProteinWeightNodes operator /(ProteinWeightNodes node, double c)
            {
                node.Evaporate();
                lock (node._synchRoot)
                {
                    node._weight /= c;
                    CheckConstrain(node);
                }
                return node;
            }
        }

        public class ProteinWeightNode
        {
            protected static object CounterSynchRoot = new object();

            public static long Counter;
            private static long _counter;
            private readonly object _synchRoot = new object();

            private double _weight = 2.0;

            [DataMember]
            public ProteinNode Node { get; set; }

            [DataMember]
            public double Weight
            {
                get
                {
                    Evaporate();
                    CheckConstrain(this);
                    return _weight;
                }
            }

            public static void AddToCounter(long c)
            {
                lock (CounterSynchRoot)
                {
                    Counter += c;
                }
            }

            private static void CheckConstrain(ProteinWeightNode node)
            {
                if (node._weight > MaxFeromon)
                {
                    node._weight = MaxFeromon;
                }
                else if (node._weight < MinFeromon)
                {
                    node._weight = MinFeromon;
                }
            }

            protected void Evaporate()
            {
                var caunter = Counter;
                lock (_synchRoot)
                {
                    var diff = Counter - _counter;
                    if (diff == 0)
                    {
                        return;
                    }
                    var z = Math.Pow(0.99, diff);
                    _weight *= z;
                    _counter = caunter;
                }
            }

            public static ProteinWeightNode operator +(ProteinWeightNode node, double c)
            {
                node.Evaporate();
                lock (node._synchRoot)
                {
                    node._weight += c;
                    CheckConstrain(node);
                }
                return node;
            }

            public static ProteinWeightNode operator -(ProteinWeightNode node, double c)
            {
                node.Evaporate();
                lock (node._synchRoot)
                {
                    node._weight -= c;
                    CheckConstrain(node);
                }
                return node;
            }

            public static ProteinWeightNode operator *(ProteinWeightNode node, double c)
            {
                node.Evaporate();
                lock (node._synchRoot)
                {
                    node._weight *= c;
                    CheckConstrain(node);
                }
                return node;
            }

            public static ProteinWeightNode operator /(ProteinWeightNode node, double c)
            {
                node.Evaporate();
                lock (node._synchRoot)
                {
                    node._weight /= c;
                    CheckConstrain(node);
                }
                return node;
            }


            public double InitWeight(double a)
            {
                lock (_synchRoot)
                {
                    _weight = a;
                    CheckConstrain(this);
                }
                return _weight;
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TroopManagement
{
    public static class TroopUtility
    {
        public static string GenerateRandomName()
        {
            string _newName = "New Name";
            _newName = GetRandomFirstName() + " " + GetRandomLastName();
            return _newName;
        }

        public static string GenerateRandomBoatName()
        {
            List<string> _names = new List<string>();

            _names.Add("De Kano");
            _names.Add("Spetter");
            _names.Add("Mooie Boot");

            return _names[UnityEngine.Random.Range(0, _names.Count)];
        }

        public static string GenerateRandomRaiderpartyName()
        {
            List<string> _names = new List<string>();

            #region Party names
            _names.Add("Jochem's Party");
            _names.Add("Ravenous Raiders");
            _names.Add("Looters");
            _names.Add("Exiled Vikings");
            #endregion

            return _names[UnityEngine.Random.Range(0, _names.Count)];
        }

        public static TroopType GetRandomTroopType()
        {
            int _rng = UnityEngine.Random.Range(0, Enum.GetNames(typeof(TroopType)).Length);
            return (TroopType)_rng;
        }
        public static BoatType GetRandomBoatType()
        {
            int _rng = UnityEngine.Random.Range(0, Enum.GetNames(typeof(BoatType)).Length);
            return (BoatType)_rng;
        }

        static string GetRandomFirstName()
        {
            List<string> _names = new List<string>();

            #region Firts Names
            _names.Add("Ron");
            _names.Add("Harry");
            _names.Add("Voldemort");
            _names.Add("Hagrid");
            _names.Add("Hermelien");
            _names.Add("Bart");
            _names.Add("Dees");
            _names.Add("Jesse");
            _names.Add("Jochem");
            _names.Add("Gerjohn");
            _names.Add("Jan");
            #endregion

            return _names[UnityEngine.Random.Range(0,_names.Count)];
        }

        static string GetRandomLastName()
        {
            List<string> _names = new List<string>();

            #region Last Names
            _names.Add("Potter");
            _names.Add("Weasly");
            _names.Add("Bartolomeus");
            _names.Add("Folkertsma");
            _names.Add("The Slayer");
            _names.Add("Veen");
            _names.Add("Van Diepen");
            _names.Add("Kers");
            _names.Add("Van Den Bosch");
            _names.Add("Don Bon-Bon Von Megatron");
            #endregion

            return _names[UnityEngine.Random.Range(0, _names.Count)];
        }

        public static string IntToRomanNumeral (int _i)
        {
            if (_i < 0 || _i > 3999) throw new ArgumentOutOfRangeException("Int must be between 0 and 3999");
            if (_i < 1) return "";
            if (_i >= 1000) return "M" + IntToRomanNumeral(_i - 1000);
            if (_i >= 900) return "CM" + IntToRomanNumeral(_i - 900);
            if (_i >= 500) return "D" + IntToRomanNumeral(_i - 500);
            if (_i >= 400) return "CD" + IntToRomanNumeral(_i - 400);
            if (_i >= 100) return "C" + IntToRomanNumeral(_i - 100);
            if (_i >= 90) return "XC" + IntToRomanNumeral(_i - 90);
            if (_i >= 50) return "L" + IntToRomanNumeral(_i - 50);
            if (_i >= 40) return "XL" + IntToRomanNumeral(_i - 40);
            if (_i >= 10) return "X" + IntToRomanNumeral(_i - 10);
            if (_i >= 9) return "IX" + IntToRomanNumeral(_i - 9);
            if (_i >= 5) return "V" + IntToRomanNumeral(_i - 5);
            if (_i >= 4) return "IV" + IntToRomanNumeral(_i - 4);
            if (_i >= 1) return "I" + IntToRomanNumeral(_i - 1);
            throw new ArgumentOutOfRangeException("Could not convert int to roman numeral");
        }
    }

    public interface IEngageable
    {
        int troopCount
        {
            get;
        }

        void Attack(Party _attacker);
    }

    [Serializable]
    public class Party
    {
        public string partyName = "New party";
        public List<TroopStats> members = new List<TroopStats>();
        Dictionary<string, int> nameDatabase = new Dictionary<string, int>();
        public List<BoatStats> boats = new List<BoatStats>();

        public delegate void OnPartyAdded(Party _changedParty, TroopStats _added);
        public static event OnPartyAdded onPlayerPartyAdd;
        public delegate void OnPartyRemoved(Party _changedParty, TroopStats _removed);
        public static event OnPartyRemoved onPlayerPartyRemove;

        public Party(string _name)
        {
            partyName = _name;
            AddRandomBoat();
            AddRandomTroop();
        }

        public Party() { }

        public int TotalCapacity
        {
            get
            {
                int _i = 0;
                foreach(BoatStats bs in boats)
                {
                    _i += bs.troopCapacity;
                }
                return _i;
            }
        }

        public int TotalMembers
        {
            get
            {
                return members.Count;
            }
        }

        public bool AddNewBoat(string _boatName, BoatType _type)
        {
            BoatStats _newBoatStats = new BoatStats(_type);
            _boatName += (" " + TroopUtility.IntToRomanNumeral(AddNameToDatabase(_boatName)));
            _newBoatStats.boatName = _boatName;
            boats.Add(_newBoatStats);
            return true;
        }

        public bool RemoveBoatByName(string _boatToRemove)
        {
            if (boats.Count > 1)
            {
                BoatStats _removed = null;
                foreach (BoatStats bs in boats)
                {
                    if (bs.boatName == _boatToRemove)
                    {
                        _removed = bs;
                        boats.Remove(bs);
                        break;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveBoat(BoatStats _boatToRemove)
        {
            if (boats.Count > 1)
            {
                boats.Remove(_boatToRemove);
                return true;
            }
            else return false;
        }

        public bool AddNewMember(string _memberName, TroopType _type)
        {
            if (TotalCapacity > TotalMembers)
            {
                TroopStats _newMemberStats = new TroopStats(_type);
                _memberName += (" " + TroopUtility.IntToRomanNumeral(AddNameToDatabase(_memberName)));
                _newMemberStats.troopName = _memberName;
                members.Add(_newMemberStats);
                if (onPlayerPartyAdd != null)
                    onPlayerPartyAdd(this, _newMemberStats);
                return true;
            }
            else
            {
                Debug.LogWarning("Can's add " + _memberName + ", this party reached max capacity.");
                return false;
            }
        }

        public void RemoveMemberByName(string _memberToRemove)
        {
            TroopStats _removed = null;
            foreach (TroopStats ts in members)
            {
                if (ts.troopName == _memberToRemove)
                {
                    _removed = ts;
                    members.Remove(ts);
                    break;
                }
            }
            if (onPlayerPartyRemove != null && _removed != null)
                onPlayerPartyRemove(this, _removed);
        }

        public void RemoveMember(TroopStats _memberToRemove)
        {
            members.Remove(_memberToRemove);
            if (onPlayerPartyRemove != null && _memberToRemove != null)
                onPlayerPartyRemove(this,_memberToRemove);
        }

        public void AddRandomTroop()
        {
            AddNewMember(TroopUtility.GenerateRandomName(), TroopUtility.GetRandomTroopType());
        }

        public void AddRandomBoat()
        {
            AddNewBoat(TroopUtility.GenerateRandomBoatName(), TroopUtility.GetRandomBoatType());
        }

        int AddNameToDatabase(string _name)
        {
            if(nameDatabase == null)
                nameDatabase = new Dictionary<string, int>();

            if (nameDatabase.ContainsKey(_name))
            {
                nameDatabase[_name]++;
                return nameDatabase[_name];
            }
            else
            {
                nameDatabase.Add(_name, 1);
                return 0;
            }
        }
    }

    [Serializable]
    public enum TroopType
    {
        RegularViking,
        Beserker
    };

    [Serializable]
    public enum BoatType
    {
        TestBoat
    };

    [Serializable]
    public class Relationship
    {
        int relation = 0;
        public int Relation
        {
            get
            {
                return relation;
            }
        }
        public void ImproveRelation(int _amount)
        {
            relation += _amount;
        }

        public Relationship() { }
    }

    [Serializable]
    public class RelationshipManager
    {
        public void PopulateRelationships(Party _party)
        {
            if (_party.members.Count > 0)
            {
                foreach (TroopStats ts in _party.members)
                {
                    if (ts.relations == null)
                        ts.relations = new SerializableDictionary<TroopStats, Relationship>();
                    foreach (TroopStats ts2 in _party.members)
                    {
                        if (ts != ts2)
                        {
                            if(ts2.relations == null)
                                ts2.relations = new SerializableDictionary<TroopStats, Relationship>();
                            if (!ts.relations.ContainsKey(ts2))
                            {
                                Relationship _newRe = new Relationship();
                                ts.relations.Add(ts2, _newRe);
                                ts2.relations.Add(ts, _newRe);
                            }
                        }
                    }
                }
            }
        }

        public void UpdateRelations(Party _party, TroopStats _changedTroop)
        {

        }

        public void TroopAdded(Party _party, TroopStats _addedTroop)
        {
            _addedTroop.relations = new SerializableDictionary<TroopStats, Relationship>();
            foreach(TroopStats ts in _party.members)
            {
                if(ts != _addedTroop)
                {
                    Relationship _newRe = new Relationship();
                    ts.relations.Add(_addedTroop, _newRe);
                    _addedTroop.relations.Add(ts, _newRe);
                }
            }
        }

        public void TroopRemoved(Party _party, TroopStats _removed)
        {
            foreach (TroopStats ts in _party.members)
            {
                if (ts != _removed)
                {
                    ts.relations.Remove(_removed);
                }
                else
                {
                    Debug.LogError("RelationshipManager: Removed troop is still in party?");
                }
            }
        }
    }

    [Serializable]
    public class TroopStats
    {
        [Header("BaseTroop stats")]
        public string troopName = "New Troop";
        public TroopType type;
        public int health = 100;
        public int damage = 10;
        public float attackRange = 2f;
        [Range(0, 100)]
        public int blockChance = 30;
        public Sprite sprite;
        public SerializableDictionary<TroopStats, Relationship> relations = new SerializableDictionary<TroopStats, Relationship>();

        public TroopStats() { }

        public TroopStats(string _name, TroopType _type, int _hp, int _dmg, float _range, int _blockChance)
        {
            troopName = _name;
            type = _type;
            health = _hp;
            damage = _dmg;
            attackRange = _range;

            if (_blockChance > 100) blockChance = 100;
            else if (_blockChance < 0) blockChance = 0;
            else blockChance = _blockChance;

            sprite = Resources.Load<Sprite>(_type.ToString());

            relations = new SerializableDictionary<TroopStats, Relationship>();
        }

        public TroopStats(TroopType _type)
        {
            TroopStats _newStats = GetStatsForType(_type);

            troopName = _newStats.troopName;
            type = _newStats.type;
            health = _newStats.health;
            damage = _newStats.damage;
            attackRange = _newStats.attackRange;
            blockChance = _newStats.blockChance;

            sprite = Resources.Load<Sprite>(_type.ToString());

            relations = new SerializableDictionary<TroopStats, Relationship>();
        }

        public TroopStats GetStatsForType(TroopType _type)
        {
            TroopStats _newStats = null;

            switch (_type)
            {
                case TroopType.RegularViking:
                    _newStats = new TroopStats("NewRegularViking", TroopType.RegularViking, 100, 10, 2, 40);
                    break;
                case TroopType.Beserker:
                    _newStats = new TroopStats("NewBeserker", TroopType.Beserker, 80, 25, 2, 10);
                    break;
            }

            if (_newStats != null)
                return _newStats;
            else
            {
                Debug.LogError("Requested trooptype not setup in the GetStatsForType method!");
                return null;
            }
        }
    }

    [Serializable]
    public class BoatStats
    {
        [Header("Boat Stats")]
        public string boatName = "New Boat";
        public BoatType boatType;
        public int troopCapacity;
        public int lootCapasity;
        public float knots;
        public int health = 100;
        public Sprite sprite;

        public BoatStats() { }

        public BoatStats(string _name, BoatType _type, int _tCap, int _lCap, float _knots, int _hp)
        {
            boatName = _name;
            boatType = _type;
            troopCapacity = _tCap;
            lootCapasity = _lCap;
            knots = _knots;
            health = _hp;
            sprite = Resources.Load<Sprite>(_type.ToString());
        }

        public BoatStats(BoatType _type)
        {
            BoatStats _newStats = GetStatsForType(_type);

            boatName = _newStats.boatName;
            boatType = _newStats.boatType;
            troopCapacity = _newStats.troopCapacity;
            lootCapasity = _newStats.lootCapasity;
            knots = _newStats.knots;
            health = _newStats.health;
            sprite = Resources.Load<Sprite>(_type.ToString());
        }

        public BoatStats GetStatsForType(BoatType _type)
        {
            BoatStats _newStats = null;

            switch (_type)
            {
                case BoatType.TestBoat:
                    _newStats = new BoatStats("NewTestboat", BoatType.TestBoat, 10, 10, 10, 100);
                    break;
            }

            if (_newStats != null)
                return _newStats;
            else
            {
                Debug.LogError("Requested Boattype not setup in the GetStatsForType method!");
                return null;
            }
        }
    }
}

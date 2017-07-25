using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace SemanticAnalyzer
{
    public enum Score
    {
        sd, ssd
    }
    public class EntityKey
    {
        protected bool Equals(EntityKey other)
        {
            return string.Equals(Src, other.Src) && string.Equals(Entity, other.Entity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EntityKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Src != null ? Src.GetHashCode() : 0) * 397) ^ (Entity != null ? Entity.GetHashCode() : 0);
            }
        }

        public string Src;
        public string Entity;
        public EntityKey(string src, string entity)
        {
            Src = src;
            Entity = entity;
        }

    }

    public class EntityValue
    {
        public string Name;
        public string Type;
        public int PositiveSpecific;
        public int NegativeSpecific;
        public int NuteralSpecific;
        public int PositiveGeneral;
        public int NegativeGeneral;
        public int NuteralGeneral;


        public EntityValue(string name, string type, int positiveSpecific, int negativeSpecific, int nuteralSpecific, int positiveGeneral, int negativeGeneral, int nuteralGeneral)
        {
            Name = name;
            Type = type;
            PositiveSpecific = positiveSpecific;
            NegativeSpecific = negativeSpecific;
            NuteralSpecific = nuteralSpecific;
            PositiveGeneral = positiveGeneral;
            NegativeGeneral = negativeGeneral;
            NuteralGeneral = nuteralGeneral;
        }

    }
    public class FileUtils
    {

        public static void ReadSourcesBais(string sourcesBaisPath, Dictionary<EntityKey, EntityValue> sourcesBais)
        {
            var lines = File.ReadLines(sourcesBaisPath);
            foreach (var line in lines)
            {
                var elementsList = line.Split(',');
                EntityKey key = new EntityKey(elementsList[0], elementsList[1]);
                EntityValue value = new EntityValue(elementsList[2], elementsList[3], Int32.Parse(elementsList[4]), Int32.Parse(elementsList[5]), Int32.Parse(elementsList[6]), Int32.Parse(elementsList[7]), Int32.Parse(elementsList[8]), Int32.Parse(elementsList[9]));
                sourcesBais.Add(key, value);
            }
        }

        public static void UpdateSourcesBais(Dictionary<EntityKey, EntityValue> sourcesBais, string src, string entityId, string entityName, string entityType, Score entitySpecific, Score articleGeneral)
        {
            //EntityKey key = null;
            //EntityValue initValue = EntityValue(string name, string type, 0, 0, 0, 0, 0, 0);
            //if (!(sourcesBais.ContainsKey(key))) //new entity
            //{
            //    sourcesBais.Add(key);
            //}
            //else 
            //{
            //    sourcesBais
            //}
        }

        private static string ConvertToCSVLine(EntityKey key, EntityValue value)
        {
            string line = key.Src + ',' + key.Entity + ',' + value.Name + ',' + value.Type + ',' + (value.PositiveSpecific).ToString() +
                          ',' + value.NegativeSpecific.ToString() + ',' + value.NuteralSpecific.ToString() +
                          ',' + value.PositiveGeneral.ToString() + ',' + value.NegativeGeneral.ToString() +
                          ',' + value.NuteralGeneral.ToString();
            return line;
        }

        public static void WriteSourcesBais(string sourcesBaisPath, Dictionary<EntityKey, EntityValue> sourcesBais)
        {
            var sourcesBaisOutput = new StringBuilder();
            foreach (var item in sourcesBais)
            {
                sourcesBaisOutput.AppendLine(ConvertToCSVLine(item.Key, item.Value));
            }
            File.WriteAllText(sourcesBaisPath, sourcesBaisOutput.ToString());
        }
    }
}

namespace SparkEngine.Dictionaries
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Content;
    using SparkEngine.DataStructures;
    using SparkEngine.IDs;
    using SparkEngine.Rendering;

    public static class StructureDictionary
    {
        #region Fields

        private const string StructureDataPath = "Data/Structures/";

        private static IDictionary<int, DrawData> structureDrawData;
        private static IDictionary<int, ObeliskXML.Generator> generators;
        //private static IDictionary<int, HousingData> housings;

        #endregion

        #region Methods

        public static DrawData GetDrawData(int key)
        {
            return structureDrawData[key];
        }

        public static ObeliskXML.Generator GetGenerator(int key)
        {
            return generators[key];
        }

        //public static HousingData GetHousing(int key)
        //{
        //    return housings[key];
        //}

        internal static void LoadStructures(ContentManager content)
        {
            LoadDrawData();
            LoadGenerators(content);
            //LoadHousings();
        }

        private static void LoadDrawData()
        {
            structureDrawData = new Dictionary<int, DrawData>
            {

            };
        }

        private static void LoadGenerators(ContentManager content)
        {
            generators = new Dictionary<int, ObeliskXML.Generator>
            {
                { StructureIDs.Obelisk, content.Load<ObeliskXML.Generator>(StructureDataPath + "Obelisk") },
                { StructureIDs.Temple, content.Load<ObeliskXML.Generator>(StructureDataPath + "Temple") }
            };
        }

        //private static void LoadHousings()
        //{
        //    housings = new Dictionary<int, HousingData>
        //    {
        //        { StructureIDs.House, new HousingData(GetDrawData(StructureIDs.House), 4) }
        //    };
        //}

        #endregion
    }
}

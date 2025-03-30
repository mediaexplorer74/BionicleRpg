
// Type: GameManager.Database.Mapper
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;
using GameManager.GameObjects.Components;
using Microsoft.Data.Sqlite;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
//using System.Data.SQLite;


namespace GameManager.Database
{
  public class Mapper
  {
    public List<CharacterData> MapCharactersFromReader(SqliteDataReader reader)
    {
      List<CharacterData> characterDataList = new List<CharacterData>();
      while (reader.Read())
      {
        int int32_1 = reader.GetInt32(0);
        string str = reader.GetString(1);
        int int32_2 = reader.GetInt32(2);
        int int32_3 = reader.GetInt32(3);
        int int32_4 = reader.GetInt32(4);
        characterDataList.Add(new CharacterData()
        {
          CharacterID = int32_1,
          Name = str,
          Element = (Element) int32_2,
          CurrentWeapon = (AttackType) int32_3,
          CurrentMask = (MaskType) int32_4
        });
      }
      return characterDataList;
    }

    public List<SavegameData> MapSavegameFromReader(SqliteDataReader reader)
    {
      List<SavegameData> savegameDataList = new List<SavegameData>();
      while (reader.Read())
      {
        int int32_1 = reader.GetInt32(0);
        int int32_2 = reader.GetInt32(1);
        float num1 = reader.GetFloat(2);
        float num2 = reader.GetFloat(3);
        float x = reader.GetFloat(4);
        float y = reader.GetFloat(5);
        savegameDataList.Add(new SavegameData()
        {
          ID = int32_1,
          WorldSeed = int32_2,
          Health = num1,
          Energy = num2,
          WorldPosition = new Vector2(x, y)
        });
      }
      return savegameDataList;
    }
  }
}

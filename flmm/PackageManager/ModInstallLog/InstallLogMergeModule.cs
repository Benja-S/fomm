﻿using System;
using System.Collections.Generic;

namespace Fomm.PackageManager.ModInstallLog
{
  /// <summary>
  ///   A mod's install log tracks which files were installed as a
  ///   pasrt of a specific mod. This is used when the mod is uninstalled.
  /// </summary>
  public class InstallLogMergeModule : InstallLogBase
  {
    /// <summary>
    ///   Describes an edit to an Ini file.
    /// </summary>
    internal class IniEdit : IComparable<IniEdit>
    {
      #region Properties

      /// <summary>
      ///   Gets the file that was edited.
      /// </summary>
      /// <value>The file that was edited.</value>
      public string File { get; private set; }

      /// <summary>
      ///   Gets the section in the file that was edited.
      /// </summary>
      /// <value>The section in the file that was edited.</value>
      public string Section { get; private set; }

      /// <summary>
      ///   Gets the key in the file that was edited.
      /// </summary>
      /// <value>The key in the file that was edited.</value>
      public string Key { get; private set; }

      /// <summary>
      ///   Gets or sets the value to which the key was set.
      /// </summary>
      /// <value>The value to which the key was set.</value>
      public string Value { get; set; }

      #endregion

      #region Constructors

      /// <summary>
      ///   A simple constructor that initializes the object with the given values.
      /// </summary>
      /// <param name="p_strFile">The Ini file that was edited.</param>
      /// <param name="p_strSection">The section in the Ini file that was edited.</param>
      /// <param name="p_strKey">The key in the Ini file that was edited.</param>
      public IniEdit(string p_strFile, string p_strSection, string p_strKey)
      {
        File = p_strFile;
        Section = p_strSection;
        Key = p_strKey;
      }

      #endregion

      #region IComparable<IniEdit> Members

      /// <summary>
      ///   Compares this IniEdit to the given IniEdit.
      /// </summary>
      /// <remarks>
      ///   Two IniEdit objects can be strictly ordered by
      ///   the following properties in the following order:
      ///   File, Section, Key
      /// </remarks>
      /// <param name="other">The IniEdit to which to compare this IniEdit.</param>
      /// <returns>
      ///   A value less than zero if this instance is less than the given instance,
      ///   or a value of zero  if this instance is equal to the given instance,
      ///   or a value greater than zero if this instance is greater than the given
      ///   instance.
      /// </returns>
      public int CompareTo(IniEdit other)
      {
        var intResult = File.CompareTo(other.File);
        if (intResult == 0)
        {
          intResult = Section.CompareTo(other.Section);
          if (intResult == 0)
          {
            intResult = Key.CompareTo(other.Key);
          }
        }
        return intResult;
      }

      #endregion
    }

    /// <summary>
    ///   Describes an edit to a game-specific value.
    /// </summary>
    internal class GameSpecificValueEdit : IComparable<GameSpecificValueEdit>
    {
      #region Properties

      /// <summary>
      ///   Gets the key of the value that was edited.
      /// </summary>
      /// <value>The key of the value that was edited.</value>
      public string Key { get; private set; }

      /// <summary>
      ///   Gets or sets the data to which the game-specific value was set.
      /// </summary>
      /// <value>The data to which the game-specific value was set.</value>
      public byte[] Data { get; set; }

      #endregion

      #region Constructors

      /// <summary>
      ///   A simple constructor that initializes the object with the given values.
      /// </summary>
      /// <param name="p_strKey">The key of the game-specific value that was edited.</param>
      public GameSpecificValueEdit(string p_strKey)
      {
        Key = p_strKey;
      }

      #endregion

      #region IComparable<OtherEdit> Members

      /// <summary>
      ///   Compares this <see cref="GameSpecificValueEdit" /> to the given <see cref="GameSpecificValueEdit" />.
      /// </summary>
      /// <remarks>
      ///   Two <see cref="GameSpecificValueEdit" /> objects can be strictly ordered by
      ///   the keys of the edits.
      /// </remarks>
      /// <param name="other">
      ///   The <see cref="GameSpecificValueEdit" /> to which to compare this
      ///   <see cref="GameSpecificValueEdit" />.
      /// </param>
      /// <returns>
      ///   A value less than zero if this instance is less than the given instance,
      ///   or a value of zero  if this instance is equal to the given instance,
      ///   or a value greater than zero if this instance is greater than the given
      ///   instance.
      /// </returns>
      public int CompareTo(GameSpecificValueEdit other)
      {
        var intResult = Key.CompareTo(other.Key);
        return intResult;
      }

      #endregion
    }

    #region Properties

    /// <summary>
    ///   Gets the list of data files installed during an install.
    /// </summary>
    /// <value>The list of data files installed during an install.</value>
    internal List<string> DataFiles { get; private set; }

    /// <summary>
    ///   Gets the list of original data files that were overwritten.
    /// </summary>
    /// <value>The list of original  data files that were overwritten.</value>
    internal List<string> ReplacedOriginalDataFiles { get; private set; }

    /// <summary>
    ///   Gets the list of Ini edits performed during an install.
    /// </summary>
    /// <value>The list of Ini edits performed during an install.</value>
    internal List<IniEdit> IniEdits { get; private set; }

    /// <summary>
    ///   Gets the list of original Ini values that were overwritten.
    /// </summary>
    /// <value>The list of original Ini values that were overwritten.</value>
    internal List<IniEdit> ReplacedOriginalIniValues { get; private set; }

    /// <summary>
    ///   Gets the list of game-specifc value edits performed during an install.
    /// </summary>
    /// <value>The list of game-specifc value edits performed during an install.</value>
    internal List<GameSpecificValueEdit> GameSpecificValueEdits { get; private set; }

    /// <summary>
    ///   Gets the list of original game-specifc values that were overwritten.
    /// </summary>
    /// <value>The list of original game-specifc values that were overwritten.</value>
    internal List<GameSpecificValueEdit> ReplacedGameSpecificValueData { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    ///   A simple constructor that initializes the object.
    /// </summary>
    public InstallLogMergeModule()
    {
      DataFiles = new List<string>();
      ReplacedOriginalDataFiles = new List<string>();
      IniEdits = new List<IniEdit>();
      ReplacedOriginalIniValues = new List<IniEdit>();
      GameSpecificValueEdits = new List<GameSpecificValueEdit>();
      ReplacedGameSpecificValueData = new List<GameSpecificValueEdit>();
    }

    #endregion

    /// <summary>
    ///   Performs a case insensitive search.
    /// </summary>
    /// <param name="p_lstValues">The list though which to search.</param>
    /// <param name="p_strSearchString">The value for which to search.</param>
    /// <returns>
    ///   <lang langref="true" /> if the value is found in the list;
    ///   <lang langref="false" /> otherwise.
    /// </returns>
    private bool ListContains(List<string> p_lstValues, string p_strSearchString)
    {
      var strLoweredSearchString = p_strSearchString.ToLowerInvariant();
      for (var i = p_lstValues.Count - 1; i >= 0; i--)
      {
        if (p_lstValues[i].ToLowerInvariant().Equals(strLoweredSearchString))
        {
          return true;
        }
      }
      return false;
    }

    #region File Management

    /// <summary>
    ///   Determins if this merge module contains the specified file.
    /// </summary>
    /// <param name="p_strDataPath">The file for whose presence in this merge module will be determined.</param>
    /// <returns>
    ///   <lang langref="true" /> if the specified file is in this merge module;
    ///   <lang langref="false" /> otherwise.
    /// </returns>
    internal bool ContainsFile(string p_strDataPath)
    {
      var strNormalizedPath = NormalizePath(p_strDataPath);
      return ListContains(DataFiles, strNormalizedPath);
    }

    /// <summary>
    ///   Adds the given file to the mod install log.
    /// </summary>
    /// <remarks>
    ///   Adding a file to a mod's install log indicates that said file was installed
    ///   as part of the mod.
    /// </remarks>
    /// <param name="p_strDataPath">The file that was installed for the mod.</param>
    internal void AddFile(string p_strDataPath)
    {
      var strNormalizedPath = NormalizePath(p_strDataPath);
      if (!ListContains(DataFiles, strNormalizedPath))
      {
        DataFiles.Add(strNormalizedPath);
      }
    }

    /// <summary>
    ///   Adds the given original data file to the mod install log.
    /// </summary>
    /// <remarks>
    ///   This backs up an original data file we are overwriting.
    /// </remarks>
    /// <param name="p_strDataPath">The file that was overwritten.</param>
    internal void BackupOriginalDataFile(string p_strDataPath)
    {
      if (!ListContains(ReplacedOriginalDataFiles, p_strDataPath))
      {
        ReplacedOriginalDataFiles.Add(p_strDataPath);
      }
    }

    #endregion

    #region Ini Management

    /// <summary>
    ///   Adds the given Ini edit to the mod install log.
    /// </summary>
    /// <remarks>
    ///   Adding an Ini edit to a mod's install log indicates that said edit was made
    ///   as part of the mod.
    /// </remarks>
    /// <param name="p_strFile">The Ini file that was edited.</param>
    /// <param name="p_strSection">The section in the Ini file that was edited.</param>
    /// <param name="p_strKey">The key in the Ini file that was edited.</param>
    /// <param name="p_strValue">The value to which the key was set.</param>
    internal void AddIniEdit(string p_strFile, string p_strSection, string p_strKey, string p_strValue)
    {
      var strLoweredFile = p_strFile.ToLowerInvariant();
      var strLoweredSection = p_strSection.ToLowerInvariant();
      var strLoweredKey = p_strKey.ToLowerInvariant();
      var iniEdit = new IniEdit(strLoweredFile, strLoweredSection, strLoweredKey);
      var intIndex = IniEdits.IndexOf(iniEdit);
      if (intIndex == -1)
      {
        IniEdits.Add(iniEdit);
      }
      else
      {
        iniEdit = IniEdits[intIndex];
      }
      iniEdit.Value = p_strValue;
    }

    /// <summary>
    ///   Adds the given original Ini value to the mod install log.
    /// </summary>
    /// <remarks>
    ///   This backs up an original Ini value we are overwriting.
    /// </remarks>
    /// <param name="p_strFile">The Ini file that was edited.</param>
    /// <param name="p_strSection">The section in the Ini file that was edited.</param>
    /// <param name="p_strKey">The key in the Ini file that was edited.</param>
    /// <param name="p_strValue">The original value of the edited key.</param>
    internal void BackupOriginalIniValue(string p_strFile, string p_strSection, string p_strKey, string p_strValue)
    {
      var strLoweredFile = p_strFile.ToLowerInvariant();
      var strLoweredSection = p_strSection.ToLowerInvariant();
      var strLoweredKey = p_strKey.ToLowerInvariant();
      var iniEdit = new IniEdit(strLoweredFile, strLoweredSection, strLoweredKey);
      var intIndex = ReplacedOriginalIniValues.IndexOf(iniEdit);
      if (intIndex == -1)
      {
        ReplacedOriginalIniValues.Add(iniEdit);
      }
      else
      {
        iniEdit = ReplacedOriginalIniValues[intIndex];
      }
      iniEdit.Value = p_strValue;
    }

    #endregion

    #region Game-Specific Value Management

    /// <summary>
    ///   Adds the given <see cref="GameSpecificValueEdit" /> to the mod install log.
    /// </summary>
    /// <remarks>
    ///   Adding a <see cref="GameSpecificValueEdit" /> to a mod's install log indicates that said edit was made
    ///   as part of the mod.
    /// </remarks>
    /// <param name="p_strKey">The key of the value that was edited.</param>
    /// <param name="p_bteData">The data to which the value was set.</param>
    internal void AddGameSpecificValueEdit(string p_strKey, byte[] p_bteData)
    {
      var strLoweredKey = p_strKey.ToLowerInvariant();
      var gseEdit = new GameSpecificValueEdit(strLoweredKey);
      var intIndex = GameSpecificValueEdits.IndexOf(gseEdit);
      if (intIndex == -1)
      {
        GameSpecificValueEdits.Add(gseEdit);
      }
      else
      {
        gseEdit = GameSpecificValueEdits[intIndex];
      }
      gseEdit.Data = p_bteData;
    }

    /// <summary>
    ///   Adds the given original data of the a game-specific value to the mod install log.
    /// </summary>
    /// <remarks>
    ///   This backs up the original data of the a game-specific value we are overwriting.
    /// </remarks>
    /// <param name="p_strKey">The key of the value that was edited.</param>
    /// <param name="p_bteData">The original data of the edited value.</param>
    internal void BackupOriginalGameSpecificValueEdit(string p_strKey, byte[] p_bteData)
    {
      var strLoweredKey = p_strKey.ToLowerInvariant();
      var oetEdit = new GameSpecificValueEdit(strLoweredKey);
      var intIndex = ReplacedGameSpecificValueData.IndexOf(oetEdit);
      if (intIndex == -1)
      {
        ReplacedGameSpecificValueData.Add(oetEdit);
      }
      else
      {
        oetEdit = ReplacedGameSpecificValueData[intIndex];
      }
      oetEdit.Data = p_bteData;
    }

    #endregion
  }
}
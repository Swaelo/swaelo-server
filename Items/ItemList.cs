﻿// ================================================================================================================================
// File:        ItemList.cs
// Description: Lists every item available in the game
// ================================================================================================================================

using Server;
using Server.Items;
using System;
using System.Collections.Generic;

//Contains helper functions for getting other data about an item from giving only its ItemNumber
public static class ItemList
{
    //Master list of every item currently available in the game, sorted into a dictionary by their ItemNumber values
    public static Dictionary<int, ItemData> MasterItemList = new Dictionary<int, ItemData>();

    //Loads all the items information from text file
    public static void InitializeItemList()
    {
        //Open the item list export file
        string[] FileLines = System.IO.File.ReadAllLines("C:/mmo-client/Assets/Exports/MasterItemList.txt");

        //Loop through all the lines, processing one at a time
        foreach(string Line in FileLines)
        {
            //Split each line with its : seperators
            string[] LineSplit = Line.Split(':');

            //Extract all the items data into a new ItemData object
            ItemData NewItem = new ItemData();
            NewItem.ItemName = LineSplit[0];
            NewItem.ItemDisplayName = LineSplit[1];
            NewItem.ItemType = FindItemType(LineSplit[2]);
            NewItem.ItemEquipmentSlot = FindItemSlot(LineSplit[3]);
            NewItem.ItemNumber = Int32.Parse(LineSplit[4]);

            //Store the new item definition into the dictionary with all the others
            MasterItemList.Add(NewItem.ItemNumber, NewItem);
        }
    }

    //Returns the ItemData component from an ItemNumber value
    public static ItemData GetItemData(int ItemNumber)
    {
        if (!MasterItemList.ContainsKey(ItemNumber))
            return null;

        return MasterItemList[ItemNumber];
    }

    //Returns the correct item type value from the partial string read out from the file importing
    private static ItemType FindItemType(string ItemTypeValue)
    {
        if (ItemTypeValue == "Consumable")
            return ItemType.Consumable;
        else if (ItemTypeValue == "Equipment")
            return ItemType.Equipment;
        else if (ItemTypeValue == "AbilityGem")
            return ItemType.AbilityGem;

        return ItemType.NULL;
    }

    //Returns the correct equipment slot value from the partial string read out from the file importing
    private static EquipmentSlot FindItemSlot(string ItemSlotValue)
    {
        if (ItemSlotValue == "Head")
            return EquipmentSlot.Head;
        else if (ItemSlotValue == "Back")
            return EquipmentSlot.Back;
        else if (ItemSlotValue == "Neck")
            return EquipmentSlot.Neck;
        else if (ItemSlotValue == "LeftShoulder")
            return EquipmentSlot.LeftShoulder;
        else if (ItemSlotValue == "RightShoulder")
            return EquipmentSlot.RightShoulder;
        else if (ItemSlotValue == "Chest")
            return EquipmentSlot.Chest;
        else if (ItemSlotValue == "LeftGlove")
            return EquipmentSlot.LeftGlove;
        else if (ItemSlotValue == "RightGlove")
            return EquipmentSlot.RightGlove;
        else if (ItemSlotValue == "Legs")
            return EquipmentSlot.Legs;
        else if (ItemSlotValue == "LeftHand")
            return EquipmentSlot.LeftHand;
        else if (ItemSlotValue == "RightHand")
            return EquipmentSlot.RightHand;
        else if (ItemSlotValue == "LeftFoot")
            return EquipmentSlot.LeftFoot;
        else if (ItemSlotValue == "RightFoot")
            return EquipmentSlot.RightFoot;

        return EquipmentSlot.NULL;
    }

    //Tells you the name of an item from only having its item number
    public static string GetItemName(int ItemNumber)
    {
        if (!MasterItemList.ContainsKey(ItemNumber))
            return null;

        return MasterItemList[ItemNumber].ItemName;
    }

    //Tells you what type an item is
    public static string GetItemType(int ItemNumber)
    {
        //1-2 are potions/consumables
        if (ItemNumber == 1 || ItemNumber == 2)
            return "Consumable";
        //3-20 are all equipment items
        else if (ItemNumber > 2 && ItemNumber < 21)
            return "Equipment";
        //21-23 are ability gems
        else if (ItemNumber == 21 || ItemNumber == 22)
            return "Ability Gem";

        //The rest are undefined / outlier values
        return "NULL";
    }

    //Tells you what EquipmentSlot an item belongs to
    public static EquipmentSlot GetItemEquipmentSlot(int ItemNumber)
    {
        //1-2 are potions
        if (ItemNumber == 1 || ItemNumber == 2)
            return EquipmentSlot.NULL;
        //3-4 are main hand weapons
        else if (ItemNumber == 3 || ItemNumber == 4)
            return EquipmentSlot.RightHand;
        //5 is a shield for off hand
        else if (ItemNumber == 5)
            return EquipmentSlot.LeftHand;
        //6 is helmets
        else if (ItemNumber == 6)
            return EquipmentSlot.Head;
        //7-8 is left/right shoulders
        else if (ItemNumber == 7)
            return EquipmentSlot.LeftShoulder;
        else if (ItemNumber == 8)
            return EquipmentSlot.RightShoulder;
        //9-10 is left/right gloves
        else if (ItemNumber == 9)
            return EquipmentSlot.LeftGlove;
        else if (ItemNumber == 10)
            return EquipmentSlot.RightGlove;
        //11 is amulets
        else if (ItemNumber == 11)
            return EquipmentSlot.Neck;
        //12 is cloaks
        else if (ItemNumber == 12)
            return EquipmentSlot.Back;
        //13-14 is chest pieces
        else if (ItemNumber == 13 || ItemNumber == 14)
            return EquipmentSlot.Chest;
        //15-16 is leggings
        else if (ItemNumber == 15 || ItemNumber == 16)
            return EquipmentSlot.Legs;
        //17-18 is left feet
        else if (ItemNumber == 17 || ItemNumber == 18)
            return EquipmentSlot.LeftFoot;
        //19-20 is right feet
        else if (ItemNumber == 19 || ItemNumber == 20)
            return EquipmentSlot.RightFoot;
        //The rest are ability gems
        else
            return EquipmentSlot.NULL;
    }
}
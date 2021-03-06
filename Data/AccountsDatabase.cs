﻿// ================================================================================================================================
// File:        AccountsDatabase.cs
// Description: Allows the server to interact with the local SQL database accounts tables
// ================================================================================================================================

using MySql.Data.MySqlClient;

namespace Server.Data
{
    class AccountsDatabase
    {
        //Checks if the given account name is available for use or if its already been taken by someone else
        public static bool IsAccountNameAvailable(string AccountName)
        {
            //Define the query to search for an account with this name in the database
            string AccountQuery = "SELECT * FROM accounts WHERE Username='" + AccountName + "'";

            //Execute the command and start reading from the accounts table
            MySqlCommand AccountCommand = new MySqlCommand(AccountQuery, Database.Connection);
            MySqlDataReader AccountReader = AccountCommand.ExecuteReader();

            //Read from the table and check if any account exists with the given given account name
            AccountReader.Read();
            bool AccountNameAvailable = !AccountReader.HasRows;
            AccountReader.Close();

            return AccountNameAvailable;
        }

        //Saves a brand new user account into the database
        //NOTE: Assumes this account doesnt already exist and the login credentials provided are valid
        public static void RegisterNewAccount(string AccountName, string AccountPassword)
        {
            string RegisterQuery = "INSERT INTO accounts(Username,Password) VALUES('" + AccountName + "','" + AccountPassword + "')";
            MySqlCommand RegisterCommand = new MySqlCommand(RegisterQuery, Database.Connection);
            RegisterCommand.ExecuteNonQuery();
        }

        //Checks if the account name and password provided are valid login credentials
        //NOTE: Assumes this account already exists
        public static bool IsPasswordCorrect(string AccountName, string AccountPassword)
        {
            //Define the query to check this accounts login credentials
            string PasswordQuery = "SELECT * FROM accounts WHERE Username='" + AccountName + "' AND Password='" + AccountPassword + "'";

            //Execute this command to open up the table for this account name
            MySqlCommand PasswordCommand = new MySqlCommand(PasswordQuery, Database.Connection);
            MySqlDataReader PasswordReader = PasswordCommand.ExecuteReader();

            //Read the table to check if the password provided is correct
            PasswordReader.Read();
            bool PasswordMatches = PasswordReader.HasRows;
            PasswordReader.Close();

            //Return the final value
            return PasswordMatches;
        }
    }
}

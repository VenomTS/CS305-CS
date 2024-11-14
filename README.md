# Banking System
A console-based application written in C# (Version 12) that uses JSON as a database with a goal of simulating a bank and user's interactions with the bank

## Table of Contents
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Goals](#goals)
- [Features](#features)

## Requirements <a name="requirements"></a>
.NET Runtime 8.0 - [Download Here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) <br/>
(Optional) File Manager - [Download Here (7zip)](https://www.7-zip.org/) <br/>
(Optional) Git - [Download Here](https://git-scm.com/downloads) <br/>
(Optional) IDE for C# - [Download Here (Rider)](https://www.jetbrains.com/rider/download/)

## Installation <a name="installation"></a>
Way 1:
1. Download .NET Runtime 8.0 from [Requirements](#requirements)
2. Download the latest release of the program from [Here](https://github.com/VenomTS/CS305-CS/releases)
3. Extract the downloaded program using file manager [Requirements](#requirements)
4. Run the "Banking System.exe" inside the "Banking System" folder that you extracted

 Way 2:
 1. Download Git from [Requirements](#requirements)
 2. Download IDE of your choosing ([Rider](#requirements))
 3. Go to your terminal and execute the following command ```git clone https://github.com/VenomTS/CS305-CS```
 4. Open your IDE and select 'Open an Existing Project'
 5. Locate the clone of the repository
 6. Open the "Banking System.sln"
 7. Run the program in your IDE

## Usage
Once you run the program, the terminal will open with four options. <br/>
One of the options will be in ![#00FF00](https://placehold.co/15x15/00ff00/00ff00.png) **GREEN** color while the rest will be in ![#FF0000](https://placehold.co/15x15/ff0000/ff0000.png) **RED** color. <br/>
The ![#00FF00](https://placehold.co/15x15/00ff00/00ff00.png) **GREEN** option is where your ***CURSOR*** is. <br/>
To move to the other options, use either **&#8592;** or **&#8594;**. <br/>
When you want to select the option, press **ENTER**. <br/>
In case where you need to input a text, the terminal will print out all the instructions on what and how to input. <br/>
For this type of input, use your keyboard and follow the instructions on the screen.

## Goals
The goal of the project is to simulate the user's interaction with a bank. <br/>
Technical goals are (In-Depth Explanation in [Features](#features)): <br/>
**1. Account management through a database** <br/>
**2. Authentication system** <br/>
**3. Allow transactions and keep track of all transactions** <br/>
**4. Freezing and Unfreezing account on demand** <br/>

## Features
All the implemented features are tied to technical goals mentioned in [Goals](#goals) <br/><br/>
**1. Account management through a database** <br/>
   The system allows for creating accounts and stores all of the account's data in a database <br/>
   Every change that happens with the account is stored in a database and updated after each usage of the system, thus allowing the most accurate data to be present in the database at all times <br/><br/>
**2. Authentication system** <br/>
   All accounts must be secured in such a way that no malicious actor can access them and withdraw or transfer funds to other accounts <br/>
   For this purpose, all accounts require a 4 digit pin that only user knows. <br/>
   Every time user tries to login, they are asked their 4 digit pin, if they do not know it, they cannot access the account <br/><br/>
**3. Allow transactions and keep track of all transactions** <br/>
   The system allows three types of transactions, withdrawal of funds, deposition of funds and transfering funds from one account to another <br/>
   Each of these actions deal with money, thus they require high levels of precision and the system needs to accurately keep track of all the transactions that ever happened <br/>
   In case of malicious transactions, there is a log in the database that shows the time at which transaction happened, type of transaction and, in case of transfers, who the money went to <br/><br/>
**4. Freezing and Unfreezing account on demand** <br/>
   Malicious actors are all around, and it is not a question of ***IF you will be hacked***, but ***WHEN you will be hacked***. <br/>
   In case of a malicious actor obtaining your 4 digit pin, you can always go to your account and freeze it, thus preventing all types of transactions<br/>
   If the account is frozen accidentally, there is an option to unfreeze it too

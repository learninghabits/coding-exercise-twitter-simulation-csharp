# Coding Exercise (Twitter Simulation)

The aim of this application is to simulate twitter feeds using files. The challenge is to read raw data from a text file, format it as objects, and display an aggregated list of tweets for each user (twitter handle) and people they follow. 

## Content  

* Input
* Output
* Assumptions
* Requirements
* Running the application

## Input  

Input to the application consists of two files. The users file and the tweets file.  

### The users file 

A well-formed line will follow a pattern like this: [USER follows A, B, C] where USER represents a twitter handle and A, B,C represents a list of twitter handles they follow.   
   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; e.g. @Tshepo follows @Jeff, @Travis, @UncleBob

### The tweets file

A well-formed line will follow a patter like this: [USER> MESSAGE] where USER represents a twitter handler and a MESSAGE represents a message (of up to 140 chars) they tweeted.

 ## Output  

 The output of the application is a result of doing the following:
 
 1. Matching users and their tweets
 2. Grouping user's tweets and their follows' tweets
 3. Displaying the grouped data on a console.  

### Output format

USER   
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<tab> USER: MESSAGE   
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<tab> USER: MESSAGE   
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<tab> USER: MESSAGE   
USER   
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<tab> USER: MESSAGE   
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<tab> USER: MESSAGE   
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<tab> USER: MESSAGE   

## Assumptions  

1. All users will be twitter handles
2. The application may skip an invalid line (for users and tweets) and continue processing valid ones
3. Exceptions are to be handled at the top most object
4. Localization is not required. Error messages are hard-coded in the application.
5. The CI server can download packages via Nuget (Server has access to the internet / a nuget server on the intranet)
6. UNC, SFTP, FTP and other file locations that may require access to the network to read a file is not supported.

## Requirements  

 > .NET 4.6.1

## Running the application  

1. Rebuild application
2. Navigate to your local GIT directory (folder), bin\Debug folder
2. Open the the command line  
4. type command : App  users.txt tweets.txt

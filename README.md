# RozWorld-Server-Exec
Dedicated server console for running RozWorld servers.

**This is a work in progress, as with other parts of RozWorld, commits will be in the 'pre-dev' branch until enough is finished to warrant a newer branch**

## What is this
This is a really simple console application that uses the RozWorld server library, with the logger set to output to the console.

## Building this ting
Download everything, make sure all the references to the RozWorld-API and RozWorld-Server are correct and make sure you also have the Oddmatics-IO library set correctly too in the other projects. If you encounter build errors, make sure the *release* build of Oddmatics-IO is in the output directory of this project, once it is, the program should build and run correctly.
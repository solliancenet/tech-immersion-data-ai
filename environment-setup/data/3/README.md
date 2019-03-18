# Azure SQL Managed Instance setup

Complete the steps below to prepare the environment for the [Day 1, Experience 3](../../../day1-exp3/) lab.

## Pre-requisites

  - General - Prepare instructions for 1 SQL MI cluster shared amongst multiple attendees, where read-only databases are shared and read/write/configure databases are created one per attendee. Also, one ASE shared by all attendees.
  - The following configuration needs to be applied to the SQL MI cluster.
  - The following databases are used:
    - `ContosoAutoDb` running on SQL Server 2008 R2 in VM. Attendees should have read-only access.
    - `ContosoAutoDb` running on the SQL MI. Attendees should have read-only access.
    - `ContosoAutoDb-XXXX` running on the SQL MI. Attendees should have read-only access.
  - An Application Service Environment
    - One ASE shared by all attendees
  - A per attendee Web App in the ASE that is pre-connected to the SQL MI VNET:
    - Name: techimmersion-XXXXX (where XXXX is the attendee’s unique ID)
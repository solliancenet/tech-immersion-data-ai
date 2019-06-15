# Open source databases at scale setup

Complete the steps below to prepare the environment for the [Day 1, Experience 5](../../../day1-exp5/README.md) lab.

## Pre-requisites

- Each attendee needs a resource group with:
  - Azure Database for PostgreSQL Hyperscale (Citus) server group

A computer or VM the attendee will use for the lab requires the following:

- [pgAdmin](https://www.pgadmin.org/download/) 4 or greater

## PostgreSQL configuration

Fill out the new server details form with the following information:

- Server group name: enter a unique name for the new server group, such as **ti-postgres-SUFFIX**, which will also be used for a server subdomain.
- Admin username: currently required to be the value **citus**, and can't be changed.
- Password: **Record the password and make it available to attendees**.
- Location: use the location you provided when creating the resource group, or the closest available.
- Configure server group: leave the settings in this section unchanged.

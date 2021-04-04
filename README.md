# Lackbot

Lackbot is a Discord bot created for my private Discord server, although if for some reason you are
interested then there is no reason you could not host it for use on other servers.


## Overview

Lackbot is a fairly simple Discord bot in functionality, although the implementation is more elaborate.
The bot itself has three primary functions:

* Auto-Responses: Automatically respond to matching messages with a customized response.
  This is a loose imitation of Slackbot, although it has grown to be more customizable.
  Matching criteria can include strings in the message, the time that the message is sent,
  and later may include the author and channel that the message was sent by/in.
 
* Auto-Reacts: Automatically react to matching messages with a customized emoji.
  Matching criteria can include strings in the message, the author, and later may
  include the time or channel that the message was sent in.
 
* Scheduled Messages: Automatically send messages at a configured time, to a configured channel.
  The schedules are defined by [cron expressions](https://crontab.guru).

There are also a couple commands that will likely be expanded over time. Currently there is
a rock paper scissors command, as well as a dice rolling command.

### Tech Stack

* **API:** ASP.NET Core 5.0

* **Database:** MongoDB

* **Bot:** .NET 5

* **Interface:** Angular


## Installation

**Requirements:** Docker, [a Discord bot token](https://www.writebots.com/discord-bot-token/), ability to forward ports (for live usage)

1. Clone the repository: `git clone https://www.github.com/las6731/lackbot.net.git`

2. Set up configs

    2.1. Open `bot_config.json` in the text editor of your choice, and replace `YOUR TOKEN HERE` with your bot token.

    2.2. Open `mongo/mongo-init.js` in the text editor of your choice, and change the username/password to whatever you want.
         Update the credentials in `db_credentials.json` accordingly. You should also change the credentials for the root DB
         account; changes will need to be made in `mongo/docker-compose.yaml` and `mongo/mongo-init.js` accordingly.

3. Forward ports 4200 and 5000 on your router, for the frontend and the API respectively.

4. Start the database: `cd mongo && docker-compose up -d`. Open Mongo Express at http://localhost:8081 to confirm it is running.

5. Start the bot: `cd .. && docker-compose up -d`
   (Note: this will take a while for the first time, as docker builds all the necessary images. Subsequent builds will be much quicker.)

6. Open the frontend at http://localhost:4200 to confirm it is running. The bot should also be online in the server you added the bot to.

7. To pull changes and update: `git pull && docker-compose build && docker-compose up -d`


## Contributing

I don't expect anyone to contribute to the bot, but if you _do_ want to then here are some brief instructions:

**Requirements:** Docker, [a Discord bot token](https://www.writebots.com/discord-bot-token/), an IDE, .NET SDK

1. Follow steps 1, 2.2, and 4 above to set up the database.

2. Open the solution in your preferred IDE. Build the solution, and run `LackBot.Discord`, to generate a config template.

3. Update the config file in `LackBot.Discord/bin/Debug/net5.0/` with your bot token.

4. Copy your `db_credentials.json` config into `LackBot.API/`

5. Run `LackBot.API`, then re-run `LackBot.Discord`. Both should come up successfully.

6. Install npm packages: `cd frontend && npm install`

7. Install Angular CLI: `npm install -g @angular/cli`

8. Run frontend: `ng serve`

Now that your local environment is up and running, you can start working on your changes.
When you're done, create a pull request and I'll review it!

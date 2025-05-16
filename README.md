# LVK.Pushover

[![build](https://github.com/lassevk/LVK.Pushover/actions/workflows/build.yml/badge.svg)](https://github.com/lassevk/LVK.Pushover/actions/workflows/build.yml)
[![codecov](https://codecov.io/gh/lassevk/LVK.Pushover/graph/badge.svg?token=N58US136E7)](https://codecov.io/gh/lassevk/LVK.Pushover)
[![codeql](https://github.com/lassevk/LVK.Pushover/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/lassevk/LVK.Pushover/actions/workflows/github-code-scanning/codeql)

This library add a comprehensive client for the [pushover.net](https://LVK.Pushover)
push notification service to .NET.

## Installation

Install the [LVK.Pushover](https://www.nuget.org/packages/LVK.Pushover) package using your
favorite nuget package tool, or the command line:

    dotnet nuget install LVK.Pushover

## Configuration

Assuming you're in a project using the Microsoft.Extensions.DependencyInjection and
Microsoft.Extensions.Hosting packages, you can add the client to your project with this
statement. Add it to your Program.cs file:

    builder.Services.AddPushoverClient(options => options
        .WithApiToken(builder.Configuration["Pushover:ApiToken"]));
   
## Usage

Simply inject the `IPushoverClient` into your normal classes and call methods on it to send
notifications, here is an example:

    public class SomeService
    {
        private readonly IPushoverClient _pushoverClient;
        public SomeService(IPushoverClient pushoverClient)
        {
            _pushoverClient = pushoverClient;
        }

        public async Task SomeMethod()
        {
            await _pushoverClient.SendMessageAsync(msg => msg
                .WithMessage("Hello from your application")
                .WithTitle("Well hello there!")
                .WithRecipient("your-unique-user-key"));
        }
    }

## Features

Supports the following Pushover features:

* Send to one or multiple users or user groups (defined on LVK.Pushover web site)
* Send to default or specific device(s)
* Html formatting, Monospace formatting
* Override message timestamp
* Specify time-to-live, where message will be deleted on users device after this time
* Message priority, from lowest to emergency
* Specify emergency message retry interval, expiration, and optional callback webhook
* Notification sounds, from built-in to custom
* Attachments
* Validate user key and/or devices
* Check receipts from emergency messages, to see if user has acknowledged the message yet
* Cancel emergency messages either by their receipt id or one of their tags

# Future features

There are more features available through the Pushover API, and I will look into adding
more of these in the future.

Specifically, these features looks interesting:

* Subscription API
* Groups API
* Glances API
* Teams API
* Licensing API

You can read more about these in the API documentation [here](https://LVK.Pushover/api).

If there are APIs here that you would like to have now, please create an issue and
describe your usecase, and I might prioritize getting it implemented.

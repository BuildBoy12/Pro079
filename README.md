# Pro079
A plugin to enhance playing 079 by giving it some fancy commands to play with, becuase the whole spanish community agreed that playing SCP-079 is boring, this plugin gives him more stuff to not be bored to literal death.

# Configs

**Check the wiki to get a detailed explanation of every single config here:** https://github.com/BuildBoy12/pro079/wiki

# Translations
These will auto-generate in the Exiled config folder as well under their own category, and are free for you to modify as you please.

## Aliases
Each of the included modules have a x_cmd translation config, such as `teslacmd: te`, can be edited to change the base command.

Take into account these will not change what the `.079` command will state. For this reason, it is highly recommended to match your translations throughout your configs. If you're really going to modify it, it's because you're translating things like "suicide" to, let's say, "Selbstmord" in german, so you're probably going to have to end up checking everything. This is to give full control to the users to rearrange, change, etc. without sacrificing CPU to do 1000 checks/changes in the same frame, which could end up delaying the entire server.

# API Guide

This plugin features an API with which you can add new commands/ultimates via plugins (similar to gamemodes/itemmanager)
Please, check this one for reference, and use it as a template to get things done quick: https://github.com/BuildBoy12/Pro079Template

As well as this is a really poorly detailed template, it's the basics and you could explore everything else for yourself by checking the [Manager.cs](https://github.com/BuildBoy12/pro079/blob/master/pro079/Pro079Manager.cs) and [Config079.cs](https://github.com/BuildBoy12/pro079/blob/master/pro079/Config079.cs) classes themselves.

***

### Massive thanks to RogerFK, the original dev on this plugin, for making the framework for what it is now.
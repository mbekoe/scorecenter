# How to Upgrade your Settings to V2 #

## Convert the settings ##
### Before Upgrading ScoreCenter ###
If you have not upgraded ScoreCenter yet.
  * Open the configuration.
  * Export only the scores you want to upgrade in a XML file.

Download the upgrade program and use it to convert the settings. Note that it is a command tool:
> upgradesc <your export file>.xml
This file generate <your export file>_v2.xml_

### After Upgrading ScoreCenter ###
If you already installed the new version of ScoreCenter.
Retrieve the version 1 of the settings file MyScoreCenter.settings.xml from the MediaPortal data directory.
Since the import procedure will not let you choose the scores to import, I recommend that, if you can, you edit the file and manually remove all the scores you don't want to upgrade.

Download the upgrade program and use it to convert the settings. Note that it is a command tool:
> upgradesc <your export file>.xml
This file generate <your export file>_v2.xml_

## Import the converted settings ##
If you have not installed ScoreCenter v2, install it.
Start MediaPortal and open the plugin to import the online settings.
Then, exit MediaPortal and open the configuration screen and import the converted file (select only new scores it should be enough).
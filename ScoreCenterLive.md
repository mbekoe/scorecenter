# How it works #
The Live feature is implemented as a Process Plugin. Which runs all the time in MediaPortal.
To start the live, go to MyScoreCenter plugin and in the context menu select "Start Live".
This will create a file _MyScoreCenterLive.Settings.xml_ in the settings directory, which is an export of the settings containing only the scores configure as "live".

The creation of this file will start the live process which consist to every x minutes (see options), download the score and compare it to the previous one and show the difference in a notification dialog.

You can go back to MyScoreCenter to stop the plugin using the entry in the context menu. This will delete the live setting file and stop the live process.

# Configuration #
In the option dialog of the Configuration Screen, you will find the following parameters:
  * Notification Time: the number of seconds to show the notification dialog.
  * Check Every: the number of minutes between each verifications.
  * Play Sound: play a sound (see below) when showing the notification dialog.

To set a score as "live", you can either use the Configuration and check the option "Live", or in MediaPortal select the score in the list view and the enable/disable the live status by using the context menu.

Then you can configure the Live Display by giving an output format.
The format is a classic '.Net' format, for example if you want to show only column 1 and 3, the format is (without the quotes) '{0} {2}' (column index is 0-based).
If you want to show column 2, then 1 separeted by a dash use '{1} - {0}'.
If the format failed because of a typo or a missing column, ALL the columns will be displayed.

# Skin #
**Live Status Label**: the Property #ScoreCenter.Live is set by the plugin when the status changed is shows the status followed by the number of scores set as live, ex: "Live Off (1)".

**List View Pin Icon**: live scores are displayed in the list view with a PinIcon. It will use _`<skin>\Media\ScoreCenterLive.png`_ if it exists otherwise _`<thums>\ScoreCenter\Misc\Live.png`_.

**Notification Dialog**: by default, the MediaPortal Notification Dialog is used (DialogNotify.xml). But you can create a dedicated notification dialog: if the file _MyScoreCenterNotify.xml_ exists it will be used for the notification dialog (ID = 42010).

**Play Sound**: if the file _<skin folder>/Sounds/notifyscore.wav_ exists it will be played when the notification dialog is shown (if the option is enabled). Otherwise it will play _<skin folder>/Sounds/notify.wav_.
**version 2.12.0 (4 May 2014)**:
  * Add new type of score _FussballdeScore_ for fussball.de web site.
  * Add new actions _CutAfter_ et _CutBefore_ for rules.
  * Add parameter _TwoLegs_ for WF scores to get different sizes.
  * Refresh icon when set from the editor.

**version 2.11.0 (18 January 2014)**:
  * Fix issue with icon folder.
  * Update Default Wide Skin (thanks to catavolt).
  * Update Titan Skin live icon.
  * Add separate parameter for WorldFootball Qualification Level (WF.QualificationLevel).

**version 2.10.0 (7 January 2014)**:
  * Update for MP 1.6.
  * Remove HtmlAgilityPack (included in MP).

**version 2.9.0 (4 September 2013)**:
  * Fix issues with new layout of Worldfootball.
  * Upgrade HtmlAgilityPack to version 1.4.6.

**version 2.8.1.0 (19 June 2013)**:
  * Compatibility with MP1.4.

**version 2.8.0.0 (24 March 2013)**:
  * Skin files for Titan skin.
  * Fix issue with Live Score to use rules (Skip and Replace only).

**version 2.7.1.0 (23 September 2012)**:
  * Version compatible with MP1.3 alpha.

**version 2.7.0.0 (15 April 2012)**:
  * Add: New way to parse pages with no tables. First XPath expression is for lines and second (new field) is for columns.
  * Fix: Issue with HTML comments when detecting the current round for WorldFootball scores.

**version 2.6.1.0 (18 January 2012)**:
  * Fix: Issue with Regular Expression used to find current round for WorldFootball scores.

**version 2.6.0.0 (2 January 2012)**:
  * Fix: Issue with Home Score when using a WorldFootball scores.
  * Fix: Issue when no localization file exists for current language.
  * Improve WorldFootball results by using "Result only" page, which is updated faster (better for live).
  * Add: Auto Refresh score.
  * Add: Post-processing parameter to use "alt" attribute from _img_ tag or not.

**version 2.5.0.0 (9 December 2011)**:
  * Improve: WorldFootball scores analyzed which items to show (last results, ranking, scorer).
  * Add: WorldFootball possibility to navigate through rounds.
  * Add: WorldFootball add _Assist_, _Stadium_ and _Referee_.
  * Add: Localization for scores.
  * Add: Improve parsing to use "alt" attribute of _img_ tag when the column contains only images.
  * Add: Alternate color for rows.
  * Reorganize _Generic Score Editor_ with tabs.

**version 2.4.0.0 (30 October 2011)**:
  * Fix: Missing Icon when going to MediaPortal Home.
  * Fix: Focus is lost when going back.
  * Fix: Format Live Score before comparing them.
  * Fix: Update from config dialog does not refresh settings.
  * Fix: Exception in config dialog when adding new score to an empty settings.
  * Fix: When the settings is empty ask the user if he wants to download the online settings.
  * Add: Skin parameter #ScoreCenter.LiveOn to give Live status.
  * Add: Skin parameter #ScoreCenter.LiveIcon to show Live status as an icon.
  * Add: Disabled pin icon for score for which live is not set.
  * Remove: Save button for scores. Changes to scores are now auto saved.

**version 2.3.0.0 (24 September 2011)**:
  * Live Feature.
  * Add: WorldFootball scores, add a global "Results" for Tournament and Qualification type. (to use with the live feature).
  * Add: German scores localization (thanks to catavolt).

**version 2.2.0.2**:
  * Version for MP 1.2 RC

**version 2.2.0.0 (28 August 2011)**:
  * Add language files for Hungarian (thanks to vrm42).
  * WorldFootball Team: add Players and Transfers.
  * Fix: Change of "Cache Expiration" is not saved.
  * Fix: Selection is lost after browsing to a real score.
  * Fix: When starting on home screen Title and Source are not set.
  * Fix: It is not possible to change columns width for WorldFootball scores.
  * Fix: CacheExpiration is not working.

**version 2.1.0.0 (26 July 2011)**:
  * New type for Worldfootball _Team_ to follow a team (club or national).
  * Add 'Top Scorer History' for Leagues and Tournament.
  * It is now possible to add children to a Worldfootball score.

**version 2.0.0.0 (25 July 2011)**:
  * New structure for the XML settings.
  * Scores have a type. Possible types are Folder, Generic, Rss or WorldFootball.
  * WorldFootball type makes it easier to add a league from www.worldfootball.com.
  * Possibility to use parameters in URL.
  * New date tags for URL to indicate seasons.
  * New Rule: 'Is Last'

**version 1.4.0.0 (1 May 2011):**
  * Add compatibility check for MP 1.2 beta.
  * Replace Blue3 skins with new Default skins.
  * Upgrade to HtmlAgilityPack 1.4.

**version 1.3.1 (2 November 2010):**
  * Fix issue with XPath Element.

**version 1.3.0 (1 November 2010):**
  * Add German language file (thanks to catavolt).
  * Add option to include or exclude (default) table's caption.
  * 'XPath Element' fields now allows more than one indice. Use ';' to separate the indices.
  * Possibility to select behavior between two xpath elements: _Noting_ (current behavior), _Empty line_ or _Repeat Header_.

**version 1.2.0 (22 August 10):**
  * Fix update issues with icons.
  * Possibility to select a score as the start page.
  * Add an option for scores using an RTL language (Right to Left) to reverse the column order.

**version 1.1 (13 dec 09):**
  * Possibility to wrap text and to show new lines.
  * Config: Preview formatting in test tab.
  * Config: Set alignment by right-clicking a column in the grid.
  * Config: The 'Include THeader Rows' now includes (or excludes) the TFooter rows as well.
  * Config: Highlight the Save button when the score's setting has been changed.
  * Config: Improve performance when creating or copying a node.
  * Skin: Add a 'dummy' label control with ID=50. If the control is present in the skin then the list will be invisible when a score is displayed. This is feature is designed for small screen resolution.

**version 1.0 (5 nov 09):**
  * Fix: issue with listcontrol page label.
  * Add Russian language file (thanks to bounguine).
  * New Propery "#ScoreCenter.Source" to display the name of the source (web site).
  * Download icons zip file and unzip it if icons are missing after update.
  * Remember navigation (reselect previous item when going back).

**version 0.11 (22 sep 09):**
  * Rule: Format is not required to create a rule
  * Internationalzation of the plugin
  * Config: New option to include (default) or not the `<theader>` tags when parsing a table
  * Config: Display the total width of the headers to give an idea if it will fit on one page
  * Fix: pb with encoding value in the downloaded HTML

**version 0.10 (3 Sep 09):**
  * Online synchronization
  * New Rule: IsNull
  * New Rule Formatting: MergeCells and ReplaceText
  * Cell Alignment: use '+' to center text
  * Paging for long score
  * Possibility to order the results in the configuration
  * Configuration: improve performance of the building of the tree and saving

**version 0.9 (19 Aug 09):**
  * Management of icons from the configuration
  * Import/Export to exchange settings
  * Set the name of the plugin in configuration
  * Rename categories/leagues from the tree
  * Enable checkbox in the tree to disable/enable a full category
  * Context menu in the plugin to deactivate elements
  * Context menu to enable the "Auto Size" mode (in case the website changes the table)
  * Property to set the encoding of the web page, button to go visit the web page from the configuration
  * Variable URL (with date elements like {M\_yyyy} ==> 8\_2009)
  * XPath can include any type of elements (not just tables), non table element are displayed on a single line (for example a list of div elements)
  * Additional field to retrieve a specific element in the XPath result
  * Post processing rules to format results

**version 0.5 (12 Jul 09) initial public release**
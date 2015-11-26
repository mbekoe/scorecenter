# Introduction #

There are three different things that can be translated:
  * the plugin's strings (menu, ...),
  * the scoress names (shown in the navigation part),
  * the scores content.

The best way to create a new language file is to copy an existing and rename it with your language code and then replace the strings with the translation.

Do not forget to **share** it with the MediaPortal community on the [forum](http://forum.team-mediaportal.com/mediaportal-plugins-47/my-score-center-67687/) and then I will add it to the distribution.

All the files are in the MediaPortal data directory `%ProgramData%\Team MediaPortal\MediaPortal\language`.

# Plugin #

The internationalization of the plugin itself uses the standard MediaPortal way.

The files are named `strings_<lang>.xml`.

```
<?xml version="1.0" encoding="UTF-8"?>
<Language name="English" characters="255">
  <Section name="unmapped">
    <String id="1">Clear the cache</String>
    ...
  </Section>
</Language>
```

# Name of Scores #

In the default settings the name of the scores are always in English.
If you want to translate them you will need to do it in the file named `scores_<lang>.xml`.

```
<?xml version="1.0" encoding="utf-8"?>
<ScoreLocalisation>
  <Strings>
    <LocString id="Athletics">Athlétisme</LocString>
    <LocString id="Olympics">Jeux Olympiques</LocString>
    ...
  </Strings>
  <Globals>
    <LocString id="Today">Aujourd'hui</LocString>
    <LocString id="Results">Résultats</LocString>
    <LocString id="Group[\s*](?&lt;grp&gt;\.*)" isRegEx="true">Groupe ${grp}</LocString>
    ...
  </Globals>
</ScoreLocalisation>
```

The first section `<Strings>` is used to translate scores using their id.
The second section `<Globals>` is used to translate globally all scores. For example, if all scores named "Today" will be translate the same way.
It is also possible to use Regular Expressions by setting the flag _isRegEx_ to true (default is false). This is useful to translate similar strings. For example "Group A" and "Group B" are respectively translated to "Groupe A" and "Groupe B".

# Scores #

It is also possible to translate the content of the scores.
There are two ways to do that, by using dictionaries or rules.

## Using Dictionaries ##

The `scores_<lang>.xml` file can also be used to define dictionaries.

```
<?xml version="1.0" encoding="utf-8"?>
<ScoreLocalisation>
  ...
  <ScoreDictionaries>
    <ScoreDictionary name="tennis">
    <LocString id="Roger Federer">Roger</LocString>
  </ScoreDictionary>
</ScoreDictionaries>
```

A dictionary must have a name and you can have many dictionaries.
To use a dictionary you need to set the name of the dictionary you want to use in the score configuration (on the "Post Processing" tab).

**important**: when you use a dictionary each element of the score is processed with all element in the dictionary. So it is important to avoid big dictionary and better create a small dictionary for each category of scores.

## Using Rules ##

It is also possible to do text replacement by using a Rule (third tab in the configuration of the score).

For example, if in the first column instead of "1" you want to see "One", then create a rule for your score with the following parameter:
  * Column: 1 (0 means any column, -1 means the rank of the line)
  * Operator: =
  * Value: 1,one
  * Action: Replace Text
when using 'Replace text' the value field must contain the string to replace and the new value separated by a comma.
# How to create a score #

First you'll need to find a web page which displays the results you want.
ScoreCenter works only for results displayed in HTML (no flash) and it works especially well when the result is displayed in an HTML table.

It is also important to choose a page which displayed up to date results. For example, it should not contains a year or a round number.

Then you will need to know a little about XPath. The [w3schools](http://www.w3schools.com/xpath/default.asp) has a great tutorial on this (and other subjects too).

# Simple example #
Let's try to get the standings for AFL standings from [foxsports.com.au](http://www.foxsports.com.au/afl/results).

Open the page in your browser and look at the source (ctr+U) in firefox. Then search for something in the table like the title (AFL Ladder) or the first team.
Here we find this code.
```
<div class="thumb-halfbar2 clear">

        <h2><strong><a href="http://www.foxsports.com.au/results">AFL Ladder</a></strong></h2>
        
    <table class="data4" cellspacing="0">
        <caption>Round 22 (Updated Sep 11)</caption>
        <thead>
        <tr>
            <th scope="col">&nbsp;</th>
            <th scope="col" title="Team" class="team"></th>

```

What we want is the table after the H2 title "AFL Ladder". This table has a class attribute with the value "data4". So the XPath expression to use to get this table is:
```
//table[@class='data4']
```
Which means "get all table with class attribute = 'data4'.

In this case there is only one table with this attribute, but if there are more than one and you only want the second one, use the _XPath Element_ field to set the index (0-based) of the table to get.
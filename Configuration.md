# Navigation #
The different scores are classified by _Category_ and _League_.
A category is for example a sport, like football (or soccer) and a league a championship for this sport. Then for a league you can have different scores: last results, standing...


---

# Score Definition #
A score is defined by an URL and an XPath expression (both are required).

The XPath expression can include any type of HTML elements. If the element is an HTML table, then its lines will be processed. For all other kind of elements the inner text (i.e. the html without the tags) will be output as a line.

Example:
|//table|all the tables in the document|
|:------|:-----------------------------|
|//table`[`@class='score'`]`|all the tables with the class attributes equal to 'score'|
|//div`[`@id='team'`]`//table`[`1`]`|the first table included in a div with id='team'|

You can select several path with the operator |
Ex: //table[@id='1'] | //table[@id='2']


---

# Additional Parsing Rules #
## Variables URL ##
Sometimes the URL is not fixed and depend on the date (current year or current month). To define a variable url use {} and [.Net custom date formatting](http://msdn.microsoft.com/en-us/library/8kb3ddd4(VS.96).aspx).

Example: if date is May 25th 2011
|`{`dd\_M\_yy`}`|25\_5\_11|
|:--------------|:--------|
|`{`yyyyMMdd`}` |20110525 |

Season tags: these tags are made to show a season, new seasons usually change during the summer in July.
<table border='1'>
<tr>
<th>tag</th>
<th>Season 10/11</th>
<th>Season 11/12</th>
<th>Season 12/13</th>
</tr>
<tr>
<th>01/11-06/11</th>
<th>07/11-12/11</th>
<th>01/12-06/12</th>
<th>07/12-12/12</th>
</tr>
<tr><td>{YY-YY+1}</td><td>10-11</td><td>11-12</td><td>12-13</td></tr>
<tr><td>{YYYY-YY+1}</td><td>2010-11</td><td>2011-12</td><td>2012-13</td></tr>
<tr><td>{YYYY-YYYY+1}</td><td>2010-2011</td><td>2011-2012</td><td>2012-2013</td></tr>
<tr><td>{YYYY+1}</td><td>2011</td><td>2012</td><td>2013</td></tr>
<tr><td>{YY+1}</td><td>11</td><td>12</td><td>13</td></tr>
<tr><td>{YYYY-1}</td><td>2010</td><td>2011</td><td>2012</td></tr>
<tr><td>{YY-1}</td><td>10</td><td>11</td><td>12</td></tr>
</table>

## XPath Element ##
Sometimes it is difficult to find an expression XPath that returns only the desired element. To make it easier, you can enter the number (0-based) of the element in the "Element field", only this element will be processed.

## Skip, Max Lines and Headers ##

The skip property indicates the number of lines to skip.
Max Lines indicates the maximum number of lines to include in the results.
_Note_ these properties apply to each tables returned by the XPath expression.

The header property allows you to enter your own header.
_Note_ the header line will be displayed only once.

The Sizes field allows you to specify the size of each columns.
If the size is 0 the column will be skipped.
By default the columns are align to the left.
If the size is preceed with a minus sign '-', the column will be align to the right.
If the size is preceed with a plus sign '+', the column will be centered.


---

# Formatting the results #
It is possible to format the results by applying rules to it.
Rules can be defined in the configuration screen in the _Rules_ tab.
The rules are defined for the current score.

A rule is composed of:
  * a column number
    * 1-based index of the column
    * -1 means the line number
    * 0 means any columns
  * an operator
    * equal, not equal, <, <=, >, >=
    * contains, not contains, starts/ends with, not starts/ends with
    * in list
    * modulo
  * a value
    * in list: values are separated by a comma => 1,2,3
    * modulo: the value is the divisor and the test is true if the result is 0
  * an action
    * format the cell
    * format the line containing the cell
  * a style to apply the format
    * styles are defined separately and contain only the font color

The default configuration file contains several examples.
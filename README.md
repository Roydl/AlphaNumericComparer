<p align="center">
<a href="https://dotnet.microsoft.com/download/dotnet/5.0" rel="nofollow"><img src="https://img.shields.io/badge/core-v3.1%20%7C%20v5.0-lightgrey.svg?style=flat&amp;logo=dot-net&amp;logoColor=white" alt="Platform"></a>
<a href="https://github.com/Roydl/AlphaNumericComparer/actions/workflows/dotnet.yml"><img src="https://github.com/Roydl/AlphaNumericComparer/actions/workflows/dotnet.yml/badge.svg" alt="Build"></a>
<a href="https://github.com/Roydl/AlphaNumericComparer/commits/master"><img src="https://img.shields.io/github/last-commit/Roydl/AlphaNumericComparer.svg?style=flat&amp;logo=github&amp;logoColor=silver" alt="Commits"></a>
<a href="https://github.com/Roydl/AlphaNumericComparer/blob/master/LICENSE.txt"><img src="https://img.shields.io/github/license/Roydl/AlphaNumericComparer.svg?style=flat" alt="License"></a>
</p>
<p align="center">
<a href="https://www.nuget.org/packages/Roydl.AlphaNumericComparer" rel="nofollow"><img src="https://img.shields.io/nuget/v/Roydl.AlphaNumericComparer.svg?style=flat&amp;logo=nuget&amp;logoColor=silver&amp;label=nuget" alt="NuGet"></a>
<a href="https://github.com/Roydl/AlphaNumericComparer/archive/master.zip"><img src="https://img.shields.io/badge/download-source-yellow.svg?style=flat&amp;logo=github&amp;logoColor=silver" alt="Source"></a>
<a href="https://www.si13n7.com" rel="nofollow"><img src="https://img.shields.io/website/https/www.si13n7.com.svg?style=flat&amp;down_color=red&amp;down_message=offline&amp;up_color=limegreen&amp;up_message=online&amp;logo=data%3Aimage%2Fpng%3Bbase64%2CiVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEwSURBVDhPxZJNSgNBEIXnCp5AcCO4CmaTRRaKBhdCFkGCCKLgz2Y2RiQgCiqZzmi3CG4COj0X8ApewSt4Ba%2FQ9leZGpyVG8GComtq3qv3qmeS%2Fw9nikHMd5sVn3bqLx7zom1NcW8z%2F6G9CjoPm722rPEv45EJ21vD0O30AvX12IWDvTRsrPXrnjPlUYO0u3McVpZXhch5cnguZ7vVDWfpjRAZgPqc%2BIMEgKQe9Pfr0xn%2FBqZJjAUNQKilp5cC1gHYYz8Usc3OQsTz9HZWK5BMJwFDwrbWbuIXhfhg%2FDpWuE2mK5lEgQtiz4baU14u3V09i5peiipy6qVAxFWtZiflJiq8AAiIZx1CnxpStGmEpEHDZf4r2pUd%2BMjYxomoxJofo4L%2FHqyR57OF6vEvIkm%2BAYRc%2BWd4P97CAAAAAElFTkSuQmCC" alt="Website"></a>
<a href="https://www.si13n7.de" rel="nofollow"><img src="https://img.shields.io/website/https/www.si13n7.de.svg?style=flat&amp;down_color=red&amp;down_message=offline&amp;label=mirror&amp;up_color=limegreen&amp;up_message=online&amp;logo=data%3Aimage%2Fpng%3Bbase64%2CiVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEwSURBVDhPxZJNSgNBEIXnCp5AcCO4CmaTRRaKBhdCFkGCCKLgz2Y2RiQgCiqZzmi3CG4COj0X8ApewSt4Ba%2FQ9leZGpyVG8GComtq3qv3qmeS%2Fw9nikHMd5sVn3bqLx7zom1NcW8z%2F6G9CjoPm722rPEv45EJ21vD0O30AvX12IWDvTRsrPXrnjPlUYO0u3McVpZXhch5cnguZ7vVDWfpjRAZgPqc%2BIMEgKQe9Pfr0xn%2FBqZJjAUNQKilp5cC1gHYYz8Usc3OQsTz9HZWK5BMJwFDwrbWbuIXhfhg%2FDpWuE2mK5lEgQtiz4baU14u3V09i5peiipy6qVAxFWtZiflJiq8AAiIZx1CnxpStGmEpEHDZf4r2pUd%2BMjYxomoxJofo4L%2FHqyR57OF6vEvIkm%2BAYRc%2BWd4P97CAAAAAElFTkSuQmCC" alt="Mirror"></a>
</p>

# Roydl.AlphaNumericComparer

Types of `IComparer` that enables the alphanumeric comparison of two objects.

| Default Comparer | Alphanumeric Comparer |
| ---- | ---- |
| Alpha10000 | Alpha111 |
| Alpha111 | Alpha1150 |
| Alpha1150 | Alpha10000 |
| Foxtrot10000 | Foxtrot111 |
| Foxtrot111 | Foxtrot1150 |
| Foxtrot1150 | Foxtrot10000 |
| Oscar10000 | Oscar111 |
| Oscar111 | Oscar1150 |
| Oscar1150 | Oscar10000 |


#### Usage:
```cs
// Can be used in the same way as all `IComparer`.
var sortedDictionary = new SortedDictionary<string, object>(new AlphaNumericComparer<string>());
var sortedList = new SortedList<string, object>(new AlphaNumericComparer<string>());
var unsortedCollection  = new string[] { /* some strings */ };
var sortedCollection = unsorted.OrderBy(str => str, new AlphaNumericComparer<string>());

// Can even be used to sort `System.Windows.Forms` elements or the like.
// In case of `System.Windows.Forms.ListView` you just have to set
// the `ListViewItemSorter` field to automatically sort the list items.
myListView.ListViewItemSorter = new AlphaNumericComparer();
```

---


## Would you like to help?

- [Star this Project](https://github.com/Roydl/AlphaNumericComparer/stargazers) :star: and show me that this project interests you :hugs:
- [Open an Issue](https://github.com/Roydl/AlphaNumericComparer/issues/new) :coffee: to give me your feedback and tell me your ideas and wishes for the future :sunglasses:
- [Open a Ticket](https://support.si13n7.de/) :mailbox: if you don't have a GitHub account, you can contact me directly on my website :wink:
- [Donate by PayPal](http://donate.si13n7.com/) :money_with_wings: to buy me some cookies :cookie:

